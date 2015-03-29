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

		public abstract SaveNoteResult SaveNote(Note request);

		public abstract LoadNotesResult LoadNotes();

		#endregion

		public void Initialize(string userDataFolder, Dictionary<string, string> properties)
		{
			UserDataFolder = userDataFolder;
			OnInitialize(properties);
		}

		public void SaveNoteAsync(AsyncCallback<SaveNoteResult> callback, Note note, int? saveRequestId = null)
		{
			new AsyncRunner<SaveNoteResult, Note>().AsyncRun(SaveNote, callback, note);
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
			public Dictionary<string, Note> NoteList { get; set; }
		}

		public class SaveNoteRequest
		{
			public Note _Note { get; set; }
		}

		public class SaveNoteResult : TrackedResult
		{
			public Note SavedNote { get; set; }
		}
	}
}
