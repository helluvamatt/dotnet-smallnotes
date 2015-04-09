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
using SmallNotes.Data.Entities;
using System.Xml;
using Common.Data;
using SmallNotes.UI.Utils;

namespace SmallNotes
{
	public class SmallNotesTrayApplicationContext : TrayApplicationContext<Settings>
	{
		#region Static properties

		public static ColorList BackgroundColorList { get; private set; }

		#endregion

		#region Private members

		private DatabaseManager _DatabaseManager;
		private Dictionary<string, NoteForm> _Forms;
		private Dictionary<int, NoteForm> _SavingNoteForms;
		private List<NoteForm> _NewNoteForms;

		private const string INI_FILE_NAME = "SmallNotes.ini";
		private string IniFile
		{
			get
			{
				return Path.Combine(AppDataPath, INI_FILE_NAME);
			}
		}

		#endregion

		public SmallNotesTrayApplicationContext() : base()
		{
			// Initialize variables
			_Forms = new Dictionary<string, NoteForm>();
			_SavingNoteForms = new Dictionary<int, NoteForm>();
			_NewNoteForms = new List<NoteForm>();
			_DatabaseManager = new DatabaseManager(AppDataPath);
		}

		#region TrayApplicationContext implementation

		protected override void OnInitializeContext()
		{
			// Make sure the AppDataPath folder is available
			Directory.CreateDirectory(AppDataPath);

			// Initialize the load/save event handlers
			_DatabaseManager.NoteSaved += _DatabaseManager_NoteSaved;
			_DatabaseManager.NotesLoaded += _DatabaseManager_NotesLoaded;
			_DatabaseManager.NoteSaving += _DatabaseManager_NoteSaving;
			_DatabaseManager.NotesLoading += _DatabaseManager_NotesLoading;

			// Load Note template.css
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmallNotes.UI.template.css"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					NoteForm.StylesheetTemplate = reader.ReadToEnd();
				}
			}

			// Load Note template.html
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmallNotes.UI.template.html"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					NoteForm.DocumentTemplate = reader.ReadToEnd();
				}
			}

			// Load application configuration
			LoadSettingsAsync(Settings => {

				// Initialize the database
				_DatabaseManager.SetDatabase(Settings.DatabaseType, Settings.DatabaseProperties);

				// Populate settings
				if (optionsForm != null)
				{
					((SmallNotesOptionsForm)optionsForm).SelectedDatabase = _DatabaseManager.DatabaseDescriptor;
					((SmallNotesOptionsForm)optionsForm).PopulateSettings();
				}
			
			}, IniFile);

			// Load background color list from colors.xml
			if (BackgroundColorList == null)
			{
				string path = Path.Combine(AppDataPath, "colors.xml");
				new AsyncRunner<ColorList, string>().AsyncRun(f => {
					ColorList list = new ColorList();
					try
					{
						using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
						{
							list.LoadFromXml(reader);
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
					return list;
					
				}, list => { BackgroundColorList = list; }, path);
			}
		}

		protected override OptionsForm BuildOptionsForm()
		{
			SmallNotesOptionsForm form = new SmallNotesOptionsForm(SettingsManager);
			form.OptionChanged += SmallNotesOptionsForm_OptionChanged;
			form.DatabaseSettingsUpdated += SmallNotesOptionsForm_DatabaseSettingsUpdated;
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

		protected override string ApplicationName
		{
			get
			{
				return Resources.AppTitle;
			}
		}

		protected override Icon ApplicationIcon
		{
			get
			{
				return new Icon(GetType(), "appicon.ico");
			}
		}

		protected override string AppDataPath
		{
			get
			{
				return Path.GetDirectoryName(Path.GetFullPath(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath)));
			}
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

		private void _DatabaseManager_NotesLoaded(DatabaseManager.LoadNotesResult result)
		{
			// Send updates to the options form (if it exists)
			if (optionsForm != null)
			{
				((SmallNotesOptionsForm)optionsForm).NoteList = result.NoteList;
				((SmallNotesOptionsForm)optionsForm).IsLoadingNotes = false;
			}

			// Redraw windows, possibly adding new ones
			foreach (KeyValuePair<string, Note> entry in result.NoteList)
			{
				if (_Forms.ContainsKey(entry.Key))
				{
					NoteForm existingNoteForm = _Forms[entry.Key];
					existingNoteForm.Visible = entry.Value.Visible;
					if (entry.Value.IsChangedFrom(existingNoteForm.Data))
					{
						existingNoteForm.Data = entry.Value;
					}
				}
				else if (entry.Value.Visible)
				{
					// New Note: Add form and display
					NoteForm newNoteForm = CreateNoteForm();
					newNoteForm.Data = entry.Value;
					newNoteForm.Show();
					_Forms.Add(entry.Key, newNoteForm);
				}
				else
				{
					Logger.DebugFormat("Note '{0}' has Visible = {1} was not shown.", entry.Value.ID, entry.Value.Visible);
				}
			}
		}

		private void _DatabaseManager_NoteSaved(DatabaseManager.SaveNoteResult result)
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

			// Send update to OptionsForm (if it exists)
			if (optionsForm != null)
			{
				((SmallNotesOptionsForm)optionsForm).UpdateNote(result.SavedNote);
			}
		}

		private void _DatabaseManager_NotesLoading(object sender, DatabaseManager.NotesLoadingRequest e)
		{
			if (optionsForm != null) ((SmallNotesOptionsForm)optionsForm).IsLoadingNotes = true;
		}

		private void _DatabaseManager_NoteSaving(object sender, Note e)
		{
			// Do nothing
			Logger.DebugFormat("Saving note ('{0}')...", e.Title);
		}

		private void SmallNotesOptionsForm_OptionChanged(object sender, OptionChangedEventArgs args)
		{
			SaveSettingsAsync(null, IniFile);

			// Reconfigure FastRendering setting
			if (args.Name == "FastRendering")
			{
				bool fastRendering = SettingsManager.SettingsObject.FastRendering;
				AllNoteFormsIterator(f => f.FastResizeMove = fastRendering);
			}
			else if (args.Name == "CustomCss")
			{
				string customCss = SettingsManager.SettingsObject.CustomCss;
				AllNoteFormsIterator(f => f.CustomStylesheet = customCss);
			}
		}

		private void SmallNotesOptionsForm_DatabaseSettingsUpdated(IDatabaseDescriptor obj)
		{
			// Save settings
			SettingsManager.SettingsObject.DatabaseType = obj.GetType().FullName;
			SettingsManager.SettingsObject.DatabaseProperties = ModelSerializer.SerializeModelToHash(obj);
			SaveSettingsAsync(null, IniFile);

			// Reconfigure when changing backends
			_DatabaseManager.SetDatabase(SettingsManager.SettingsObject.DatabaseType, SettingsManager.SettingsObject.DatabaseProperties);
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
			_DatabaseManager.SaveNoteAsync(target.Data, saveRequestId);
		}

		#endregion

		#region Utility methods

		private NoteForm CreateNoteForm()
		{
			NoteForm noteForm = new NoteForm(AppDataPath, BackgroundColorList);
			noteForm.NoteUpdated += noteForm_NoteUpdated;
			noteForm.NoteFactory = () => _DatabaseManager.CreateNewNote();
			noteForm.FastResizeMove = SettingsManager.SettingsObject.FastRendering;
			return noteForm;
		}

		private void AllNoteFormsIterator(Action<NoteForm> callback)
		{
			foreach(KeyValuePair<string, NoteForm> entry in _Forms)
			{
				callback(entry.Value);
			}
			lock (_NewNoteForms)
			{
				foreach(NoteForm entry in _NewNoteForms)
				{
					callback(entry);
				}
			}
			lock (_SavingNoteForms)
			{
				foreach(KeyValuePair<int, NoteForm> entry in _SavingNoteForms)
				{
					callback(entry.Value);
				}
			}
		}

		#endregion
	}
}
