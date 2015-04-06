using SmallNotes.Properties;
using SmallNotes.UI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SmallNotes.UI.Editors
{
	public class ForegroundColorEditor : ColorEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ColorList list = new ColorList();
			list.Items.Add(new ColorList.ColorItem(Resources.ForegroundColor_Automatic) { HexColor = null });
			list.Items.Add(new ColorList.ColorItem(Resources.Black) { Color = Color.Black });
			list.Items.Add(new ColorList.ColorItem(Resources.White) { Color = Color.White });
			return DoEditValue(provider, value, list);
		}

		#region ForegroundColorTypeConverter

		public class ForegroundColorTypeConverter : TypeConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return value != null ? (string)value : Resources.ForegroundColor_Automatic;
				}
				return "(none)";
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return false;
			}
		}

		#endregion
	}
}
