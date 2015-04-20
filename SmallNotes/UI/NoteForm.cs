using ColorCode;
using Common.Data.Async;
using CommonMark;
using CommonMark.Syntax;
using log4net;
using SmallNotes.Data;
using SmallNotes.Data.Cache;
using SmallNotes.Data.Entities;
using SmallNotes.Properties;
using SmallNotes.UI.Utils;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace SmallNotes.UI
{
	public partial class NoteForm : Form
	{
		#region Properties

		#region Note data property
		private Note _Data;
		public Note Data
		{
			get
			{
				return _Data;
			}
			set
			{
				_Data = value;
				EditMode = _Data == null;
			}
		}
		#endregion

		#region EditMode property
		private bool _EditMode;
		public bool EditMode
		{
			set
			{
				_EditMode = value;
				FormBorderStyle = _EditMode ? System.Windows.Forms.FormBorderStyle.SizableToolWindow : System.Windows.Forms.FormBorderStyle.None;
				ControlBox = _EditMode;
				editorPanel.Visible = _EditMode;
				displayPanel.Visible = !_EditMode;
				TopMost = !_EditMode;
				UpdateUI();
			}
			get
			{
				return _EditMode;
			}
		}
		#endregion

		#region CustomStylesheet property

		private string _CustomStylesheet;
		public string CustomStylesheet
		{
			get
			{
				return _CustomStylesheet;
			}
			set
			{
				_CustomStylesheet = value;
				UpdateUI();
			}
		}

		#endregion

		public string AppDataPath { get; set; }

		public bool FastResizeMove { get; set; }

		private long _IdleTimeout;
		public long IdleTimeout
		{
			get
			{
				return _IdleTimeout;
			}
			set
			{
				_IdleTimeout = value;
				if (_IdleTimeout < 0) _IdleTimeout = 0;
				if (_IdleTimeout < 1) SetTitleDisplayVisible(true);
				else MouseActivityForTitleBar();
			}
		}

		public string DefaultBackgroundColor
		{
			set
			{
				if (value != null)
				{
					BackColor = ColorTranslator.FromHtml(value);
				}
			}
		}

		public string DefaultForegroundColor
		{
			set
			{
				if (value != null)
				{
					ForeColor = ColorTranslator.FromHtml(value);
				}
			}
		}

		public Func<Note> NoteFactory { get; set; }

		private ILog Logger { get; set; }

		#endregion

		#region Static Properties

		public static string StylesheetTemplate { get; set; }
		public static string DocumentTemplate { get; set; }
		public static Dictionary<string, Tag> TagList { get; set; }

		#endregion

		#region Private members

		private FileCache _FileCache;

		private Point _formLocation;
		private Size _formSize;
		private Point _mouseDown;
		private bool _moveCapture;
		private bool _moveMoved;
		private bool _resizeCapture;
		private bool _resizeResized;
		private bool _lastUpFromDrag;

		private bool _automaticForeColor = false;

		private ToolStripMenuItem customBackgroundColorMenuItem;

		private bool _closeNoSave = false;

		private CancellationTokenSource _tokenSource;

		private List<Tag> _selectedTags;

		#endregion

		public NoteForm(string appDataPath, ColorList backgroundColorList, FileCache cache)
		{
			Logger = LogManager.GetLogger(GetType());
			_FileCache = cache;
			AppDataPath = appDataPath;
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, true);
			CustomStylesheet = "";
			displayBrowser.ImageLoad += _FileCache.AsyncImageLoadHandler;
			displayBrowser.StylesheetLoad += _FileCache.AsyncStylesheetLoadHandler;

			// Populate color menu
			if (backgroundColorList.Items.Count > 0)
			{
				foreach (ColorList.ColorItem item in backgroundColorList.Items)
				{
					ToolStripMenuItem newItem = new ToolStripMenuItem();
					newItem.Text = item.Name;
					newItem.Image = item.Icon;
					newItem.Click += backgroundColorMenuItem_Click;
					newItem.Tag = item.HexColor;
					backgroundColorDropDownButton.DropDownItems.Add(newItem);
				}
				backgroundColorDropDownButton.DropDownItems.Add(new ToolStripSeparator());
			}
			customBackgroundColorMenuItem = new ToolStripMenuItem();
			customBackgroundColorMenuItem.Text = Resources.CustomMenuItem;
			customBackgroundColorMenuItem.Image = ColorList.DrawEmptyCustomIcon();
			customBackgroundColorMenuItem.Click += customBackgroundColorMenuItem_Click;
			backgroundColorDropDownButton.DropDownItems.Add(customBackgroundColorMenuItem);

			// Populate foreground color menu icons
			blackForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("Black") { Color = Color.Black }.Icon;
			whiteForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("White") { Color = Color.White }.Icon;
		}

		public void CloseNoSave()
		{
			_closeNoSave = true;
			Close();
		}

		#region Utility methods

		private void UpdateUI()
		{

			// Window title
			Text = _EditMode ? (Data != null ? Data.Title : Resources.NewNoteTitle) : String.Empty;

			// Editor
			titleTextBox.Text = Data != null ? Data.Title : Resources.NewNoteTitle;
			markdownTextBox.Text = Data != null ? Data.Text : String.Empty;
			_automaticForeColor = Data == null || !Data.ForegroundColor.HasValue;
			if (Data != null) BackColor = Data.BackgroundColor;
			ForeColor = ComputeForegroundColor(Data, BackColor);
			RedrawAutomaticMenuItemIcon();
			_selectedTags = Data != null ? Data.Tags : new List<Tag>();

			// Display
			if (Data != null)
			{
				string documentHtml = RenderNoteToHtml(Data, CustomStylesheet);

				// Display the note
				displayBrowser.BackColor = BackColor;
				displayBrowser.ForeColor = ForeColor;
				displayBrowser.Text = documentHtml;
				titleDisplayLabel.Text = Data.Title;

				// Ensure correct location and size
				StartPosition = FormStartPosition.Manual;
				if (!Data.Dimensions.IsEmpty) this.Size = Data.Dimensions;
				if (!Data.Location.IsEmpty) this.Location = Data.Location;

				// Display the bottom title bar (possibly temporarily)
				MouseActivityForTitleBar();
			}
		}

		#region GetCommonMarkSettings for custom HTML rendering

		private static CommonMarkSettings _CommonMarkSettings;

		public static CommonMarkSettings GetCommonMarkSettings()
		{
			if (_CommonMarkSettings == null)
			{
				_CommonMarkSettings = CommonMarkSettings.Default.Clone();
				_CommonMarkSettings.OutputDelegate = new Action<Block, TextWriter, CommonMarkSettings>((block, target, settings) => new CustomHtmlFormatter(target, settings).WriteDocument(block));
			}
			return _CommonMarkSettings;
		}

		private class CustomHtmlFormatter : CommonMark.Formatters.HtmlFormatter
		{
			private CodeColorizer _colorizer;

			private TextWriter _writer;

			public CustomHtmlFormatter(TextWriter target, CommonMarkSettings settings) : base(target, settings)
			{
				_writer = target;
				_colorizer = new CodeColorizer();
			}

			protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
			{
				ignoreChildNodes = false;
				if (block.Tag == BlockTag.FencedCode && block.FencedCodeData != null)
				{
					string langName = block.FencedCodeData.Info;
					string title = null;
					int colonSearch = langName.IndexOf(':');
					if (colonSearch > -1)
					{
						title = langName.Substring(colonSearch + 1);
						langName = langName.Substring(0, colonSearch);
					}
					if (!string.IsNullOrEmpty(langName))
					{
						ILanguage lang = Languages.FindById(langName);
						if (lang != null)
						{
							if (title == null) title = lang.Name;
							_colorizer.Colorize(block.StringContent.ToString(), lang, new NoteCodeFormatter(title), StyleSheets.Default, _writer);
							ignoreChildNodes = true;
						}
					}
				}
				if (!ignoreChildNodes)
				{
					base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
				}
			}

			protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
			{
				if (inline.Tag == InlineTag.Image)
				{
					ignoreChildNodes = false;
					if (isOpening)
					{
						Func<string,string> uriResolver = this.Settings.UriResolver;
						string uri = uriResolver != null ? uriResolver(inline.TargetUrl) : inline.TargetUrl;
						this.Write("<a href=\"");
						this.WriteEncodedUrl(uri);
						this.Write("\"><img src=\"");
						this.WriteEncodedUrl(uri);
						this.Write("\" alt=\"");
						if (!isClosing)
						{
							this.RenderPlainTextInlines.Push(true);
						}
					}

					if (isClosing)
					{
						// this.RenderPlainTextInlines.Pop() is done by the plain text renderer above.

						this.Write('\"');
						if (inline.LiteralContent.Length > 0)
						{
							this.Write(" title=\"");
							this.WriteEncodedHtml(inline.LiteralContent);
							this.Write('\"');
						}

						if (this.Settings.TrackSourcePosition) this.WritePositionAttribute(inline);
						this.Write(" /></a>");
					}
				}
				else
				{
					base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
				}
			}
		}

		#endregion

		private void CheckRadioMenu(ToolStripDropDownItem menu, ToolStripItem selectedItem)
		{
			foreach (ToolStripItem item in menu.DropDownItems)
			{
				if (item != null && item is ToolStripMenuItem)
				{
					((ToolStripMenuItem)item).Checked = item == selectedItem;
				}
			}
		}

		private bool IsCursorOverResizeGripper()
		{
			Point cursorPos = titleDisplayLabel.PointToClient(Cursor.Position);
			Rectangle gripperArea = new Rectangle(titleDisplayLabel.Width - UIElements.RESIZE_GRIPPER_SIZE, 0, UIElements.RESIZE_GRIPPER_SIZE, titleDisplayLabel.Height);
			return gripperArea.Contains(cursorPos);
		}

		private void CheckCursor()
		{
			if (displayPanel.Visible && IsCursorOverResizeGripper())
			{
				Cursor = Cursors.SizeNWSE;
			}
			else
			{
				Cursor = Cursors.Default;
			}
		}

		private void RedrawAutomaticMenuItemIcon()
		{
			automaticForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("Auto") { Color = GetAutomaticForegroundColor(BackColor) }.Icon;
		}

		private void UpdateLocationAndSize()
		{
			if (Data != null)
			{
				Data.Location = this.Location;
				Data.Dimensions = this.Size;
				OnNoteUpdated();
			}
		}

		private async void MouseActivityForTitleBar()
		{
			// Give focus
			displayScrollPanel.Focus();

			// Make sure the status is visible
			SetTitleDisplayVisible(true);

			// Cancel hide timeout
			if (_tokenSource != null)
			{
				_tokenSource.Cancel();
			}

			// Start timeout for hiding Titlebar
			if (IdleTimeout > 0)
			{
				try
				{
					_tokenSource = new CancellationTokenSource();
					await Task.Delay(TimeSpan.FromMilliseconds(IdleTimeout), _tokenSource.Token);
					if (!Bounds.Contains(Control.MousePosition))
					{
						SetTitleDisplayVisible(false);
					}
					_tokenSource = null;
				}
				catch (TaskCanceledException)
				{
					// Do nothing
				}
			}
		}

		private void SetTitleDisplayVisible(bool visible)
		{
			displayScrollPanel.Height = Height - ((visible ? titleDisplayLabel.Height : 0) + 4);
			displayScrollPanel.AutoScroll = visible;

			// TODO Fade animation
			titleDisplayLabel.Visible = visible;
		}

		#region Static

		private static Color ComputeForegroundColor(Note note, Color backgroundColor)
		{
			return note != null && note.ForegroundColor.HasValue ? note.ForegroundColor.Value : GetAutomaticForegroundColor(backgroundColor);
		}

		private static bool IsBackgroundDark(Color backColor)
		{
			float value = backColor.R * 0.299f + backColor.G * 0.587f + backColor.B * 0.114f;
			return value < 186;
		}

		public static string RenderNoteToHtml(Note note, string customCss)
		{
			// Compute link color
			// ForegroundColor == Automatic: LinkColor = AliceBlue for dark backgrounds, Blue for light backgrounds
			// ForegroundColor != Automatic: LinkColor = ForegroundColor
			Color linkColor = note.ForegroundColor.HasValue ? note.ForegroundColor.Value : IsBackgroundDark(note.BackgroundColor) ? Color.AliceBlue : Color.Blue;

			// Parse Markdown to HTML
			string bodyHtml = CommonMarkConverter.Convert(note.Text, GetCommonMarkSettings());

			// Process _StylesheetTemplate
			SimpleTemplate styleTemplate = new SimpleTemplate() { Template = StylesheetTemplate };
			styleTemplate["ForeColor"] = ColorTranslator.ToHtml(ComputeForegroundColor(note, note.BackgroundColor));
			styleTemplate["BackColor"] = ColorTranslator.ToHtml(note.BackgroundColor);
			styleTemplate["LinkColor"] = ColorTranslator.ToHtml(linkColor);
			styleTemplate["CustomStylesheet"] = customCss;

			// Process _DocumentTemplate
			SimpleTemplate documentTemplate = new SimpleTemplate() { Template = DocumentTemplate };
			documentTemplate["Title"] = note.Title;
			documentTemplate["StyleSheet"] = styleTemplate.Render();
			documentTemplate["BodyHtml"] = bodyHtml;
			return documentTemplate.Render();
		}

		public static Color GetAutomaticForegroundColor(Color backColor)
		{
			return IsBackgroundDark(backColor) ? Color.White : Color.Black;
		}

		#endregion

		#endregion

		#region Event handlers

		private void saveButton_Click(object sender, EventArgs e)
		{
			// Update the UI
			if (Data != null)
			{
				Data.Text = markdownTextBox.Text;
				Data.Title = titleTextBox.Text;
				Data.BackgroundColor = BackColor;
				Data.ForegroundColor = _automaticForeColor ? (Color?)null : ForeColor;
				Data.Location = this.Location;
				Data.Dimensions = this.Size;
				Data.Tags = _selectedTags;
				EditMode = false;
			}
			else
			{
				Note newNote = NoteFactory.Invoke();
				newNote.Text = markdownTextBox.Text;
				newNote.Title = titleTextBox.Text;
				newNote.BackgroundColor = BackColor;
				newNote.ForegroundColor = _automaticForeColor ? (Color?)null : ForeColor;
				newNote.Visible = true;
				newNote.Dimensions = this.Size;
				newNote.Location = this.Location;
				newNote.Tags = _selectedTags;
				Data = newNote;
			}

			// Save the note
			OnNoteUpdated();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (Data != null)
			{
				EditMode = false;
			}
			else
			{
				Close();
			}
		}

		private void displayBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!EditMode)
			{
				EditMode = true;
			}
			_moveCapture = false;
		}

		private void displayBrowser_LinkClicked(object sender, HtmlLinkClickedEventArgs e)
		{
			if (!_lastUpFromDrag)
			{
				Logger.DebugFormat("Link clicked: '{0}'", e.Link);
				// Open links in the user's default browser (Chrome, etc.)
				Process.Start(e.Link);
			}
			e.Handled = true;
		}

		private void displayBrowser_MouseDown(object sender, MouseEventArgs e)
		{
			_moveCapture = true;
			_mouseDown = e.Location;
			_formLocation = ((Form)TopLevelControl).Location;
			_lastUpFromDrag = false;
		}

		private void displayBrowser_MouseMove(object sender, MouseEventArgs e)
		{
			if (_moveCapture)
			{
				if (FastResizeMove) displayBrowser.DisableRedraw();
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
				((Form)TopLevelControl).Location = newLocation;
				_formLocation = newLocation;
				_moveMoved = true;
				if (_lastUpFromDrag || Math.Abs(newLocation.X - _mouseDown.X) > SystemInformation.DoubleClickSize.Width || Math.Abs(newLocation.Y - _mouseDown.Y) > SystemInformation.DoubleClickSize.Height)
				{
					_lastUpFromDrag = true;
				}
				Invalidate();
			}

			MouseActivityForTitleBar();
		}

		private void displayBrowser_MouseUp(object sender, MouseEventArgs e)
		{
			displayBrowser.EnableRedraw();
			if (_moveMoved)
			{
				UpdateLocationAndSize();
			}
			_moveMoved = false;
			_moveCapture = false;
		}

		private void titleDisplayLabel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!EditMode)
			{
				EditMode = true;
			}
			_moveCapture = false;
			_resizeCapture = false;
		}

		private void titleDisplayLabel_MouseDown(object sender, MouseEventArgs e)
		{
			_formLocation = ((Form)TopLevelControl).Location;
			_formSize = ((Form)TopLevelControl).Size;
			if (IsCursorOverResizeGripper())
			{
				_resizeCapture = true;
				_mouseDown = titleDisplayLabel.PointToScreen(e.Location);
			}
			else
			{
				_mouseDown = e.Location;
				_moveCapture = true;
			}
		}

		private void titleDisplayLabel_MouseMove(object sender, MouseEventArgs e)
		{
			if (_moveCapture)
			{
				if (FastResizeMove) displayBrowser.DisableRedraw();
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
				((Form)TopLevelControl).Location = newLocation;
				_formLocation = newLocation;
				_moveMoved = true;
				Invalidate();
			}
			if (_resizeCapture)
			{
				if (FastResizeMove) displayBrowser.DisableRedraw();
				Point loc = titleDisplayLabel.PointToScreen(e.Location);
				int dx = loc.X - _mouseDown.X;
				int dy = loc.Y - _mouseDown.Y;
				Size newSize = new Size(_formSize.Width + dx, _formSize.Height + dy);
				Size oldSize = ((Form)TopLevelControl).Size;
				((Form)TopLevelControl).Size = newSize;
				_resizeResized = true;
			}

			MouseActivityForTitleBar();
			CheckCursor();
		}

		private void titleDisplayLabel_MouseUp(object sender, MouseEventArgs e)
		{
			displayBrowser.EnableRedraw();
			if (_moveMoved || _resizeResized)
			{
				UpdateLocationAndSize();
			}
			_moveCapture = false;
			_moveMoved = false;
			_resizeCapture = false;
			_resizeResized = false;
		}

		private void titleDisplayLabel_MouseEnter(object sender, EventArgs e)
		{
			CheckCursor();
		}

		private void titleDisplayLabel_MouseLeave(object sender, EventArgs e)
		{
			CheckCursor();
		}

		private void backgroundColorMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			CheckRadioMenu(backgroundColorDropDownButton, item);
			BackColor = ColorTranslator.FromHtml((string)item.Tag);
		}

		private void customBackgroundColorMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(backgroundColorDropDownButton, (ToolStripItem)sender);

			// Launch custom color dialog
			colorPickerDialog.Color = BackColor;
			colorPickerDialog.FullOpen = true;
			if (colorPickerDialog.ShowDialog() == DialogResult.OK)
			{
				ColorList.ColorItem customItem = new ColorList.ColorItem("");
				customItem.HexColor = ColorTranslator.ToHtml(colorPickerDialog.Color);
				customBackgroundColorMenuItem.Image = customItem.Icon;
				BackColor = colorPickerDialog.Color;
			}
		}

		private void automaticForegroundColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(foregroundColorDropDownButton, (ToolStripItem)sender);
			_automaticForeColor = true;
			ForeColor = GetAutomaticForegroundColor(BackColor);
		}

		private void blackForegroundColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(foregroundColorDropDownButton, (ToolStripItem)sender);
			_automaticForeColor = false;
			ForeColor = Color.Black;
		}

		private void whiteForegroundColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(foregroundColorDropDownButton, (ToolStripItem)sender);
			_automaticForeColor = false;
			ForeColor = Color.White;
		}

		private void customForegroundColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(foregroundColorDropDownButton, (ToolStripItem)sender);

			// Launch custom color dialog
			colorPickerDialog.Color = ForeColor;
			colorPickerDialog.FullOpen = true;
			if (colorPickerDialog.ShowDialog() == DialogResult.OK)
			{
				customForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("") { Color = colorPickerDialog.Color }.Icon;
				_automaticForeColor = false;
				ForeColor = colorPickerDialog.Color;
			}
		}

		private void titleTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			Text = titleTextBox.Text;
		}

		private void NoteForm_Activated(object sender, EventArgs e)
		{
			if (EditMode)
			{
				markdownTextBox.Focus();
				markdownTextBox.SelectionStart = markdownTextBox.Text.Length;
				markdownTextBox.SelectionLength = 0;
				markdownTextBox.ScrollToCaret();
			}
		}

		private void NoteForm_BackColorChanged(object sender, EventArgs e)
		{
			// Redraw image for main button
			Bitmap bm = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(bm);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Brush colorBrush = new SolidBrush(BackColor);
			g.FillRectangle(colorBrush, 0, 0, 16, 16);
			g.DrawString("A", new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(GetAutomaticForegroundColor(BackColor)), 2, 0);
			backgroundColorDropDownButton.Image = bm;

			// Redraw image for automatic color item
			RedrawAutomaticMenuItemIcon();

			// Recalculate automatic foreground color if necessary
			if (_automaticForeColor) ForeColor = GetAutomaticForegroundColor(BackColor);
		}

		private void NoteForm_ForeColorChanged(object sender, EventArgs e)
		{
			// Redraw image for main button
			Bitmap bm = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(bm);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.DrawString("A", new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(ForeColor), 2, 0);
			foregroundColorDropDownButton.Image = bm;
		}

		private void NoteForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && !_closeNoSave && Data != null)
			{
				Data.Visible = false;
				OnNoteUpdated();
			}
		}

		private void tagsToolStripButton_Click(object sender, EventArgs e)
		{
			// Launch Note -> Tags editor
			NoteTagForm noteTagForm = new NoteTagForm();
			noteTagForm.Owner = this;
			noteTagForm.NoteTitle = Text;
			noteTagForm.TagList = TagList.Values.ToList();
			if (Data != null)
			{
				noteTagForm.SelectedTagList = _selectedTags;
			}
			if (noteTagForm.ShowDialog() == DialogResult.OK)
			{
				_selectedTags = noteTagForm.SelectedTagList;
			}
		}

		private void titleDisplayLabel_Paint(object sender, PaintEventArgs e)
		{
			// Only draw if we actually have something to draw
			if (Data != null)
			{
				// Setup graphics environment
				Graphics g = e.Graphics;
				Rectangle clipRect = e.ClipRectangle;
				g.FillRectangle(new SolidBrush(BackColor), clipRect);
				g.DrawLine(new Pen(titleDisplayLabel.ForeColor, 1), clipRect.Location, new Point(clipRect.Right, clipRect.Top));
				g.SmoothingMode = SmoothingMode.AntiAlias;
				Font boldFont = new Font(titleDisplayLabel.Font, FontStyle.Bold);
				Font italicFont = new Font(titleDisplayLabel.Font, FontStyle.Italic);

				// Draw custom colored resize gripper
				UIElements.DrawColoredResizeGripper(g, clipRect, ForeColor);
				clipRect.Width -= UIElements.RESIZE_GRIPPER_SIZE;

				// Create text flags
				StringFormat textFlags = new StringFormat();
				textFlags.FormatFlags = StringFormatFlags.NoWrap;
				textFlags.Alignment = StringAlignment.Near;
				textFlags.LineAlignment = StringAlignment.Center;

				// Measure and draw Title text
				// Give a maximum 50% of the available width to the title, the rest goes to the labels
				clipRect.X += UIElements.TAG_MARGIN;
				float titleWidth = Math.Min(g.MeasureString(Data.Title, boldFont).Width, clipRect.Width / 2);
				RectangleF titleBounds = new RectangleF(clipRect.X, clipRect.Y, titleWidth, clipRect.Height);
				StringFormat titleStringFlags = new StringFormat(textFlags);
				titleStringFlags.Trimming = StringTrimming.EllipsisCharacter;
				g.DrawString(Data.Title, boldFont, new SolidBrush(titleDisplayLabel.ForeColor), titleBounds, titleStringFlags);

				// Start drawing tags
				if (Data.Tags != null && Data.Tags.Count > 0)
				{
					// Initial draw parameters
					titleWidth += UIElements.TAG_MARGIN;
					float tagsAvailableWidth = clipRect.Width - titleWidth;
					float tagsStartX = clipRect.X + titleWidth;

					// Compute draw mesurements and draw tags
					int moreTagsCount = Data.Tags.Count;
					foreach (Tag tag in Data.Tags)
					{
						float singleTagAvailableWidth = tagsAvailableWidth;

						// First make sure the "N more" label will fit (if it won't, stop drawing)
						float moreTagsWidth = moreTagsCount > 1 ? (g.MeasureString(string.Format(Resources.N_More, moreTagsCount), titleDisplayLabel.Font).Width + UIElements.TAG_MARGIN * 2) : 0f;
						if (moreTagsWidth > tagsAvailableWidth) break;
						singleTagAvailableWidth -= moreTagsWidth;

						// If the remaining space is more than the minimum tag size, draw the tag
						if (singleTagAvailableWidth >= UIElements.MIN_TAG_SIZE)
						{
							// Fit the tag in the remaining space
							RectangleF bounds = new RectangleF(tagsStartX, clipRect.Y + UIElements.TAG_MARGIN, singleTagAvailableWidth, clipRect.Height - (UIElements.TAG_MARGIN * 2));
							RectangleF actualBounds = UIElements.DrawTag(g, bounds, tag, titleDisplayLabel.Font);
							float actualWidth = actualBounds.Width + UIElements.TAG_MARGIN;
							tagsAvailableWidth -= actualWidth;
							tagsStartX += actualWidth;
							moreTagsCount--;
						}
					}

					// Optionally draw the "N more" label
					if (moreTagsCount > 0)
					{
						// Draw the "N more" label
						string nMoreStr = string.Format(Resources.N_More, moreTagsCount);
						RectangleF nMoreStrBounds = new RectangleF(tagsStartX, clipRect.Y, clipRect.Right - tagsStartX, clipRect.Height);
						g.DrawString(nMoreStr, italicFont, new SolidBrush(titleDisplayLabel.ForeColor), nMoreStrBounds, new StringFormat(textFlags));
					}
				}
			}
		}

		#endregion

		#region NoteUpdatedEvent

		public delegate void NoteUpdatedEvent(object sender, NoteUpdateEventArgs args);

		public event NoteUpdatedEvent NoteUpdated;

		public class NoteUpdateEventArgs : EventArgs
		{
			public Note UpdatedNote { get; set; }
		}

		private void OnNoteUpdated()
		{
			if (NoteUpdated != null) NoteUpdated(this, new NoteUpdateEventArgs { UpdatedNote = Data });
		}

		#endregion

		#region Overridden Form methods

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!EditMode)
			{
				Rectangle rect = new Rectangle(e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
				e.Graphics.DrawRectangle(new Pen(ForeColor, 2), rect);
			}
		}

		#endregion

	}
}
