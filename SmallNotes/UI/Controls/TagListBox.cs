using SmallNotes.Data.Entities;
using SmallNotes.Properties;
using SmallNotes.UI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes.UI.Controls
{
	public class TagListBox : ListBox
	{
		public TagListBox() : base()
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public bool DrawNoteCount { get; set; }

		public void Populate(List<Tag> tags)
		{
			SuspendLayout();
			Items.Clear();
			Items.AddRange(tags.OrderBy(t => t.Title).ToArray());
			PerformLayout();
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Tag> SelectedTags
		{
			get
			{
				return SelectedItems.Cast<Tag>().ToList();
			}
			set
			{
				SuspendLayout();
				for (int i = 0; i < Items.Count; i++ )
				{
					SetSelected(i, value.Contains((Tag)Items[i]));
				}
				PerformLayout();
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if (e.Index >= Items.Count) return;
			if (!(Items[e.Index] is Tag)) return;

			Graphics g = e.Graphics;
			Tag tag = (Tag)Items[e.Index];

			// Paint normal background (selected or not)
			e.DrawBackground();

			// Set smoothing mode
			g.SmoothingMode = SmoothingMode.AntiAlias;

			// Paint special tag background for the first column
			Rectangle rect = new Rectangle(e.Bounds.X + UIElements.TAG_MARGIN, e.Bounds.Y + UIElements.TAG_MARGIN, e.Bounds.Width - (UIElements.TAG_MARGIN * 2), e.Bounds.Height - (UIElements.TAG_MARGIN * 2));
			GraphicsPath roundedRect = UIElements.CreateRoundRect(rect, UIElements.TAG_RADIUS);
			var brush = new SolidBrush(tag.Color);
			g.FillPath(brush, roundedRect);

			// Paint content
			Color foreground = NoteForm.GetAutomaticForegroundColor(tag.Color);
			SolidBrush textBrush = new SolidBrush(foreground);
			StringFormat stringFlags = new StringFormat();
			stringFlags.Alignment = StringAlignment.Near;
			stringFlags.LineAlignment = StringAlignment.Center;
			stringFlags.Trimming = StringTrimming.EllipsisCharacter;
			stringFlags.FormatFlags = StringFormatFlags.NoWrap;

			// Compute text bounds
			Rectangle textBounds = new Rectangle(e.Bounds.X + UIElements.TAG_PADDING, e.Bounds.Y + UIElements.TAG_PADDING, e.Bounds.Width - UIElements.TAG_PADDING * 2, e.Bounds.Height - UIElements.TAG_PADDING * 2);

			// Count string
			if (DrawNoteCount)
			{
				string countStr = string.Format(tag.Notes.Count == 1 ? Resources.TagNotesSingular : Resources.TagNotesPlural, tag.Notes.Count);
				float countStrWidth = g.MeasureString(countStr, e.Font).Width;
				if (textBounds.Width - (countStrWidth + UIElements.TAG_PADDING) > UIElements.MIN_TAG_SIZE)
				{
					RectangleF countStrBounds = new RectangleF(textBounds.Right - countStrWidth, textBounds.Y, countStrWidth, textBounds.Height);
					g.DrawString(countStr, e.Font, textBrush, countStrBounds, new StringFormat(stringFlags));
					textBounds.Width -= ((int)countStrWidth + UIElements.TAG_PADDING);
				}
			}

			// Title string
			g.DrawString(tag.Title, e.Font, textBrush, textBounds, new StringFormat(stringFlags));
		}
	}
}
