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
using SQLiteNetExtensions.Extensions;
using SmallNotes.Data.Entities;
using SmallNotes.Data.FileDatabaseImpl.Entities;

namespace SmallNotes.Data.FileDatabaseImpl
{
	public class FileDatabase : IDatabase
	{
		private const string DEFAULT_DBFILE = "Notes.db";

		private string _SavePath;
		private SQLiteConnectionString _ConnStr;
		private SQLiteConnectionPool _ConnPool;

		private ILog Logger { get; set; }

		public FileDatabase(FileDatabaseDescriptor fdd)
		{
			Logger = LogManager.GetLogger(GetType());
			_SavePath = fdd.DbFile;
		}

		#region IDatabase implementation

		public void Initialize()
		{
			// Make sure SavePath points to the database file, not just the location where the file might be
			if (Directory.Exists(_SavePath))
			{
				_SavePath = Path.Combine(_SavePath, DEFAULT_DBFILE);
			}

			_ConnPool = new SQLiteConnectionPool(new SQLite.Net.Platform.Win32.SQLitePlatformWin32());
			_ConnStr = new SQLiteConnectionString(_SavePath, true);

			// Create the table structure (may also migrate if needed)
			try
			{
				SQLiteConnection conn = CreateConnection();
				// Ensure the database schema is ready
				conn.CreateTable<SQLiteNote>();
				conn.CreateTable<SQLiteTag>();
				conn.CreateTable<SQLiteNoteTags>();
			}
			catch (SQLiteException ex)
			{
				Logger.Error("Failed to create Notes database.", ex);
				File.Delete(_SavePath);
				throw ex;
			}
		}

		public Note SaveNote(Note note)
		{
			var now = DateTime.Now;
			note.Modified = now;
			if (string.IsNullOrEmpty(note.ID)) note.Created = now;
			CreateConnection().InsertOrReplaceWithChildren(note);
			Logger.DebugFormat("Saved note. ID = {0}", note.ID);
			return note;
		}

		public Tag SaveTag(Tag tag)
		{
			CreateConnection().InsertOrReplaceWithChildren(tag);
			Logger.DebugFormat("Saved tag. ID = {0}", tag.ID);
			return tag;
		}

		public Dictionary<string, Note> GetNotes()
		{
			Dictionary<string, Note> noteList = new Dictionary<string, Note>();
			foreach (Note note in CreateConnection().GetAllWithChildren<SQLiteNote>())
			{
				noteList.Add(note.ID, note);
			}
			return noteList;
		}

		public Dictionary<string, Tag> GetTags()
		{
			Dictionary<string, Tag> tagList = new Dictionary<string, Tag>();
			foreach (Tag tag in CreateConnection().GetAllWithChildren<SQLiteTag>())
			{
				tagList.Add(tag.ID, tag);
			}
			return tagList;
		}

		public void DeleteNote(Note note)
		{
			Logger.DebugFormat("Deleting note '{0}'...", note.Title);
			CreateConnection().Delete(note, true);
		}

		public void DeleteTag(Tag tag)
		{
			CreateConnection().Delete(tag, true);
		}

		public Note CreateNewNote()
		{
			return new SQLiteNote() { _Tags = new List<SQLiteTag>() };
		}

		public Tag CreateNewTag()
		{
			return new SQLiteTag() { _Notes = new List<SQLiteNote>() };
		}

		public void Dispose()
		{
			// Do nothing
		}

		#endregion

		private SQLiteConnection CreateConnection()
		{
			return _ConnPool.GetConnection(_ConnStr);
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
