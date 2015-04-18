using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmallNotes.UI.Utils
{
	class ImageUtil
	{
		public static Bitmap LoadSVG(string url)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(url);
			SvgDocument svgDoc = SvgDocument.Open(xmlDoc);
			Bitmap svgImg = new Bitmap((int)svgDoc.Width, (int)svgDoc.Height, PixelFormat.Format32bppArgb);
			svgDoc.Draw(svgImg);
			return svgImg;
		}
	}
}
