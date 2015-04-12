using SmallNotes.Data.Entities;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes.UI
{
	public partial class NoteTagForm : Form
	{
		private string _NoteTitle;
		public string NoteTitle
		{
			get
			{
				return _NoteTitle;
			}
			set
			{
				_NoteTitle = value;
				Text = string.Format(Resources.TagsFor, _NoteTitle);
			}
		}

		private List<Tag> _TagList;
		public List<Tag> TagList
		{
			get
			{
				return _TagList;
			}
			set
			{
				_TagList = value;
				tagListBox.Populate(_TagList);
			}
		}

		public List<Tag> SelectedTagList
		{
			get
			{
				return tagListBox.SelectedTags;
			}
			set
			{
				tagListBox.SelectedTags = value;
			}
		}

		public NoteTagForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, true);
		}
	}
}
