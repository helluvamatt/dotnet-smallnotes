using SmallNotes.Data.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class TestDatabase : IDatabase
	{
		private Dictionary<string, Note> _Notes;

		public TestDatabase(IDatabaseDescriptor descriptor)
		{
			_Notes = new Dictionary<string, Note>();
			Note testNote = CreateNewNote();
			testNote.ID = "1";
			testNote.Title = "Test Note 1";
			testNote.Text = "# This is a test\r\n\r\nThis is only a test.\r\n\r\n[Google](http://www.google.com)\r\n\r\n";
			testNote.ForegroundColor = null;
			testNote.BackgroundColor = ColorTranslator.FromHtml("#000099");
			testNote.Visible = true;
			AddNote(testNote);

			Note testNote2 = CreateNewNote();
			testNote2.ID = "2";
			testNote2.Title = "Test Note 2";
			testNote2.Text = "# Test class\r\n\r\n```c#:HelloWorld.cs\r\nusing System;\r\npublic class TestClass {\r\n    public void DoWork() {\r\n        Console.WriteLine(\"Hello, world!\")\r\n    }\r\n}\r\n```\r\n";
			testNote2.ForegroundColor = Color.Maroon; // Custom color
			testNote2.BackgroundColor = Color.AliceBlue; // Custom color
			testNote2.Visible = true;
			AddNote(testNote2);

			Note testNote3 = CreateNewNote();
			testNote3.ID = "3";
			testNote3.Title = "Image Test";
			testNote3.Text = "![Image test](http://i.imgur.com/IUWmOG1.jpg)\r\n";
			testNote3.ForegroundColor = null;
			testNote3.BackgroundColor = Color.Goldenrod;
			testNote3.Visible = true;
			AddNote(testNote3);
		}

		private void AddNote(Note note)
		{
			_Notes[note.ID] = note;
		}

		public Note SaveNote(Note note)
		{
			if (note.ID == null)
			{
				long largestId = _Notes.Values.Max(n => Int64.Parse(n.ID));
				note.ID = (largestId + 1).ToString();
			}
			AddNote(note);
			return note;
		}

		public Dictionary<string, Note> LoadNotes()
		{
			return _Notes;
		}

		public Note CreateNewNote()
		{
			return new TestNote();
		}

		public void Dispose()
		{
			// Do nothing
		}

		public class TestNote : Note { }
	}
}
