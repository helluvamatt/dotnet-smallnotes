using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallNotes.UI
{
	public class SimpleHtmlTemplate : Dictionary<string,string>
	{
		public string HtmlTemplate { get; set; }

		public string Render()
		{
			if (HtmlTemplate == null) throw new TemplateException("HtmlTemplate is null.");
			Regex replacementRegex = new Regex(@"%([A-Za-z_0-9]+)%");
			return replacementRegex.Replace(HtmlTemplate, match => this[match.Groups[1].Value]);
		}

		public class TemplateException : Exception
		{
			public TemplateException(string message)
				: base(message)
			{ }
		}
	}
}
