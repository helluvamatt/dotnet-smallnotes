using SmallNotes.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SmallNotes.UI.Editors
{
	public abstract class TrackBarEditor : UITypeEditor
	{
		private IWindowsFormsEditorService _editorService;
		private TrackBarEditorControl _trackBarEditorControl;

		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public abstract override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value);

		protected object DoEditValue(IServiceProvider provider, object value, string label, int min, int max, int smallChange, int largeChange)
		{
			_editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			_trackBarEditorControl = new TrackBarEditorControl(label, min, max, smallChange, largeChange);
			_trackBarEditorControl.Value = (long)value;
			_editorService.DropDownControl(_trackBarEditorControl);
			return _trackBarEditorControl.Value;
		}
	}
}
