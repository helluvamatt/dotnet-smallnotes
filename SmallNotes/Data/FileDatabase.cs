using Common.Data.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Common.Data;
using SQLite.Net;
using SmallNotes.Data.Entities;

namespace SmallNotes.Data
{
	public class FileDatabase : Database
	{
		private const string PROP_KEY_DBFILE = "DbFile";
		private const string DEFAULT_DBFILE = "Notes.db";

		#region SavePath property
		private string _SavePath;
		protected string SavePath
		{
			get
			{
				return _SavePath;
			}
			set
			{
				if (Path.IsPathRooted(value))
				{
					_SavePath = value;
				}
				else
				{
					_SavePath = Path.Combine(UserDataFolder, value);
				}
			}
		}
		#endregion

		private ILog Logger { get; set; }

		public override SaveNoteResult SaveNote(Note note)
		{
			try
			{
				using (SQLiteConnection conn = CreateConnection())
				{
					conn.InsertOrReplace(note);
					Logger.DebugFormat("Saved note. ID = {0}", note.ID);
					return new SaveNoteResult { Success = true, Exception = null, SavedNote = note };
				}
			}
			catch (SQLiteException ex)
			{
				Logger.Error("Exception while saving note.", ex);
				return new SaveNoteResult { Success = false, Exception = ex, SavedNote = null };
			}
		}

		public override Database.LoadNotesResult LoadNotes()
		{
			try
			{
				Dictionary<string, Note> noteList = new Dictionary<string, Note>();
				using (SQLiteConnection conn = CreateConnection())
				{
					foreach (Note note in conn.Table<SQLiteNote>())
					{
						noteList.Add(note.ID, note);
					}
				}
				return new LoadNotesResult { Success = true, Exception = null, NoteList = noteList };
			}
			catch (SQLiteException ex)
			{
				Logger.Error("Exception while loading notes.", ex);
				return new LoadNotesResult { Success = false, Exception = ex, NoteList = null };
			}
		}

		protected override void OnInitialize(Dictionary<string, string> properties)
		{
			Logger = LogManager.GetLogger(GetType());
			SavePath = properties.ContainsKey(PROP_KEY_DBFILE) ? properties[PROP_KEY_DBFILE] : DEFAULT_DBFILE;

			// Make sure SavePath points to the database file, not just the location where the file might be
			if (Directory.Exists(SavePath))
			{
				SavePath = Path.Combine(SavePath, DEFAULT_DBFILE);
			}

			// If the database file does not exist yet, create it
			if (!File.Exists(SavePath))
			{
				try
				{
					using (SQLiteConnection conn = CreateConnection(true))
					{
						// Ensure the database schema is ready
						conn.CreateTable<SQLiteNote>();
					}
				}
				catch (SQLiteException ex)
				{
					Logger.Error("Failed to create Notes database.", ex);
					File.Delete(SavePath);
				}
			}
		}

		public override Note CreateNewNote()
		{
			return new SQLiteNote();
		}

		private SQLiteConnection CreateConnection(bool create = false)
		{
			return new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), SavePath, true);
		}
	}
}
