using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.Entities
{
	public class Tag
	{
		public virtual string ID { get; set; }
		public virtual string Title { get; set; }
		public virtual bool Visible { get; set; }
		public virtual Color Color { get; set; }
		public virtual List<Note> Notes { get; set; }
	}
}
