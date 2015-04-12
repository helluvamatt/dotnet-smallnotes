using Newtonsoft.Json;
using SmallNotes.Data.Entities;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallNotes.Data.FileDatabaseImpl.Entities
{
	class SQLiteNote : Note
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

		[NotNull]
		public override DateTime Created
		{
			get
			{
				return base.Created;
			}
			set
			{
				base.Created = value;
			}
		}

		[NotNull]
		public override DateTime Modified
		{
			get
			{
				return base.Modified;
			}
			set
			{
				base.Modified = value;
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

		[Ignore, FileDatabase.SerializeToProps("ForegroundColor")]
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

		[Ignore, FileDatabase.SerializeToProps("BackgroundColor")]
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

		[Ignore]
		public override Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		[Ignore, FileDatabase.SerializeToProps("Location")]
		public string LocationStr
		{
			get
			{
				return string.Format(_commaSeparatedDigitPairStr, Location.X, Location.Y);
			}
			set
			{
				
				Match m = _commaSeparatedDigitPair.Match(value);
				if (m.Success)
				{
					int x = int.Parse(m.Groups[1].Value);
					int y = int.Parse(m.Groups[2].Value);
					Location = new Point(x, y);
				}
				else
				{
					Location = new Point(0, 0);
				}
			}
		}

		[Ignore]
		public override Size Dimensions
		{
			get
			{
				return base.Dimensions;
			}
			set
			{
				base.Dimensions = value;
			}
		}

		[Ignore, FileDatabase.SerializeToProps("Dimensions")]
		public string DimensionStr
		{
			get
			{
				return string.Format(_commaSeparatedDigitPairStr, Dimensions.Width, Dimensions.Height);
			}
			set
			{
				Match m = _commaSeparatedDigitPair.Match(value);
				if (m.Success)
				{
					int w = int.Parse(m.Groups[1].Value);
					int h = int.Parse(m.Groups[2].Value);
					Dimensions = new Size(w, h);
				}
				else
				{
					Dimensions = new Size(0, 0);
				}
			}
		}

		[NotNull]
		public string Properties
		{
			get
			{
				Dictionary<string, string> props = new Dictionary<string, string>();
				foreach (PropertyInfo prop in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					var attrs = prop.GetCustomAttributes(typeof(FileDatabase.SerializeToPropsAttribute));
					if (attrs.Count() == 1)
					{
						props.Add(((FileDatabase.SerializeToPropsAttribute)attrs.First()).Name, (string)prop.GetValue(this));
					}
				}
				return JsonConvert.SerializeObject(props);
			}
			set
			{
				Dictionary<string, string> props = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
				foreach (PropertyInfo prop in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					var attrs = prop.GetCustomAttributes(typeof(FileDatabase.SerializeToPropsAttribute));
					if (attrs.Count() == 1)
					{
						string key = ((FileDatabase.SerializeToPropsAttribute)attrs.First()).Name;
						if (props.ContainsKey(key))
						{
							prop.SetValue(this, props[key]);
						}
					}
				}
			}
		}

		[ManyToMany(typeof(SQLiteNoteTags))]
		public List<SQLiteTag> _Tags
		{
			get
			{
				return Tags != null ? Tags.Cast<SQLiteTag>().ToList() : new List<SQLiteTag>();
			}
			set
			{
				Tags = value != null ? value.Cast<Tag>().ToList() : new List<Tag>();
			}
		}

		[Ignore]
		public override List<Tag> Tags
		{
			get
			{
				return base.Tags;
			}
			set
			{
				base.Tags = value;
			}
		}

		private static Regex _commaSeparatedDigitPair = new Regex("(\\d+),(\\d+)");

		private const string _commaSeparatedDigitPairStr = "{0},{1}";
	}
}
