using Common.Data.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Common.Data;

namespace SmallNotes.Data
{
	public class FileDatabase : Database
	{
		private const string PROP_KEY_SAVEPATH = "SavePath";
		private const string DEFAULT_SAVEPATH = "Notes";

		private const string TITLE_SENTINEL = "[//]: # (TITLE:";

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
				if (note.ID == null)
				{
					// Figure out the largest ID and add one
					note.ID = (Directory.EnumerateFiles(SavePath).Max(fname => Int64.Parse(fname.Substring(0, fname.Length - 4))) + 1).ToString();
				}
				string fileName = Path.Combine(SavePath, note.ID + ".txt");
				using (TextWriter textWriter = File.CreateText(fileName))
				{
					textWriter.WriteLine("[//]: # (TITLE:{0})", note.Title);
					textWriter.Write(note.Text);
				}
				return new SaveNoteResult { Success = true, Exception = null, SavedNote = note };
			}
			catch (Exception ex)
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
				foreach (string fileName in Directory.EnumerateFiles(SavePath))
				{
					using (TextReader textReader = File.OpenText(fileName))
					{
						// TODO Need to come up with a new format with the proper ID, title, and visibility properties
						//Note note = new Note();
						//string titleLine = textReader.ReadLine();
						//note.Title = titleLine.Substring(TITLE_SENTINEL.Length, titleLine.Length - TITLE_SENTINEL.Length - 1);
						//note.Text = textReader.ReadToEnd();
						//noteList.Add(note.ID, note);
					}
				}
				return new LoadNotesResult { Success = true, Exception = null, NoteList = noteList };
			}
			catch (Exception ex)
			{
				Logger.Error("Exception while loading notes.", ex);
				return new LoadNotesResult { Success = false, Exception = ex, NoteList = null };
			}
		}

		protected override void OnInitialize(Dictionary<string, string> properties)
		{
			SavePath = properties.ContainsKey(PROP_KEY_SAVEPATH) ? properties[PROP_KEY_SAVEPATH] : DEFAULT_SAVEPATH;
			Logger = LogManager.GetLogger(GetType());
		}
	}
}
