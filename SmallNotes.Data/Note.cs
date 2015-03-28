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
		public string ID { get; private set; }
		public bool Visible { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }

		public Note(string id)
		{
			ID = id;
			Visible = true;
		}
	}
}
