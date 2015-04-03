using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.Entities
{
	class SQLiteNote : Note
	{
		[AutoIncrement]
		[PrimaryKey]
		public int? _ID
		{
			get
			{
				return ID != null ? int.Parse(ID) : (int?)null;
			}
			set
			{
				ID = value.ToString();
			}
		}

		[Ignore]
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				base.ID = value;
			}
		}

		[NotNull]
		public override string Title
		{
			get
			{
				return base.Title;
			}
			set
			{
				base.Title = value;
			}
		}

		[NotNull]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		[NotNull]
		[Default(value: true)]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		[NotNull]
		public string BackgroundColorHex
		{
			get
			{
				return ColorTranslator.ToHtml(BackgroundColor);
			}
			set
			{
				BackgroundColor = ColorTranslator.FromHtml(value);
			}
		}

		public string ForegroundColorHex
		{
			get
			{
				return ForegroundColor.HasValue ? ColorTranslator.ToHtml(ForegroundColor.Value) : null;
			}
			set
			{
				ForegroundColor = string.IsNullOrEmpty(value) ? (Color?)null : ColorTranslator.FromHtml(value);
			}
		}

		[Ignore]
		public override Color BackgroundColor
		{
			get
			{
				return base.BackgroundColor;
			}
			set
			{
				base.BackgroundColor = value;
			}
		}

		[Ignore]
		public override Color? ForegroundColor
		{
			get
			{
				return base.ForegroundColor;
			}
			set
			{
				base.ForegroundColor = value;
			}
		}
	}
}
