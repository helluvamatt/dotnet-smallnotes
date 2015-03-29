using Common.Data.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class TestDatabase : Database
	{
		private Dictionary<string, Note> _Notes;

		protected override void OnInitialize(Dictionary<string, string> properties)
		{
			_Notes = new Dictionary<string, Note>();
			Note testNote = new Note();
			testNote.ID = "1";
			testNote.Title = "Test Note 1";
			testNote.Text = "This is a test\r\n\r\nThis is only a test.\r\n\r\n`Piece of code\r\n\r\n";
			AddNote(testNote);
		}

		private void AddNote(Note note)
		{
			_Notes[note.ID] = note;
		}

		public override SaveNoteResult SaveNote(Note note)
		{
			if (note.ID == null)
			{
				long largestId = _Notes.Values.Max(n => Int64.Parse(n.ID));
				note.ID = (largestId + 1).ToString();
			}
			AddNote(note);
			return new SaveNoteResult { Success = true, Exception = null, SavedNote = note };
		}

		public override Database.LoadNotesResult LoadNotes()
		{
			return new Database.LoadNotesResult { Success = true, Exception = null, NoteList = _Notes };
		}
	}
}
