using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SmallNotes.Data.Entities;

namespace SmallNotes.Data
{
	public interface IDatabase : IDisposable
	{
		void Initialize();

		Note SaveNote(Note note);

		Tag SaveTag(Tag tag);

		Dictionary<string, Note> GetNotes();

		Dictionary<string, Tag> GetTags();

		void DeleteNote(Note note);

		void DeleteTag(Tag tag);

		Note CreateNewNote();

		Tag CreateNewTag();
	}
}
