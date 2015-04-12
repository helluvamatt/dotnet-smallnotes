using SmallNotes.Data.Entities;
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
	public partial class TagForm : Form
	{
		#region Properties

		private Tag _Data;
		public Tag Data
		{
			get
			{
				return _Data;
			}
			set
			{
				_Data = value;
				colorButton.BackColor = _Data != null ? _Data.Color : Color.White;
				titleTextBox.Text = _Data != null ? _Data.Title : string.Empty;
			}
		}

		private Func<Tag> _TagFactory;

		#endregion

		public TagForm(Func<Tag> tagFactory)
		{
			if (tagFactory == null) throw new ArgumentNullException("tagFactory");
			_TagFactory = tagFactory;
			InitializeComponent();
			Data = _TagFactory.Invoke();
		}

		private void colorButton_Click(object sender, EventArgs e)
		{
			colorDialog.Color = Data.Color;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				Data.Color = colorDialog.Color;
				colorButton.BackColor = Data.Color;
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (Data == null) Data = _TagFactory.Invoke();
			Data.Title = titleTextBox.Text;
		}
	}
}
