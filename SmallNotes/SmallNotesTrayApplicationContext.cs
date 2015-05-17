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
using System.Collections.Concurrent;
using Common.UI.Win32Interop;
using Common.UI.Hotkeys;

namespace SmallNotes
{
	public class SmallNotesTrayApplicationContext : TrayApplicationContext<Settings>
	{
		#region Static properties

		public static ColorList BackgroundColorList { get; private set; }

		#endregion

		#region Private members

		private DatabaseManager _DatabaseManager;
		private ConcurrentDictionary<string, NoteForm> _Forms;
		private ConcurrentDictionary<int, NoteForm> _SavingNoteForms;
		private List<NoteForm> _NewNoteForms;
		private HotkeyManager _HotkeyManager;
		private StackSet<string> _MostRecentNoteId = new StackSet<string>();

		private bool _AllNotesVisible = true;

		private const string INI_FILE_NAME = "SmallNotes.ini";
		private string IniFile
		{
			get
			{
				return Path.Combine(AppDataPath, INI_FILE_NAME);
			}
		}

		#endregion

		#region Hotkey names

		public const string HK_NEWNOTE = "NewNote";
		public const string HK_SHOWHIDEALLNOTES = "ShowHideAllNotes";
		public const string HK_EDITMOSTRECENTNOTE = "EditMostRecentNote";

		#endregion

		public SmallNotesTrayApplicationContext() : base()
		{
			// Initialize variables
			_Forms = new ConcurrentDictionary<string, NoteForm>();
			_SavingNoteForms = new ConcurrentDictionary<int, NoteForm>();
			_NewNoteForms = new List<NoteForm>();
			_DatabaseManager = new DatabaseManager(AppDataPath);
			_HotkeyManager = new HotkeyManager();
		}

		#region TrayApplicationContext implementation

		protected override void OnInitializeContext()
		{
			// Initialize the hotkey manager
			_HotkeyManager.AddHook(HK_NEWNOTE, Resources.Hotkey_NewNote_Name, Resources.Hotkey_NewNote_Desc, hotkey_NewNote);
			_HotkeyManager.AddHook(HK_SHOWHIDEALLNOTES, Resources.Hotkey_ShowHideAllNotes_Name, Resources.Hotkey_ShowHideAllNotes_Desc, hotkey_ShowHideAllNotes);
			_HotkeyManager.AddHook(HK_EDITMOSTRECENTNOTE, Resources.Hotkey_EditMostRecentNote_Name, Resources.Hotkey_EditMostRecentNote_Desc, hotkey_EditMostRecentNote);

			// Make sure the AppDataPath folder is available
			Directory.CreateDirectory(AppDataPath);

			// Initialize the load/save event handlers
			_DatabaseManager.NoteSaved += _DatabaseManager_NoteSaved;
			_DatabaseManager.NotesLoaded += _DatabaseManager_NotesLoaded;
			_DatabaseManager.NoteSaving += _DatabaseManager_NoteSaving;
			_DatabaseManager.NoteDeleted += _DatabaseManager_NoteDeleted;
			_DatabaseManager.DatabaseChanged += _DatabaseManager_DatabaseChanged;
			_DatabaseManager.TagsLoaded += _DatabaseManager_TagsLoaded;
			_DatabaseManager.FatalError += _DatabaseManager_FatalError;

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

			// Load Note exportTemplate.css
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SmallNotes.UI.exportTemplate.css"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					NoteForm.ExportStylesheetTemplate = reader.ReadToEnd();
				}
			}

			// Load application configuration
			LoadSettingsAsync(Settings => {

				// Initialize the database
				_DatabaseManager.SetDatabase(Settings.DatabaseType, Settings.DatabaseProperties);

				// Initialize hotkeys
				foreach (KeyValuePair<string, GlobalHotkeyHook> entry in _HotkeyManager.GetHooks())
				{
					if (Settings.Hotkeys != null && Settings.Hotkeys.ContainsKey(entry.Key))
					{
						entry.Value.Key = Settings.Hotkeys[entry.Key];
					}
					if (Settings.HotkeyEnabled != null && Settings.HotkeyEnabled.ContainsKey(entry.Key))
					{
						entry.Value.Enabled = Settings.HotkeyEnabled[entry.Key];
					}
				}
				_HotkeyManager.Enabled = Settings.HotkeysEnabled;

				// Populate settings
				if (optionsForm != null)
				{
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
			SmallNotesOptionsForm form = new SmallNotesOptionsForm(SettingsManager, _DatabaseManager, _HotkeyManager);
			form.OptionChanged += SmallNotesOptionsForm_OptionChanged;
			form.NewNoteAction += SmallNotesOptionsForm_NewNoteAction;
			form.ShowNoteAction += SmallNotesOptionsForm_ShowNoteAction;
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

		protected override void Dispose(bool disposing)
		{
			_HotkeyManager.Dispose();
			base.Dispose(disposing);
		}

		#endregion

		#region Event handlers

		private void showOptionsMenuItem_Click(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		private void newNoteMenuItem_Click(object sender, EventArgs e)
		{
			CreateNewNote();
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			ExitThread();
		}

		private void _DatabaseManager_NotesLoaded(DatabaseManager.LoadNotesResult result)
		{
			if (result.Success)
			{
				// First, obliterate all existing forms
				AllNoteFormsIterator(form => { form.CloseNoSave(); form.Dispose(); });
				lock (_NewNoteForms)
				{
					_NewNoteForms.Clear();
				}
				_SavingNoteForms.Clear();
				_Forms.Clear();

				// Draw newly valid ones
				foreach (KeyValuePair<string, Note> entry in result.NoteList)
				{
					if (entry.Value.Visible)
					{
						// New Note: Add form and display
						NoteForm newNoteForm = CreateNoteForm();
						newNoteForm.Data = entry.Value;
						newNoteForm.Show();
						_Forms.TryAdd(entry.Key, newNoteForm);
						_MostRecentNoteId.Push(entry.Key);
					}
					else
					{
						Logger.DebugFormat("Note '{0}' has Visible = {1} was not shown.", entry.Value.ID, entry.Value.Visible);
					}
				}
			}
			else
			{
				MessageBox.Show(string.Format(Resources.LoadNotesFailed, result.Exception.Message), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _DatabaseManager_NoteSaved(DatabaseManager.SaveNoteResult result)
		{
			if (result.Success)
			{
				if (result.ResultId.HasValue)
				{
					NoteForm form;
					if (_SavingNoteForms.TryRemove(result.ResultId.Value, out form))
					{
						// Handle the case where the form was closed while the note was saving
						if (form.IsDisposed)
						{
							Note oldFormData = form.Data;
							form = CreateNoteForm();
							form.Data = oldFormData;
						}

						// Repopulate the Note Data
						if (form.Data == null)
						{
							form.Data = result.SavedNote;
						}
						else
						{
							form.Data.ID = result.SavedNote.ID;
						}

						// Make sure the note is in the _Forms hash
						_Forms.TryAdd(form.Data.ID, form);
					}
				}
				else if (!_Forms.ContainsKey(result.SavedNote.ID) && result.SavedNote.Visible)
				{
					NoteForm form = CreateNoteForm();
					form.Data = result.SavedNote;
					form.Show();

					// Make sure the note is in the _Forms hash
					_Forms.TryAdd(form.Data.ID, form);
				}

				// Track most recent save
				if (result.SavedNote.Visible)
				{
					_MostRecentNoteId.Push(result.SavedNote.ID);
				}
			}
		}

		private void _DatabaseManager_NoteSaving(object sender, DatabaseManager.SaveNoteRequest e)
		{
			// Do nothing
			Logger.DebugFormat("Saving note ('{0}')...", e.SaveNote.Title);
		}

		private void _DatabaseManager_NoteDeleted(DatabaseManager.ObjectDeletedResult result)
		{
			NoteForm form;
			if (result.Success && _Forms.TryRemove(result.DeletedId, out form))
			{
				form.CloseNoSave();
				form.Dispose();
			}
		}

		private void _DatabaseManager_TagsLoaded(DatabaseManager.LoadTagsResult result)
		{
			NoteForm.TagList = result.Success ? result.TagList : new Dictionary<string, Tag>();
		}

		private void _DatabaseManager_DatabaseChanged(object sender, DatabaseManager.DatabaseChangedEventArgs e)
		{
			// Save settings
			SettingsManager.SettingsObject.DatabaseType = e.Database.GetType().FullName;
			SettingsManager.SettingsObject.DatabaseProperties = ModelSerializer.SerializeModelToHash(e.Database);
			SaveSettingsAsync(null, IniFile);
		}

		private void _DatabaseManager_FatalError(object sender, DatabaseManager.DatabaseException e)
		{
			// Fatal errors in the database
			MessageBox.Show(string.Format(Resources.FatalDatabaseError, e.Message), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			ExitThread();
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
			else if (args.Name == "IdleTimeout")
			{
				long idleTimeout = SettingsManager.SettingsObject.IdleTimeout;
				AllNoteFormsIterator(f => f.IdleTimeout = idleTimeout);
			}
		}

		private void SmallNotesOptionsForm_NewNoteAction()
		{
			CreateNewNote();
		}

		private void SmallNotesOptionsForm_ShowNoteAction(Note obj)
		{
			if (!obj.Visible)
			{
				obj.Visible = true;
				_DatabaseManager.SaveNoteAsync(obj);

				if (!_Forms.ContainsKey(obj.ID))
				{
					NoteForm form = CreateNoteForm();
					form.Data = obj;
					form.Show();
					lock (_Forms)
					{
						_Forms[obj.ID] = form;
					}
				}
				else
				{
					_Forms[obj.ID].Activate();
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
					_SavingNoteForms.TryAdd(saveRequestId.Value, target);
				}
			}

			// Do the actual save
			_DatabaseManager.SaveNoteAsync(target.Data, saveRequestId);
		}

		private void noteForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			NoteForm noteForm = (NoteForm)sender;
			if (noteForm.Data != null && !string.IsNullOrEmpty(noteForm.Data.ID))
			{
				_Forms.TryRemove(noteForm.Data.ID, out noteForm);
				_MostRecentNoteId.Remove(noteForm.Data.ID);
			}
			else
			{
				lock (_NewNoteForms)
				{
					_NewNoteForms.Remove(noteForm);
				}
			}
		}

		#region Hotkey callbacks

		private void hotkey_NewNote()
		{
			CreateNewNote();
		}

		private void hotkey_ShowHideAllNotes()
		{
			_AllNotesVisible = !_AllNotesVisible;
			AllNoteFormsIterator(f => f.Visible = _AllNotesVisible);
		}

		private void hotkey_EditMostRecentNote()
		{
			while (_MostRecentNoteId.Count > 0)
			{
				string mostRecentId = _MostRecentNoteId.Peek();
				if (_Forms.ContainsKey(mostRecentId))
				{
					_Forms[mostRecentId].EditMode = true;
					_Forms[mostRecentId].Activate();
					break;
				}
				else
				{
					_MostRecentNoteId.Pop();
				}
			}
		}

		#endregion

		#endregion

		#region Utility methods

		private NoteForm CreateNoteForm()
		{
			NoteForm noteForm = new NoteForm(AppDataPath, BackgroundColorList);
			noteForm.NoteUpdated += noteForm_NoteUpdated;
			noteForm.FormClosed += noteForm_FormClosed;
			noteForm.NoteFactory = () => _DatabaseManager.CreateNewNote();
			noteForm.FastResizeMove = SettingsManager.SettingsObject.FastRendering;
			noteForm.DefaultBackgroundColor = SettingsManager.SettingsObject.DefaultBackgroundColor;
			noteForm.DefaultForegroundColor = SettingsManager.SettingsObject.DefaultForegroundColor;
			noteForm.IdleTimeout = SettingsManager.SettingsObject.IdleTimeout;
			return noteForm;
		}

		private void CreateNewNote()
		{
			Logger.Info("Creating new note...");
			NoteForm newNoteForm = CreateNoteForm();
			_NewNoteForms.Add(newNoteForm);
			newNoteForm.Data = null;
			newNoteForm.Show();
			newNoteForm.Activate();
		}

		private void AllNoteFormsIterator(Action<NoteForm> callback)
		{
			lock (_Forms)
			{
				foreach (KeyValuePair<string, NoteForm> entry in _Forms)
				{
					callback(entry.Value);
				}
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
