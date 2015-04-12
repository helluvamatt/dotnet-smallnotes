using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.FileDatabaseImpl.Entities
{
	class SQLiteNoteTags
	{
		[ForeignKey(typeof(SQLiteNote))]
		public int NoteId { get; set; }

		[ForeignKey(typeof(SQLiteTag))]
		public int TagId { get; set; }
	}
}
