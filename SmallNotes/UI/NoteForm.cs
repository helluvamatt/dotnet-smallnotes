using Kiwi.Markdown;
using log4net;
using MarkdownSharp;
using SmallNotes.Data;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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

		public string NoteHtmlTemplate { get; set; }

		public string AppDataPath { get; set; }

		private ILog Logger { get; set; }

		#endregion

		#region Private members

		private Point _formLocation;
		private Size _formSize;
		private Point _mouseDown;
		private bool _moveCapture;
		private bool _resizeCapture;

		private const int GRIP_SIZE = 16;

		private ColorList _colorList;
		
		//private static int MouseAreaW = SystemInformation.DoubleClickSize.Width;
		//private static int MouseAreaH = SystemInformation.DoubleClickSize.Height;
		//private static TimeSpan DoubleClickTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

		#endregion

		public NoteForm(string appDataPath)
		{
			Logger = LogManager.GetLogger(GetType());
			AppDataPath = appDataPath;
			_colorList = new ColorList();
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, true);

			// Load color list from colors.xml
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

				}
				backgroundColorDropDownButton.DropDownItems.Add(new ToolStripSeparator());
			}
			ToolStripMenuItem customBackgroundColorMenuItem = new ToolStripMenuItem();
			customBackgroundColorMenuItem.Text = Resources.CustomMenuItem;
			customBackgroundColorMenuItem.Click += customBackgroundColorMenuItem_Click;

		}

		#region Utility methods

		private void UpdateUI()
		{
			// Window title
			Text = _EditMode ? (Data != null ? Data.Title : Resources.NewNoteTitle) : "";

			// Editor
			titleTextBox.Text = Data != null ? Data.Title : Resources.NewNoteTitle;
			markdownTextBox.Text = Data != null ? Data.Text : "";
			BackColor = Data != null ? Data.BackgroundColor : ColorTranslator.FromHtml(Resources.DefaultBackgroundColor);
			displayBrowser.BackColor = Data != null ? Data.BackgroundColor : ColorTranslator.FromHtml(Resources.DefaultBackgroundColor);
			displayBrowser.ForeColor = Data != null ? Data.ForegroundColor : ColorTranslator.FromHtml(Resources.DefaultForegroundColor);

			// Display
			if (Data != null)
			{
				// Parse Markdown to HTML
				// TODO [BUG] Using an invalid language will cause the parsing (and, right now, the application) to crash!
				MarkdownService markdown = new MarkdownService(null);
				string bodyHtml = markdown.ToHtml(Data.Text);

				// Render template
				SimpleHtmlTemplate template = new SimpleHtmlTemplate();
				template.HtmlTemplate = NoteHtmlTemplate;
				template["title"] = Data.Title;
				template["body"] = bodyHtml;
				string html = template.Render();

				// Display the note
				displayBrowser.Text = html;
				titleDisplayLabel.Text = Data.Title;
			}
		}

		private void CheckRadioMenu(ToolStripDropDownItem menu, ToolStripItem selectedItem)
		{
			foreach (ToolStripMenuItem item in menu.DropDownItems)
			{
				if (item != null) item.Checked = item == selectedItem;
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
				EditMode = false;
			}
			else
			{
				Note newNote = new Note();
				newNote.Text = markdownTextBox.Text;
				newNote.Title = titleTextBox.Text;
				newNote.BackgroundColor = ColorTranslator.FromHtml(Resources.DefaultBackgroundColor);
				newNote.ForegroundColor = ColorTranslator.FromHtml(Resources.DefaultForegroundColor);

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
			Logger.DebugFormat("Link clicked: '{0}'", e.Link);
			// Open links in the user's default browser (Chrome, etc.)
			Process.Start(e.Link);
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
		}

		private void displayBrowser_MouseMove(object sender, MouseEventArgs e)
		{
			if (_moveCapture)
			{
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
				((Form)TopLevelControl).Location = newLocation;
				_formLocation = newLocation;
			}
		}

		private void displayBrowser_MouseUp(object sender, MouseEventArgs e)
		{
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
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
				((Form)TopLevelControl).Location = newLocation;
				_formLocation = newLocation;
			}
			if (_resizeCapture)
			{
				int dx = e.Location.X - _mouseDown.X;
				int dy = e.Location.Y - _mouseDown.Y;
				Size newSize = new Size(_formSize.Width + dx, _formSize.Height + dy);
				((Form)TopLevelControl).Size = newSize;
			}

			CheckCursor();
		}

		private void displayPanel_MouseUp(object sender, MouseEventArgs e)
		{
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
			CheckRadioMenu(backgroundColorDropDownButton, (ToolStripItem)sender);
		}

		private void customBackgroundColorMenuItem_Click(object sender, EventArgs e)
		{
			CheckRadioMenu(backgroundColorDropDownButton, (ToolStripItem)sender);
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
