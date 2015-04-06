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
	public class FileDatabase : IDatabase
	{
		private const string DEFAULT_DBFILE = "Notes.db";

		private string _SavePath;

		private ILog Logger { get; set; }

		public FileDatabase(FileDatabaseDescriptor fdd)
		{
			Logger = LogManager.GetLogger(GetType());
			_SavePath = fdd.DbFile;

			// Make sure SavePath points to the database file, not just the location where the file might be
			if (Directory.Exists(_SavePath))
			{
				_SavePath = Path.Combine(_SavePath, DEFAULT_DBFILE);
			}

			// If the database file does not exist yet, create it
			if (!File.Exists(_SavePath))
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
					File.Delete(_SavePath);
				}
			}
		}

		#region IDatabase implementation

		public Note SaveNote(Note note)
		{
			using (SQLiteConnection conn = CreateConnection())
			{
				conn.InsertOrReplace(note);
				Logger.DebugFormat("Saved note. ID = {0}", note.ID);
				return note;
			}
		}

		public Dictionary<string, Note> LoadNotes()
		{
			Dictionary<string, Note> noteList = new Dictionary<string, Note>();
			using (SQLiteConnection conn = CreateConnection())
			{
				foreach (Note note in conn.Table<SQLiteNote>())
				{
					noteList.Add(note.ID, note);
				}
			}
			return noteList;
		}

		public Note CreateNewNote()
		{
			return new SQLiteNote();
		}

		public void Dispose()
		{
			// Do nothing
		}

		#endregion

		private SQLiteConnection CreateConnection(bool create = false)
		{
			return new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), _SavePath, true);
		}

		public class SerializeToPropsAttribute : Attribute
		{
			public SerializeToPropsAttribute(string name)
			{
				Name = name;
			}

			public string Name { get; set; }
		}
	}
}
