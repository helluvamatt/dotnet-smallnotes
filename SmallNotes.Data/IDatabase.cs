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
		Note SaveNote(Note request);

		Dictionary<string, Note> LoadNotes();

		Note CreateNewNote();
	}
}
