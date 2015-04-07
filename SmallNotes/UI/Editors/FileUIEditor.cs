using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SmallNotes.UI.Editors
{
	public class FileUIEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.Filter = "All Files (*.*)|*.*";
			saveDialog.CheckFileExists = true;
			string filename = (string)value;
			saveDialog.FileName = filename;
			DialogResult res = saveDialog.ShowDialog();
			if (res == DialogResult.OK)
			{
				filename = saveDialog.FileName;
			}
			return filename;
		}
	}
}
