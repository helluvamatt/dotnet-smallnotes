using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallNotes.UI
{
	public class SimpleTemplate : Dictionary<string,string>
	{
		public string Template { get; set; }

		public string Render()
		{
			if (Template == null) throw new TemplateException("Template is null.");
			Regex replacementRegex = new Regex(@"\$([A-Za-z_0-9]+)\$");
			return replacementRegex.Replace(Template, match => this[match.Groups[1].Value]);
		}

		public class TemplateException : Exception
		{
			public TemplateException(string message)
				: base(message)
			{ }
		}
	}
}
