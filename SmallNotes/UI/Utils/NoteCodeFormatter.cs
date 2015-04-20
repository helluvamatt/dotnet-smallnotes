using ColorCode;
using ColorCode.Common;
using ColorCode.Formatting;
using ColorCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SmallNotes.UI.Utils
{
	public class NoteCodeFormatter : IFormatter
	{
		private string _CodeBlockTitle;

		public NoteCodeFormatter(string codeBlockTitle)
		{
			_CodeBlockTitle = codeBlockTitle;
		}

		public void Write(string parsedSourceCode, IList<Scope> scopes, IStyleSheet styleSheet, TextWriter textWriter)
		{
			var styleInsertions = new List<TextInsertion>();
			foreach (Scope scope in scopes)
			{
				GetStyleInsertionsForCapturedStyle(scope, styleInsertions);
			}
			styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

			using (System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(textWriter))
			{
				int offset = 0;
				foreach (TextInsertion styleInsertion in styleInsertions)
				{
					htmlWriter.WriteEncodedText(parsedSourceCode.Substring(offset, styleInsertion.Index - offset));
					if (styleInsertion.Scope != null)
					{
						if (styleSheet.Styles.Contains(styleInsertion.Scope.Name))
						{
							Style style = styleSheet.Styles[styleInsertion.Scope.Name];
							htmlWriter.AddAttribute("class", style.CssClassName);
							if (style.Foreground != Color.Empty) htmlWriter.AddStyleAttribute("color", style.Foreground.ToHtmlColor());
							if (style.Background != Color.Empty) htmlWriter.AddStyleAttribute("background-color", style.Background.ToHtmlColor());
							if (style.Italic) htmlWriter.AddStyleAttribute("font-style", "italic");
							if (style.Bold) htmlWriter.AddStyleAttribute("font-weight", "bold");
						}
						htmlWriter.RenderBeginTag("span");
					}
					else
					{
						htmlWriter.RenderEndTag();
					}
					offset = styleInsertion.Index;
				}

				htmlWriter.WriteEncodedText(parsedSourceCode.Substring(offset));
			}
		}

		public void WriteFooter(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
		{
			Guard.ArgNotNull(styleSheet, "styleSheet");
			Guard.ArgNotNull(language, "language");
			Guard.ArgNotNull(textWriter, "textWriter");

			using (System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(textWriter))
			{
				htmlWriter.WriteEndTag("pre");
				htmlWriter.WriteEndTag("fieldset");
			}
		}

		public void WriteHeader(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
		{
			Guard.ArgNotNull(styleSheet, "styleSheet");
			Guard.ArgNotNull(language, "language");
			Guard.ArgNotNull(textWriter, "textWriter");

			using (System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(textWriter))
			{
				Color foreground = Color.Empty;
				Color background = Color.Empty;
				if (styleSheet.Styles.Contains(ScopeName.PlainText))
				{
					Style plainTextStyle = styleSheet.Styles[ScopeName.PlainText];
					foreground = plainTextStyle.Foreground;
					background = plainTextStyle.Background;
				}

				// Code block title
				htmlWriter.AddAttribute("class", "codeBlockTitle");
				htmlWriter.RenderBeginTag("div");
				htmlWriter.Write(_CodeBlockTitle);
				htmlWriter.RenderEndTag();

				// Code block style start
				htmlWriter.AddAttribute("class", "codeBlock " + language.CssClassName);
				if (foreground != Color.Empty) htmlWriter.AddStyleAttribute("color", foreground.ToHtmlColor());
				if (background != Color.Empty) htmlWriter.AddStyleAttribute("background-color", background.ToHtmlColor());
				htmlWriter.RenderBeginTag("div");
				
				// Pre start
				htmlWriter.RenderBeginTag("pre");
			}
		}

		private static void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
		{
			styleInsertions.Add(new TextInsertion
			{
				Index = scope.Index,
				Scope = scope
			});

			foreach (Scope childScope in scope.Children)
			{
				GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);
			}

			styleInsertions.Add(new TextInsertion { Index = scope.Index + scope.Length });
		}
	}
}
