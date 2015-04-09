using Common.Data;
using Common.Data.Async;
using log4net;
using SmallNotes.Data.Entities;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	internal sealed class DatabaseManager
	{
		#region Private members

		private const string PLUGINS_FOLDER = "plugins";

		private static ILog Logger = LogManager.GetLogger(typeof(DatabaseManager));
		private static string _AppDataPath;

		private IDatabaseDescriptor _Descriptor;
		private IDatabase _Database;

		#endregion

		public DatabaseManager(string appDataPath)
		{
			Logger = LogManager.GetLogger(GetType());
			_AppDataPath = appDataPath;
		}

		public void SetDatabase(string type, Dictionary<string, string> props)
		{
			// Clean up existing database if exists
			if (_Database != null)
			{
				_Database.Dispose();
			}

			// Initialize new one
			if (props == null) props = new Dictionary<string, string>();
			if (type == null) type = typeof(FileDatabaseDescriptor).FullName;
			_Descriptor = (IDatabaseDescriptor)ModelSerializer.DeserializeModelFromHash(props, GetDatabaseTypes()[type].GetType());
			_Database = _Descriptor.InitializeDatabase(_AppDataPath);
			LoadNotesAsync();
		}

		public IDatabaseDescriptor DatabaseDescriptor
		{
			get
			{
				return _Descriptor;
			}
		}

		public void SaveNoteAsync(Note note, int? saveRequestId = null)
		{
			if (NoteSaving != null) NoteSaving(this, note);
			new AsyncRunner<SaveNoteResult, Note>().AsyncRun(SaveNote, NoteSaved, note);
		}

		public void LoadNotesAsync(string fromPath = null)
		{
			if (NotesLoading != null) NotesLoading(this, new NotesLoadingRequest { FromPath = fromPath });
			// TODO Send request to synchronouse Load method
			new AsyncRunner<LoadNotesResult, object>().AsyncRun(LoadNotes, NotesLoaded);
		}

		public Note CreateNewNote()
		{
			return _Database.CreateNewNote();
		}

		#region Events

		public event Action<SaveNoteResult> NoteSaved;

		public event Action<LoadNotesResult> NotesLoaded;

		public event EventHandler<NotesLoadingRequest> NotesLoading;

		public event EventHandler<Note> NoteSaving;

		#endregion

		#region Static type management

		private static Dictionary<string, IDatabaseDescriptor> _DatabaseTypes = new Dictionary<string, IDatabaseDescriptor>();

		public static Dictionary<string, IDatabaseDescriptor> GetDatabaseTypes(bool skipCache = false)
		{
			// Possibly skip the cache and load the list
			if (skipCache || _DatabaseTypes.Count < 1)
			{
				// Search plugins folder for plugin assemblies
				HashSet<Assembly> assemblies = new HashSet<Assembly>();
				assemblies.Add(typeof(DatabaseManager).Assembly);
				string pluginsPath = Path.Combine(_AppDataPath, PLUGINS_FOLDER);
				if (Directory.Exists(pluginsPath))
				{
					foreach (string item in Directory.EnumerateFiles(pluginsPath))
					{
						string itemPath = Path.Combine(pluginsPath, item);
						if (!Directory.Exists(itemPath) && string.Equals(new FileInfo(itemPath).Extension, "dll", StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								assemblies.Add(Assembly.LoadFile(itemPath));
							}
							catch (Exception ex)
							{
								Logger.Error("Failed to load plugin assembly", ex);
							}
						}
					}
				}
				else
				{
					Logger.WarnFormat("Plugins path ('{0}') does not exist.", pluginsPath);
				}

				// Search for types in each assembly
				_DatabaseTypes.Clear();
				Type searchType = typeof(IDatabaseDescriptor);
				foreach (Assembly assembly in assemblies)
				{
					foreach (Type type in assembly.GetExportedTypes().Where(t => searchType.IsAssignableFrom(t)))
					{
						_DatabaseTypes.Add(type.FullName, (IDatabaseDescriptor)Activator.CreateInstance(type));
					}
				}
			}

			// return the list
			return _DatabaseTypes;
		}

		#endregion

		#region Async runner methods

		private SaveNoteResult SaveNote(Note note)
		{
			try
			{
				return new SaveNoteResult { Success = true, Exception = null, SavedNote = _Database.SaveNote(note) };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while loading notes.", ex);
				return new SaveNoteResult { Success = false, Exception = ex, SavedNote = null };
			}
		}

		private LoadNotesResult LoadNotes()
		{
			try
			{
				return new LoadNotesResult { Success = true, Exception = null, NoteList = _Database.LoadNotes() };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while loading notes.", ex);
				return new LoadNotesResult { Success = false, Exception = ex, NoteList = null };
			}
		}

		#endregion

		#region Utility types

		public class LoadNotesResult : BasicResult
		{
			public Dictionary<string, Note> NoteList { get; set; }
		}

		private class SaveNoteRequest
		{
			public Note _Note { get; set; }
		}

		public class SaveNoteResult : TrackedResult
		{
			public Note SavedNote { get; set; }
		}

		public class NotesLoadingRequest : EventArgs
		{
			public string FromPath { get; set; }
		}

		#endregion
	}
}
