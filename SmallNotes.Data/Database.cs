using Common.Data.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public abstract class Database : IDisposable
	{
		protected string UserDataFolder { get; set; }

		#region Abstract interface

		protected abstract void OnInitialize(Dictionary<string, string> properties);

		public abstract BasicResult SaveNote(Note note);

		public abstract LoadNotesResult LoadNotes();

		#endregion

		public void Initialize(string userDataFolder, Dictionary<string, string> properties)
		{
			UserDataFolder = userDataFolder;
			OnInitialize(properties);
		}

		public void SaveNoteAsync(AsyncCallback<BasicResult> callback, Note note)
		{
			new AsyncRunner<BasicResult, Note>().AsyncRun(SaveNote, callback, note);
		}

		public void LoadNotesAsync(AsyncCallback<LoadNotesResult> callback)
		{
			new AsyncRunner<LoadNotesResult, object>().AsyncRun(LoadNotes, callback);
		}

		public virtual void Dispose()
		{
			// Do nothing by default
		}

		public class LoadNotesResult : BasicResult
		{
			public List<Note> NoteList { get; set; }
		}
	}
}
