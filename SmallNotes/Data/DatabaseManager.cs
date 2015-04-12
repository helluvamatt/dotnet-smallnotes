using Common.Data;
using Common.Data.Async;
using log4net;
using SmallNotes.Data.Entities;
using SmallNotes.Data.FileDatabaseImpl;
using SmallNotes.Properties;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

		public void SetDatabase(IDatabaseDescriptor descriptor)
		{
			// Clean up existing database if exists
			if (_Database != null)
			{
				_Database.Dispose();
			}

			// Initialize new one
			_Descriptor = descriptor;
			_Database = _Descriptor.InitializeDatabase(_AppDataPath);
			new AsyncRunner<BasicResult, object>().AsyncRun(Initialize, result => {

				if (result.Success)
				{
					// Load all notes and call the default handler with them
					LoadNotesAsync(NotesLoaded);

					// Load all tags and call the default handler with them
					LoadTagsAsync(TagsLoaded);

					// Call the database changed event handler
					if (DatabaseChanged != null)
					{
						DatabaseChanged(this, new DatabaseChangedEventArgs { Database = _Descriptor });
					}
				}
				else
				{
					OnFatalError(new DatabaseException(result.Exception.Message, result.Exception));
				}

			});
		}

		public void SetDatabase(string type, Dictionary<string, string> props)
		{
			if (props == null) props = new Dictionary<string, string>();
			if (type == null) type = typeof(FileDatabaseDescriptor).FullName;
			IDatabaseDescriptor descriptor = (IDatabaseDescriptor)ModelSerializer.DeserializeModelFromHash(props, GetDatabaseTypes()[type].GetType());
			SetDatabase(descriptor);
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
			SaveNoteRequest req = new SaveNoteRequest { SaveNote = note };
			if (NoteSaving != null) NoteSaving(this, req);
			if (req.Cancel) return;
			new AsyncRunner<SaveNoteResult, Note>().AsyncRun<SaveNoteResult>(SaveNote, NoteSaved, note, saveRequestId);
		}

		public void SaveTagAsync(Tag tag, int? saveRequestId = null)
		{
			SaveTagRequest req = new SaveTagRequest { SaveTag = tag };
			if (TagSaving != null) TagSaving(this, req);
			if (req.Cancel) return;
			new AsyncRunner<SaveTagResult, Tag>().AsyncRun<SaveTagResult>(SaveTag, TagSaved, tag, saveRequestId);
		}

		public void LoadNotesAsync(Action<LoadNotesResult> callback)
		{
			CancelEventArgs args = new CancelEventArgs();
			if (NotesLoading != null) NotesLoading(this, args);
			if (args.Cancel) return;
			new AsyncRunner<LoadNotesResult, object>().AsyncRun(LoadNotes, callback);
		}

		public void LoadTagsAsync(Action<LoadTagsResult> callback)
		{
			CancelEventArgs args = new CancelEventArgs();
			if (TagsLoading != null) TagsLoading(this, args);
			if (args.Cancel) return;
			new AsyncRunner<LoadTagsResult, object>().AsyncRun(LoadTags, callback);
		}

		public void DeleteNoteAsync(Note note)
		{
			DeleteNoteRequest req = new DeleteNoteRequest { DeleteNote = note };
			if (NoteDeleting != null) NoteDeleting(this, req);
			if (req.Cancel) return;
			new AsyncRunner<ObjectDeletedResult, Note>().AsyncRun(DeleteNote, NoteDeleted, note);
		}

		public void DeleteTagAsync(Tag tag)
		{
			DeleteTagRequest req = new DeleteTagRequest { DeleteTag = tag };
			if (TagDeleting != null) TagDeleting(this, req);
			if (req.Cancel) return;
			new AsyncRunner<ObjectDeletedResult, Tag>().AsyncRun(DeleteTag, TagDeleted, tag);
		}

		public Note CloneNote(Note note)
		{
			Note clone = CreateNewNote();
			clone.Title = string.Format(Resources.CopyOf, note.Title);
			clone.Text = note.Text;
			clone.Visible = note.Visible;
			clone.BackgroundColor = note.BackgroundColor;
			clone.ForegroundColor = note.ForegroundColor;
			clone.Dimensions = note.Dimensions;
			clone.Location = new Point(note.Location.X + 16, note.Location.Y + 16);
			clone.Tags = note.Tags;
			var now = DateTime.Now;
			clone.Created = now;
			clone.Modified = now;
			return clone;
		}

		public Note CreateNewNote()
		{
			return _Database.CreateNewNote();
		}

		public Tag CreateNewTag()
		{
			return _Database.CreateNewTag();
		}

		public static void AddTagToNote(Note note, Tag tag)
		{
			if (note.Tags == null) note.Tags = new List<Tag>();
			if (tag.Notes == null) tag.Notes = new List<Note>();
			if (!note.Tags.Contains(tag)) note.Tags.Add(tag);
			if (!tag.Notes.Contains(note)) tag.Notes.Add(note);
		}

		public static void RemoveTagFromNote(Note note, Tag tag)
		{
			if (note.Tags == null) note.Tags = new List<Tag>();
			if (tag.Notes == null) tag.Notes = new List<Note>();
			note.Tags.Remove(tag);
			tag.Notes.Remove(note);
		}

		private static bool IsBrowseable(PropertyInfo prop)
		{
			var attrs = prop.GetCustomAttributes<BrowsableAttribute>().ToList();
			return attrs.Count < 1 || attrs[0].Browsable;
		}

		public static List<PropertyInfo> GetPropertiesForType(IDatabaseDescriptor desc)
		{
			return desc.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pinfo => IsBrowseable(pinfo)).ToList();
		}

		#region Events

		public event Action<LoadNotesResult> NotesLoaded;

		public event Action<SaveNoteResult> NoteSaved;

		public event Action<ObjectDeletedResult> NoteDeleted;

		public event Action<LoadTagsResult> TagsLoaded;

		public event Action<SaveTagResult> TagSaved;

		public event Action<ObjectDeletedResult> TagDeleted;

		public event EventHandler<CancelEventArgs> NotesLoading;

		public event EventHandler<SaveNoteRequest> NoteSaving;

		public event EventHandler<DeleteNoteRequest> NoteDeleting;

		public event EventHandler<CancelEventArgs> TagsLoading;

		public event EventHandler<SaveTagRequest> TagSaving;

		public event EventHandler<DeleteTagRequest> TagDeleting;

		public event EventHandler<DatabaseChangedEventArgs> DatabaseChanged;

		public event EventHandler<DatabaseException> FatalError;

		private void OnFatalError(DatabaseException ex)
		{
			if (FatalError != null)
			{
				FatalError(this, ex);
			}
		}

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

		private BasicResult Initialize()
		{
			try
			{
				_Database.Initialize();
				return new BasicResult { Success = true, Exception = null };
			}
			catch (Exception ex)
			{
				return new BasicResult { Success = false, Exception = ex };
			}
		}

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

		private SaveTagResult SaveTag(Tag tag)
		{
			try
			{
				return new SaveTagResult { Success = true, Exception = null, SavedTag = _Database.SaveTag(tag) };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while saving tag.", ex);
				return new SaveTagResult { Success = false, Exception = ex, SavedTag = tag };
			}
		}

		private LoadNotesResult LoadNotes()
		{
			try
			{
				return new LoadNotesResult { Success = true, Exception = null, NoteList = _Database.GetNotes() };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while loading notes.", ex);
				return new LoadNotesResult { Success = false, Exception = ex, NoteList = null };
			}
		}

		private LoadTagsResult LoadTags()
		{
			try
			{
				return new LoadTagsResult { Success = true, Exception = null, TagList = _Database.GetTags() };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while loading tags.", ex);
				return new LoadTagsResult { Success = false, Exception = ex, TagList = null };
			}
		}

		private ObjectDeletedResult DeleteNote(Note note)
		{
			try
			{
				_Database.DeleteNote(note);
				return new ObjectDeletedResult { Success = true, Exception = null, DeletedId = note.ID };
			}
			catch (Exception ex)
			{
				return new ObjectDeletedResult { Success = false, Exception = ex, DeletedId = null };
			}
		}

		private ObjectDeletedResult DeleteTag(Tag tag)
		{
			try
			{
				_Database.DeleteTag(tag);
				return new ObjectDeletedResult { Success = true, Exception = null, DeletedId = tag.ID };
			}
			catch (Exception ex)
			{
				return new ObjectDeletedResult { Success = false, Exception = ex, DeletedId = null };
			}
		}

		#endregion

		#region Utility types

		#region Request types

		public class SaveNoteRequest : CancelEventArgs
		{
			public Note SaveNote { get; set; }
		}

		public class DeleteNoteRequest : CancelEventArgs
		{
			public Note DeleteNote { get; set; }
		}

		public class SaveTagRequest : CancelEventArgs
		{
			public Tag SaveTag { get; set; }
		}

		public class DeleteTagRequest : CancelEventArgs
		{
			public Tag DeleteTag {get;set;}
		}

		#endregion

		#region Result types

		public class LoadNotesResult : BasicResult
		{
			public Dictionary<string, Note> NoteList { get; set; }
		}

		public class LoadTagsResult : BasicResult
		{
			public Dictionary<string, Tag> TagList { get; set; }
		}

		public class SaveNoteResult : TrackedResult
		{
			public Note SavedNote { get; set; }
		}

		public class SaveTagResult : TrackedResult
		{
			public Tag SavedTag { get; set; }
		}

		public class ObjectDeletedResult : BasicResult
		{
			public string DeletedId { get; set; }
		}

		#endregion

		public class DatabaseChangedEventArgs : EventArgs
		{
			public IDatabaseDescriptor Database { get; set; }
		}

		public class DatabaseException : Exception
		{
			public DatabaseException(string message, Exception inner) : base(message, inner) { }
		}

		#endregion
	}
}
