using Kiwi.Markdown;
using MarkdownSharp;
using SmallNotes.Data;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
				displayBrowser.Visible = !_EditMode;
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

		#endregion

		public NoteForm()
		{
			InitializeComponent();

			// Wire DoubleClickHandler
			EventTrigger trigger = new EventTrigger();
			trigger.Handler += NoteForm_DoubleClick;
			displayBrowser.ObjectForScripting = trigger;
		}

		private void UpdateUI()
		{
			// Window title
			Text = _EditMode ? (Data != null ? Data.Title : Resources.NewNoteTitle) : "";

			// Editor
			titleTextBox.Text = Data != null ? Data.Title : Resources.NewNoteTitle;
			markdownTextBox.Text = Data != null ? Data.Text : "";
			// TODO Background color, transparency controls, etc...

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
				displayBrowser.Navigate("about:blank");
				if (displayBrowser.Document != null)
				{
					displayBrowser.Document.Write(string.Empty);
				}
				displayBrowser.DocumentText = html;
			}
		}

		#region Event handlers

		private void saveButton_Click(object sender, EventArgs e)
		{
			// Save the note
			if (Data == null) Data = new Note();
			Data.Text = markdownTextBox.Text;
			Data.Title = titleTextBox.Text;
			if (NoteUpdated != null) NoteUpdated(this, new NoteUpdateEventArgs { UpdatedNote = Data });

			// Update the UI
			EditMode = false;
			UpdateUI();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (Data != null)
			{
				EditMode = false;
				UpdateUI();
			}
			else
			{
				Close();
			}
		}

		private void displayBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (e.Url.ToString() != "about:blank")
			{
				// TODO Open links in the user's default browser (Chrome, etc.)
				e.Cancel = true;
			}
		}

		private void NoteForm_DoubleClick(object sender, EventArgs e)
		{
			if (!EditMode)
			{
				EditMode = true;
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
