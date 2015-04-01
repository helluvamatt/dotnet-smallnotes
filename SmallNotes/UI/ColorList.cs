using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SmallNotes.UI
{
	public class ColorList
	{
		public List<ColorItem> Items { get; private set; }

		public ColorList()
		{
			Items = new List<ColorItem>();
		}

		#region XML parsing

		/// <summary>
		/// Read and parse XML data to populate the list of colors
		/// </summary>
		/// <param name="reader">TextReader to read XML data</param>
		/// <exception cref="XmlException">Throws XmlException on a parsing error</exception>
		public void LoadFromXml(TextReader reader)
		{
			Items.Clear();
			bool inColors = false;
			using (XmlReader xmlReader = XmlReader.Create(reader))
			{
				// Parse XML file and load colors
				while (xmlReader.Read())
				{
					switch (xmlReader.NodeType)
					{
						case XmlNodeType.Element:
							if (xmlReader.Name == TAG_COLORS)
							{
								inColors = true;
							}
							else if (xmlReader.Name == TAG_COLOR)
							{
								if (inColors)
								{
									string name = xmlReader.GetAttribute(ATTR_NAME);
									if (name == null) throw new XmlException(string.Format("<{0}> tag is missing required attribute: {1}", TAG_COLOR, ATTR_NAME));
									string hex = xmlReader.GetAttribute(ATTR_HEX);
									if (hex == null) throw new XmlException(string.Format("<{0}> tag is missing required attribute: {1}", TAG_COLOR, ATTR_HEX));
									ColorItem newItem = new ColorItem(name);
									newItem.HexColor = hex;
									Items.Add(newItem);
								}
								else
								{
									throw new XmlException("<color> tags must be contained within a parent <colors> element!");
								}
							}
							else
							{
								throw new XmlException(string.Format("Invalid tag: <{0}>", xmlReader.Name));
							}
							break;
						case XmlNodeType.Text:
						case XmlNodeType.XmlDeclaration:
						case XmlNodeType.ProcessingInstruction:
						case XmlNodeType.Comment:
							// Ignore
							break;
						case XmlNodeType.EndElement:
							if (xmlReader.Name == TAG_COLORS)
							{
								inColors = false;
							}
							else
							{
								throw new XmlException(string.Format("Invalid end tag: {0}", xmlReader.Name));
							}
							break;
					}
				}
			}
		}

		private const string TAG_COLORS = "colors";
		private const string TAG_COLOR = "color";
		private const string ATTR_NAME = "name";
		private const string ATTR_HEX = "hex";

		#endregion

		public class ColorItem
		{
			public ColorItem(string name)
			{
				Name = name;
			}

			public string Name { get; private set; }

			public Bitmap Icon { get; set; }

			#region Color property

			private Color _Color;
			public Color Color
			{
				get
				{
					return _Color;
				}
				set
				{
					_Color = value;
					Bitmap bm = new Bitmap(16, 16);
					Graphics g = Graphics.FromImage(bm);
					g.DrawRectangle(Pens.White, 0, 0, bm.Width, bm.Height);
					if (_Color != null)
					{
						Brush b = new SolidBrush(_Color);
						g.FillRectangle(b, 1, 1, bm.Width - 1, bm.Height - 1);
					}
					Icon = bm;
				}
			}

			public string HexColor
			{
				get
				{
					return ColorTranslator.ToHtml(Color);
				}
				set
				{
					Color = ColorTranslator.FromHtml(value);
				}
			}

			#endregion

			public override string ToString()
			{
				return Name;
			}
		}
	}
}
