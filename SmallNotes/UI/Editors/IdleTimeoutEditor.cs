using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.UI.Editors
{
	public class IdleTimeoutEditor : TrackBarEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			return DoEditValue(provider, value, Resources.IdleTimeoutLabel, 0, 5000, 100, 500);
		}

		public class IdleTimeoutConverter : TypeConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType.IsAssignableFrom(typeof(string));
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				return string.Format(Resources.IdleTimeoutLabel, value);
			}
		}
	}
}
