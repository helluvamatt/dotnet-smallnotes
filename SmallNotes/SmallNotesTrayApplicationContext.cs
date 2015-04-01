using Common.Data.Async;
using Common.TrayApplication;
using SmallNotes.Data;
using SmallNotes.Properties;
using SmallNotes.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SmallNotes
{
	public class SmallNotesTrayApplicationContext : TrayApplicationContext
	{
		private const string APP_DATA_PATH = "SmallNotes";

		/// <summary>
		/// Config database
		/// </summary>
		public Database ConfigDatabase { get; private set; }

		private AsyncCallback<Database.LoadNotesResult> LoadCallback { get; set; }
		private AsyncCallback<Database.SaveNoteResult> SaveCallback { get; set; }

		private Dictionary<string, NoteForm> _Forms;
		private Dictionary<int, NoteForm> _SavingNoteForms;
		private List<NoteForm> _NewNoteForms;

		public SmallNotesTrayApplicationContext() : base()
		{
			// Initialize variables
			// TODO Enable multiple database backends (Filesystem, cloud, etc..)
			//ConfigDatabase = new FileDatabase();
			ConfigDatabase = new TestDatabase();
			LoadCallback = new AsyncCallback<Database.LoadNotesResult>();
			SaveCallback = new AsyncCallback<Database.SaveNoteResult>();
			_Forms = new Dictionary<string, NoteForm>();
			_SavingNoteForms = new Dictionary<int, NoteForm>();
			_NewNoteForms = new List<NoteForm>();
		}

		#region TrayApplicationContext implementation

		protected override void OnInitializeContext()
		{
			// Make sure the AppDataPath folder is available
			Directory.CreateDirectory(GetAppDataPath());

			// Initialize the load/save event handlers
			LoadCallback.AsyncFinished += LoadCallback_AsyncFinished;
			SaveCallback.AsyncFinished += SaveCallback_AsyncFinished;

			// TODO Load application configuration
			Dictionary<string, string> properties = new Dictionary<string, string>();

			// Initialize the database
			ConfigDatabase.Initialize(GetAppDataPath(), properties);

			// Do the load
			ConfigDatabase.LoadNotesAsync(LoadCallback);
		}

		protected override OptionsForm BuildOptionsForm()
		{
			SmallNotesOptionsForm form = new SmallNotesOptionsForm();
			// TODO Bind events, etc...
			return form;
		}

		protected override void BuildContextMenu()
		{
			// Build the context menu: showOptionsMenuItem
			ToolStripMenuItem showOptionsMenuItem = new ToolStripMenuItem(Resources.MenuItemManage);
			showOptionsMenuItem.Click += showOptionsMenuItem_Click;

			// Build the context menu: feedsMenuItem
			ToolStripMenuItem newNoteMenuItem = new ToolStripMenuItem(Resources.MenuItemNewNote);
			newNoteMenuItem.Click += newNoteMenuItem_Click;

			// Build the context menu: exitMenuItem
			ToolStripMenuItem exitMenuItem = new ToolStripMenuItem(Resources.MenuItemExit);
			exitMenuItem.Click += exitMenuItem_Click;

			// Build contextMenu
			notifyIcon.ContextMenuStrip.Items.Add(showOptionsMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(newNoteMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
		}

		protected override string GetApplicationName()
		{
			return Resources.AppTitle;
		}

		protected override Icon GetApplicationIcon()
		{
			return new Icon(GetType(), "appicon.ico");
		}

		protected override string GetAppDataPath()
		{
			return Path.GetDirectoryName(Path.GetFullPath(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath)));
		}

		#endregion

		#region Event handlers

		private void showOptionsMenuItem_Click(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		private void newNoteMenuItem_Click(object sender, EventArgs e)
		{
			Logger.Info("Creating new note...");
			NoteForm newNoteForm = CreateNoteForm();
			_NewNoteForms.Add(newNoteForm);
			newNoteForm.Data = null;
			newNoteForm.Show();
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			ExitThread();
		}

		private void LoadCallback_AsyncFinished(Database.LoadNotesResult result)
		{
			// Redraw windows, possibly adding new ones
			foreach (KeyValuePair<string, Note> entry in result.NoteList)
			{
				if (_Forms.ContainsKey(entry.Key))
				{
					NoteForm existingNoteForm = _Forms[entry.Key];
					if (entry.Value.IsChangedFrom(existingNoteForm.Data))
					{
						existingNoteForm.Data = entry.Value;
					}
				}
				else
				{
					// New Note: Add form and display
					NoteForm newNoteForm = CreateNoteForm();
					newNoteForm.Data = entry.Value;
					newNoteForm.Show();
					_Forms.Add(entry.Key, newNoteForm);
				}
			}
		}

		private void SaveCallback_AsyncFinished(Database.SaveNoteResult result)
		{
			if (result.ResultId.HasValue)
			{
				lock (_SavingNoteForms)
				{
					if (_SavingNoteForms.ContainsKey(result.ResultId.Value))
					{
						NoteForm form = _SavingNoteForms[result.ResultId.Value];
						_SavingNoteForms.Remove(result.ResultId.Value);
						if (form.Data == null)
						{
							form.Data = result.SavedNote;
						}
						else
						{
							form.Data.ID = result.SavedNote.ID;
						}
						_Forms.Add(form.Data.ID, form);
					}
				}
				
			}
		}

		private void noteForm_NoteUpdated(object sender, NoteForm.NoteUpdateEventArgs args)
		{
			// Save the note given in the args or the sender.Data
			NoteForm target = (NoteForm)sender;

			// Track the form that is being saved
			int? saveRequestId = null;
			lock (_NewNoteForms)
			{
				if (_NewNoteForms.Remove(target))
				{
					lock (_SavingNoteForms)
					{
						if (_SavingNoteForms.Keys.Count < 1)
						{
							saveRequestId = 1;
						}
						else
						{
							IEnumerable<int> gapIds = Enumerable.Range(_SavingNoteForms.Keys.Min(), _SavingNoteForms.Keys.Count).Except(_SavingNoteForms.Keys);
							if (gapIds.Count() < 1)
							{
								saveRequestId = _SavingNoteForms.Keys.Max() + 1;
							}
							else
							{
								saveRequestId = gapIds.First();
							}
						}
						_SavingNoteForms.Add(saveRequestId.Value, target);
					}
				}
			}

			// Do the actual save
			ConfigDatabase.SaveNoteAsync(SaveCallback, target.Data, saveRequestId);
		}

		#endregion

		#region Utility methods

		private NoteForm CreateNoteForm()
		{
			NoteForm noteForm = new NoteForm(AppDataFolder);
			noteForm.NoteUpdated += noteForm_NoteUpdated;
			return noteForm;
		}

		#endregion
	}
}
