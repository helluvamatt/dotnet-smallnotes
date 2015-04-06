using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SmallNotes.UI.Utils
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

			public Image Icon { get; set; }

			#region Color property

			private Color? _Color;
			public Color? Color
			{
				get
				{
					return _Color;
				}
				set
				{
					_Color = value;
					Icon = _Color.HasValue ? DrawColorIcon(_Color.Value) : DrawEmptyCustomIcon();
				}
			}

			public string HexColor
			{
				get
				{
					return Color.HasValue ? ColorTranslator.ToHtml(Color.Value) : null;
				}
				set
				{
					Color = value != null ? ColorTranslator.FromHtml(value) : (Color?)null;
				}
			}

			#endregion

			public override string ToString()
			{
				return Name;
			}
		}

		public static Image DrawColorIcon(Color c, int w = 16, int h = 16)
		{
			Bitmap bm = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(bm);
			g.DrawRectangle(Pens.White, 0, 0, w - 1, h - 1);
			Brush b = new SolidBrush(c);
			g.FillRectangle(b, 1, 1, bm.Width - 1, bm.Height - 1);
			return bm;
		}

		public static Image DrawEmptyCustomIcon(int w = 16, int h = 16)
		{
			Rectangle area = new Rectangle(0, 0, w - 1, w - 1);
			Bitmap im = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(im);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			Pen blackPen = new Pen(Color.Black, 1);
			g.FillRectangle(new SolidBrush(Color.White), area);
			g.DrawRectangle(blackPen, area);
			g.DrawLine(blackPen, new Point(0, 0), new Point(w - 1, h - 1));
			g.DrawLine(blackPen, new Point(0, h - 1), new Point(w - 1, 0));
			return im;
		}
	}
}
