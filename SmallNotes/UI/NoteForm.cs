using SmallNotes.Data;
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
	public partial class NoteForm : Form
	{
		#region Note data property
		private Note _Data;
		public Note Data
		{
			get
			{
				return _Data;
			}
			set
			{
				_Data = value;
				UpdateUI();
			}
		}
		#endregion

		public NoteForm()
		{
			InitializeComponent();
		}

		protected void UpdateUI()
		{
			//if (Data != null)
		}
	}
}
