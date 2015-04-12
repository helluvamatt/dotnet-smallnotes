using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.Entities
{
	public abstract class Note
	{
		public virtual string ID { get; set; }
		public virtual string Title { get; set; }
		public virtual bool Visible { get; set; }
		public virtual string Text { get; set; }
		public virtual Color BackgroundColor { get; set; }
		public virtual Color? ForegroundColor { get; set; }
		public virtual Size Dimensions { get; set; }
		public virtual Point Location { get; set; }
		public virtual List<Tag> Tags { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual DateTime Modified { get; set; }

		public bool IsChangedFrom(Note other)
		{
			return other.Title != Title || other.Text != Text;
		}
	}
}
