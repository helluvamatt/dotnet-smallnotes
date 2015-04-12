using SmallNotes.Data.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.TestDatabaseImpl
{
	public class TestDatabase : IDatabase
	{
		private Dictionary<string, Note> _Notes;
		private Dictionary<string, Tag> _Tags;

		public TestDatabase(IDatabaseDescriptor descriptor)
		{
			_Notes = new Dictionary<string, Note>();
			_Tags = new Dictionary<string, Tag>();
		}

		public void Initialize()
		{
			Tag testTag = CreateTag("1", "Test Tag", Color.Turquoise);
			Tag testTag2 = CreateTag("2", "Test Tag 2", Color.Fuchsia);
			Tag testTag3 = CreateTag("3", "Test Tag 3", Color.Khaki);
			Tag testTag4 = CreateTag("4", "Test Tag 4", Color.LightSeaGreen);
			Tag withImageTag = CreateTag("5", "Images", Color.Orange);
			Tag withLinkTag = CreateTag("6", "Links", Color.Purple);
			Tag longTag = CreateTag("7", "Tag with a really really long title", Color.ForestGreen);
			
			AddTag(testTag);
			AddTag(testTag2);
			AddTag(testTag3);
			AddTag(testTag4);
			AddTag(withImageTag);
			AddTag(withLinkTag);
			AddTag(longTag);

			Note testNote = CreateNewNote();
			testNote.ID = "1";
			testNote.Title = "Test Note 1";
			testNote.Text = "# This is a test\r\n\r\nThis is only a test.\r\n\r\n[Google](http://www.google.com)\r\n\r\n";
			testNote.ForegroundColor = null;
			testNote.BackgroundColor = ColorTranslator.FromHtml("#000099");
			testNote.Location = new Point(10, 10);
			testNote.Dimensions = new Size(300, 300);
			testNote.Visible = false;
			DatabaseManager.AddTagToNote(testNote, withLinkTag);
			DatabaseManager.AddTagToNote(testNote, longTag);
			DatabaseManager.AddTagToNote(testNote, testTag);
			DatabaseManager.AddTagToNote(testNote, testTag2);
			DatabaseManager.AddTagToNote(testNote, testTag4);
			DatabaseManager.AddTagToNote(testNote, testTag3);
			AddNote(testNote);

			Note testNote2 = CreateNewNote();
			testNote2.ID = "2";
			testNote2.Title = "Test Note 2";
			testNote2.Text = "# Test class\r\n\r\n```c#:HelloWorld.cs\r\nusing System;\r\npublic class TestClass {\r\n    public void DoWork() {\r\n        Console.WriteLine(\"Hello, world!\")\r\n    }\r\n}\r\n```\r\n";
			testNote2.ForegroundColor = Color.Maroon; // Custom color
			testNote2.BackgroundColor = Color.AliceBlue; // Custom color
			testNote2.Location = new Point(30, 30);
			testNote2.Dimensions = new Size(300, 300);
			testNote2.Visible = false;
			DatabaseManager.AddTagToNote(testNote2, testTag);
			AddNote(testNote2);

			Note testNote3 = CreateNewNote();
			testNote3.ID = "3";
			testNote3.Title = "Image Test";
			testNote3.Text = "![Image test](http://i.imgur.com/IUWmOG1.jpg)\r\n";
			testNote3.ForegroundColor = null;
			testNote3.BackgroundColor = Color.Goldenrod;
			testNote3.Location = new Point(50, 50);
			testNote3.Dimensions = new Size(300, 300);
			testNote3.Visible = false;
			DatabaseManager.AddTagToNote(testNote3, withImageTag);
			DatabaseManager.AddTagToNote(testNote3, testTag);
			AddNote(testNote3);
		}

		private Tag CreateTag(string id, string title, Color color)
		{
			return new TestTag { ID = id, Title = title, Color = color, Visible = true };
		}

		private void AddNote(Note note)
		{
			_Notes[note.ID] = note;
		}

		private void AddTag(Tag tag)
		{
			_Tags[tag.ID] = tag;
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

		public Tag SaveTag(Tag tag)
		{
			if (tag.ID == null)
			{
				long largestId = _Tags.Values.Max(t => Int64.Parse(t.ID));
				tag.ID = (largestId + 1).ToString();
			}
			AddTag(tag);
			return tag;
		}

		public Dictionary<string, Note> GetNotes()
		{
			return _Notes;
		}

		public Dictionary<string, Tag> GetTags()
		{
			return _Tags;
		}

		public void DeleteNote(Note note)
		{
			if (note != null && note.ID != null)
			{
				_Notes.Remove(note.ID);
			}
		}

		public void DeleteTag(Tag tag)
		{
			if (tag != null && tag.ID != null)
			{
				_Tags.Remove(tag.ID);
			}
		}

		public Note CreateNewNote()
		{
			return new TestNote();
		}

		public Tag CreateNewTag()
		{
			return new TestTag();
		}

		public void Dispose()
		{
			// Do nothing
		}

		public class TestNote : Note { }

		public class TestTag : Tag { }
	}
}
