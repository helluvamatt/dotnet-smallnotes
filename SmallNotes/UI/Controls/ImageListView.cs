using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes.UI.Controls
{
	public class ImageListView : ListView
	{
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			base.OnDrawItem(e);
		}

		public class ImageListViewItem : ListViewItem
		{

		}
	}
}
