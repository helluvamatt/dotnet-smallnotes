using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class DefaultColorValueAttribute : DefaultValueAttribute
	{
		public DefaultColorValueAttribute(string hexCode) : base(ColorTranslator.FromHtml(hexCode)) { }
	}
}
