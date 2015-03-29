using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class Note
	{
		public string ID { get; set; }
		public bool Visible { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }

		public Note()
		{
			Visible = true;
		}

		public bool IsChangedFrom(Note other)
		{
			return other.Title != Title || other.Text != Text;
		}

		public override bool Equals(object obj)
		{
			return obj is Note && ((Note)obj).ID == ID;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}
