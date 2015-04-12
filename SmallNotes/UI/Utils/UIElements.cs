using SmallNotes.Data.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.UI.Utils
{
	public class UIElements
	{
		#region Colored Resize Gripper

		const int GRIPPER_PADDING = 1;
		const int GRIPPER_SQUARE_SIZE = 3;
		private static Size squareSize = new Size(GRIPPER_SQUARE_SIZE, GRIPPER_SQUARE_SIZE);
		public const int RESIZE_GRIPPER_SIZE = (GRIPPER_PADDING * 5) + (GRIPPER_SQUARE_SIZE * 4); // pixels

		/// <summary>
		/// Draw a custom colored resize gripper
		/// </summary>
		/// <param name="g">Graphics to draw to</param>
		/// <param name="clipRect">Bounds for the draw</param>
		/// <param name="color">Custom color</param>
		public static void DrawColoredResizeGripper(Graphics g, Rectangle clipRect, Color color)
		{
			SmoothingMode origSmoothing = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.Default;

			Point[] squares = new Point[] {
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE), clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 3),
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 2, clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 2),
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE), clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 2),
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 3, clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE)),
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE) * 2, clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE)),
				new Point(clipRect.Right - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE), clipRect.Bottom - (GRIPPER_PADDING + GRIPPER_SQUARE_SIZE)),
			};

			SolidBrush brush = new SolidBrush(color);
			foreach (Point squareStart in squares)
			{
				g.FillRectangle(brush, new Rectangle(squareStart, squareSize));
			}

			g.SmoothingMode = origSmoothing;
		}

		#endregion

		#region Tag

		public const int TAG_PADDING = 3; // pixels
		public const int TAG_MARGIN = 3; // pixels
		public const int MIN_TAG_SIZE = 20; // pixels
		public const int TAG_RADIUS = 4;

		/// <summary>
		/// Draw a tag in the space, ellipsize text if necessary, return the actual draw bounds for the tag
		/// </summary>
		/// <param name="g">Graphics to draw into</param>
		/// <param name="bounds">Bounds for this tag</param>
		/// <param name="tag">Tag object to draw</param>
		/// <param name="font">Font to use when drawing</param>
		/// <param name="style">FontStyle to use when drawing</param>
		/// <returns>Rectangle representing the actually drawn bounds for this tag (may be smaller than the given bounds)</returns>
		public static RectangleF DrawTag(Graphics g, RectangleF bounds, Tag tag, Font font, FontStyle style = FontStyle.Regular)
		{
			// Measure tag
			StringFormat stringFlags = new StringFormat();
			stringFlags.Alignment = StringAlignment.Near;
			stringFlags.LineAlignment = StringAlignment.Center;
			stringFlags.Trimming = StringTrimming.EllipsisCharacter;
			stringFlags.FormatFlags = StringFormatFlags.NoWrap;
			RectangleF textBounds = new RectangleF(bounds.X + TAG_PADDING, bounds.Y + TAG_PADDING, bounds.Width - (TAG_PADDING * 2), bounds.Height - (TAG_PADDING * 2));
			float actualTextWidth = g.MeasureString(tag.Title, font, textBounds.Size, new StringFormat(stringFlags)).Width;
			if (textBounds.Width != actualTextWidth)
			{
				textBounds = new RectangleF(textBounds.X, textBounds.Y, actualTextWidth, textBounds.Height);
				bounds = new RectangleF(bounds.X, bounds.Y, actualTextWidth + TAG_PADDING * 2, bounds.Height);
			}

			// Define foreground brush
			Color foreground = NoteForm.GetAutomaticForegroundColor(tag.Color);
			SolidBrush foregroundBrush = new SolidBrush(foreground);

			// Draw background 
			GraphicsPath roundRectPath = CreateRoundRect(bounds, TAG_RADIUS);
			g.FillPath(new SolidBrush(tag.Color), roundRectPath);

			// Draw border (looks weird)
			//g.DrawPath(new Pen(foregroundBrush, 1), roundRectPath);

			// Draw label text
			g.DrawString(tag.Title, font, foregroundBrush, textBounds, new StringFormat(stringFlags));

			// Done
			return bounds;
		}

		#endregion

		#region Rounded Rectangle paths

		public static GraphicsPath CreateRoundRect(Rectangle rect, int radius)
		{
			return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
		}

		public static GraphicsPath CreateRoundRect(RectangleF rect, float radius)
		{
			return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
		}

		public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.AddLine(x + radius, y, x + width - (radius * 2), y); // Line
			gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90); // Corner
			gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2)); // Line
			gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
			gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height); // Line
			gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90); // Corner
			gp.AddLine(x, y + height - (radius * 2), x, y + radius); // Line
			gp.AddArc(x, y, radius * 2, radius * 2, 180, 90); // Corner
			gp.CloseFigure();
			return gp;
		}

		#endregion
	}
}
