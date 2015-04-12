using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes.UI.Controls
{
	public partial class TrackBarEditorControl : UserControl
	{
		private string _labelDisplayString;

		public long Value
		{
			get
			{
				return (long)trackBar.Value;
			}
			set
			{
				if (value > int.MaxValue) value = int.MaxValue;
				if (value < int.MinValue) value = int.MinValue;
				trackBar.Value = (int)value;
			}
		}

		public TrackBarEditorControl(string labelDisplayString, int min, int max, int smallChange, int largeChange)
		{
			_labelDisplayString = labelDisplayString;
			InitializeComponent();
			trackBar.Minimum = min;
			trackBar.Maximum = max;
			trackBar.SmallChange = smallChange;
			trackBar.LargeChange = largeChange;
			trackBar.TickFrequency = smallChange;
			UpdateUI();
		}

		private void trackBar_ValueChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		private void UpdateUI()
		{
			displayLabel.Text = string.Format(_labelDisplayString, trackBar.Value);
		}
	}
}
