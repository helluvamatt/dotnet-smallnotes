using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SmallNotes.UI.Editors
{
	public class BackgroundColorEditor : ColorEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			return DoEditValue(provider, value, SmallNotesTrayApplicationContext.BackgroundColorList);
		}
	}
}
