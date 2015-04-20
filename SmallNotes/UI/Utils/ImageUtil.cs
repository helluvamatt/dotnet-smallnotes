using Common.Data.Async;
using SmallNotes.Data.Entities;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WinForms;

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

		public static void RenderNoteToBitmap(Action<RenderNoteRequest> handler, RenderNoteRequest req, int? requestId = null)
		{
			new AsyncRunner<RenderNoteRequest, RenderNoteRequest>().AsyncRun<RenderNoteRequest>(RenderNoteToBitmap, handler, req, requestId);
		}

		private static RenderNoteRequest RenderNoteToBitmap(RenderNoteRequest req)
		{
			try
			{
				using (Image noteRender = HtmlRender.RenderToImage(NoteForm.RenderNoteToHtml(req.Note, req.CustomCss, NoteForm.StylesheetTemplate), req.Note.Dimensions, req.Note.BackgroundColor, null, null, HtmlRender_ImageLoad))
				{
					int dimension = Math.Min(noteRender.Width, noteRender.Height);
					Rectangle clipRect = new Rectangle(0, 0, dimension, dimension);
					Rectangle tileSize = new Rectangle(new Point(0, 0), req.Dimensions);
					Bitmap tile = new Bitmap(tileSize.Width, tileSize.Height);
					using (Graphics g = Graphics.FromImage(tile))
					{
						g.SmoothingMode = SmoothingMode.AntiAlias;
						g.DrawImage(noteRender, tileSize, clipRect, GraphicsUnit.Pixel);
					}
					req.Success = true;
					req.Image = tile;
				}
			}
			catch (Exception ex)
			{
				req.Success = false;
				req.Exception = ex;
			}

			return req;
		}

		private static void HtmlRender_ImageLoad(object sender, HtmlImageLoadEventArgs args)
		{
			var ext = Path.GetExtension(args.Src);
			if (ext != null && ext.Equals(".svg", StringComparison.OrdinalIgnoreCase))
			{
				args.Handled = true;
				args.Callback(LoadSVG(args.Src));
			}
		}

		public class RenderNoteRequest : TrackedResult
		{
			#region Inputs

			public RenderNoteRequest(Note note, Size dimensions, string customCss)
			{
				Note = note;
				Dimensions = dimensions;
				CustomCss = customCss;
			}
			
			public Size Dimensions { get; private set; }
			public string CustomCss { get; private set; }
			public Note Note { get; private set; }
			
			#endregion

			#region Outputs
			
			public Image Image { get; set; }
			
			#endregion
		}
	}
}
