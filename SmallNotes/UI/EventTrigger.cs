using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.UI
{
	[ComVisibleAttribute(true)]
	public class EventTrigger
	{
		public void trigger()
		{
			if (Handler != null) Handler(this, new EventArgs());
		}

		public event EventHandler Handler;
	}
}
