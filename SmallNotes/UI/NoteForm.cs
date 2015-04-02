using ColorCode;
using CommonMark;
using CommonMark.Syntax;
using log4net;
using SmallNotes.Data;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace SmallNotes.UI
{
	public partial class NoteForm : Form
	{
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
				UpdateUI();
			}
			get
			{
				return _EditMode;
			}
		}
		#endregion

		#region Properties

		public string CustomStylesheet { get; set; }

		public string AppDataPath { get; set; }

		public bool FastResizeMove { get; set; }

		private ILog Logger { get; set; }

		#endregion

		#region Private members

		private Point _formLocation;
		private Size _formSize;
		private Point _mouseDown;
		private bool _moveCapture;
		private bool _resizeCapture;
		private const int GRIP_SIZE = 16;
		private bool _lastUpFromDrag;

		private ColorList _colorList;
		private bool _automaticForeColor = false;
		private ToolStripMenuItem customBackgroundColorMenuItem;

		private string _StylesheetTemplate;
		private string _DocumentTemplate;

		//private static int MouseAreaW = SystemInformation.DoubleClickSize.Width;
		//private static int MouseAreaH = SystemInformation.DoubleClickSize.Height;
		//private static TimeSpan DoubleClickTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

		//private static Regex _codeBlock = new Regex(@"<code>((?:.*\n+)+)</code>", RegexOptions.Multiline | RegexOptions.Compiled);

		#endregion

		public NoteForm(string appDataPath)
		{
			Logger = LogManager.GetLogger(GetType());
			AppDataPath = appDataPath;
			_colorList = new ColorList();
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, true);

			// Load background color list from colors.xml
			try
			{
				string path = Path.Combine(AppDataPath, "colors.xml");
				using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
				{
					_colorList.LoadFromXml(reader);
				}
			}
			catch (XmlException ex)
			{
				Logger.Error("XML parsing error", ex);
			}
			catch (IOException ex)
			{
				Logger.Error("IO error", ex);
			}

			// Populate color menu
			if (_colorList.Items.Count > 0)
			{
				foreach (ColorList.ColorItem item in _colorList.Items)
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
			customBackgroundColorMenuItem.Click += customBackgroundColorMenuItem_Click;
			backgroundColorDropDownButton.DropDownItems.Add(customBackgroundColorMenuItem);

			// Populate foreground color menu icons
			blackForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("Black") { Color = Color.Black }.Icon;
			whiteForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("White") { Color = Color.White }.Icon;

			// Load template.css
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmallNotes.UI.template.css"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					_StylesheetTemplate = reader.ReadToEnd();
				}
			}
			CustomStylesheet = "";

			// Load template.html
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmallNotes.UI.template.html"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					_DocumentTemplate = reader.ReadToEnd();
				}
			}
		}

		#region Utility methods

		private void UpdateUI()
		{
			// Window title
			Text = _EditMode ? (Data != null ? Data.Title : Resources.NewNoteTitle) : "";

			// Editor
			titleTextBox.Text = Data != null ? Data.Title : Resources.NewNoteTitle;
			markdownTextBox.Text = Data != null ? Data.Text : "";
			_automaticForeColor = Data == null || !Data.ForegroundColor.HasValue;
			BackColor = Data != null ? Data.BackgroundColor : ColorTranslator.FromHtml(Resources.DefaultBackgroundColor);
			ForeColor = Data != null && Data.ForegroundColor.HasValue ? Data.ForegroundColor.Value : GetAutomaticForegroundColor(BackColor);
			RedrawAutomaticMenuItemIcon();

			// Display
			if (Data != null)
			{
				// Compute link color
				// ForegroundColor == Automatic: LinkColor = AliceBlue for dark backgrounds, Blue for light backgrounds
				// ForegroundColor != Automatic: LinkColor = ForegroundColor
				Color linkColor = Data.ForegroundColor.HasValue ? Data.ForegroundColor.Value : IsBackgroundDark(BackColor) ? Color.AliceBlue : Color.Blue;

				// Parse Markdown to HTML
				string bodyHtml = CommonMarkConverter.Convert(Data.Text, GetCommonMarkSettings());

				// Process _StylesheetTemplate
				SimpleTemplate styleTemplate = new SimpleTemplate() { Template = _StylesheetTemplate };
				styleTemplate["ForeColor"] = ColorTranslator.ToHtml(ForeColor);
				styleTemplate["BackColor"] = ColorTranslator.ToHtml(BackColor);
				styleTemplate["LinkColor"] = ColorTranslator.ToHtml(linkColor);
				styleTemplate["CustomStylesheet"] = CustomStylesheet;

				// Process _DocumentTemplate
				SimpleTemplate documentTemplate = new SimpleTemplate() { Template = _DocumentTemplate };
				documentTemplate["Title"] = Data.Title;
				documentTemplate["StyleSheet"] = styleTemplate.Render();
				documentTemplate["BodyHtml"] = bodyHtml;
				string documentHtml = documentTemplate.Render();

				// Display the note
				displayBrowser.BackColor = BackColor;
				displayBrowser.ForeColor = ForeColor;
				displayBrowser.Text = documentHtml;
				titleDisplayLabel.Text = Data.Title;
			}
		}

		#region GetCommonMarkSettings for custom HTML rendering

		private static CommonMarkSettings _CommonMarkSettings;

		public CommonMarkSettings GetCommonMarkSettings()
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
					ILanguage lang = Languages.FindById(langName);
					if (lang != null)
					{
						if (title == null) title = lang.Name;
						_colorizer.Colorize(block.StringContent.ToString(), lang, new NoteCodeFormatter(title), StyleSheets.Default, _writer);
						ignoreChildNodes = true;
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

		private void CheckCursor()
		{
			Point cursorPos = displayPanel.PointToClient(Cursor.Position);
			Rectangle gripperArea = displayPanel.GetResizeRect();
			if (displayPanel.Visible && gripperArea.Contains(cursorPos))
			{
				Cursor = Cursors.SizeNWSE;
			}
			else
			{
				Cursor = Cursors.Default;
			}
		}

		private static bool IsBackgroundDark(Color backColor)
		{
			int val = (int)Math.Round((new int[] { backColor.R, backColor.G, backColor.B }).Average());
			return val < 128;
		}

		private static Color GetAutomaticForegroundColor(Color backColor)
		{
			return IsBackgroundDark(backColor) ? Color.White : Color.Black;
		}

		private void RedrawAutomaticMenuItemIcon()
		{
			automaticForegroundColorToolStripMenuItem.Image = new ColorList.ColorItem("Auto") { Color = GetAutomaticForegroundColor(BackColor) }.Icon;
		}

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
				EditMode = false;
			}
			else
			{
				Note newNote = new Note();
				newNote.Text = markdownTextBox.Text;
				newNote.Title = titleTextBox.Text;
				newNote.BackgroundColor = BackColor;
				newNote.ForegroundColor = _automaticForeColor ? (Color?)null : ForeColor;

				Data = newNote;
			}

			// Save the note
			if (NoteUpdated != null) NoteUpdated(this, new NoteUpdateEventArgs { UpdatedNote = Data });
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

		private void displayBrowser_ImageLoad(object sender, HtmlImageLoadEventArgs e)
		{
			// TODO Use this to make sure we support SVG images
			
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
				if (_lastUpFromDrag || Math.Abs(newLocation.X - _mouseDown.X) > SystemInformation.DoubleClickSize.Width || Math.Abs(newLocation.Y - _mouseDown.Y) > SystemInformation.DoubleClickSize.Height)
				{
					_lastUpFromDrag = true;
				}
			}
		}

		private void displayBrowser_MouseUp(object sender, MouseEventArgs e)
		{
			displayBrowser.EnableRedraw();
			_moveCapture = false;
		}

		private void displayPanel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!EditMode)
			{
				EditMode = true;
			}
			_moveCapture = false;
			_resizeCapture = false;
		}

		private void displayPanel_MouseDown(object sender, MouseEventArgs e)
		{
			_mouseDown = e.Location;
			_formLocation = ((Form)TopLevelControl).Location;
			_formSize = ((Form)TopLevelControl).Size;
			Rectangle resizeGripRegion = new Rectangle(displayPanel.Width - GRIP_SIZE, displayPanel.Height - GRIP_SIZE, GRIP_SIZE, GRIP_SIZE);
			if (resizeGripRegion.Contains(_mouseDown))
			{
				_resizeCapture = true;
			}
			else
			{
				_moveCapture = true;
			}
		}

		private void displayPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (_moveCapture)
			{
				if (FastResizeMove) displayBrowser.DisableRedraw();
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
				((Form)TopLevelControl).Location = newLocation;
				_formLocation = newLocation;
			}
			if (_resizeCapture)
			{
				if (FastResizeMove) displayBrowser.DisableRedraw();
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Size newSize = new Size(_formSize.Width + dx, _formSize.Height + dy);
				((Form)TopLevelControl).Size = newSize;
			}

			CheckCursor();
		}

		private void displayPanel_MouseUp(object sender, MouseEventArgs e)
		{
			displayBrowser.EnableRedraw();
			_moveCapture = false;
			_resizeCapture = false;
		}

		private void displayPanel_MouseEnter(object sender, EventArgs e)
		{
			CheckCursor();
		}

		private void displayPanel_MouseLeave(object sender, EventArgs e)
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
			if (Data != null && NoteUpdated != null)
			{
				Data.Visible = false;
				NoteUpdated(this, new NoteUpdateEventArgs { UpdatedNote = Data });
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

		#endregion
	}
}
