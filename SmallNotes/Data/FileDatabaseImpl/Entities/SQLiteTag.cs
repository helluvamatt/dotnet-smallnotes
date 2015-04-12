using SmallNotes.Data.Entities;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.FileDatabaseImpl.Entities
{
	class SQLiteTag : Tag
	{
		[AutoIncrement, PrimaryKey]
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

		[NotNull, Default(value: true)]
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

		[NotNull, Default(value: "#ffffff")]
		public string ColorHex
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

		[Ignore]
		public override Color Color
		{
			get
			{
				return base.Color;
			}
			set
			{
				base.Color = value;
			}
		}

		[ManyToMany(typeof(SQLiteNoteTags))]
		public List<SQLiteNote> _Notes
		{
			get
			{
				return Notes != null ? Notes.Cast<SQLiteNote>().ToList() : new List<SQLiteNote>();
			}
			set
			{
				Notes = value != null ? value.Cast<Note>().ToList() : new List<Note>();
			}
		}

		[Ignore]
		public override List<Note> Notes
		{
			get
			{
				return base.Notes;
			}
			set
			{
				base.Notes = value;
			}
		}
	}
}
