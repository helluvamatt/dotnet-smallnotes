using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SmallNotes.UI
{
	public class ResizePanel : Panel
	{
		private const int GRIPPER_SIZE = 20;
		private const int GRIPPER_X_OFFSET = 18;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.Status.Gripper.Normal))
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
				Rectangle gripperArea = GetResizeRect();
				renderer.DrawBackground(e.Graphics, gripperArea);
			}
		}

		public Rectangle GetResizeRect()
		{
			return new Rectangle(Width - GRIPPER_X_OFFSET, Height - GRIPPER_SIZE, GRIPPER_SIZE, GRIPPER_SIZE);
		}
	}
}
