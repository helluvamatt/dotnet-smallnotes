using Common.Data.Async;
using log4net;
using SmallNotes.Data.Entities;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	internal sealed class DatabaseManager
	{
		#region Private members

		private ILog Logger { get; set; }

		private IDatabase _Database;
		private IDatabaseDescriptor _Descriptor;
		private string _UserDataFolder;

		#endregion

		#region Properties

		public IDatabaseDescriptor Descriptor
		{
			get
			{
				return _Descriptor;
			}
			set
			{
				_Descriptor = value;
				// Clean up existing database if exists
				if (_Database != null)
				{
					_Database.Dispose();
				}
				// Initialize new one
				_Database = _Descriptor.InitializeDatabase(_UserDataFolder);
				LoadNotes();
			}
		}

		#endregion

		public DatabaseManager(string userDataFolder)
		{
			Logger = LogManager.GetLogger(GetType());
			_UserDataFolder = userDataFolder;
		}

		public void SaveNoteAsync(Note note, int? saveRequestId = null)
		{
			new AsyncRunner<SaveNoteResult, Note>().AsyncRun(SaveNote, NoteSaved, note);
		}

		public void LoadNotesAsync()
		{
			new AsyncRunner<LoadNotesResult, object>().AsyncRun(LoadNotes, NotesLoaded);
		}

		public Note CreateNewNote()
		{
			return _Database.CreateNewNote();
		}

		#region Event handling

		#region Events

		public event Action<SaveNoteResult> NoteSaved;

		public event Action<LoadNotesResult> NotesLoaded;

		#endregion

		#endregion

		#region Async runner methods

		private SaveNoteResult SaveNote(Note note)
		{
			try
			{
				return new SaveNoteResult { Success = true, Exception = null, SavedNote = _Database.SaveNote(note) };
			}
			catch (SQLiteException ex)
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
			catch (SQLiteException ex)
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

		#endregion
	}
}
