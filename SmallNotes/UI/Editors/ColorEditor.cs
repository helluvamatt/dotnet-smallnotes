using SmallNotes.Properties;
using SmallNotes.UI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SmallNotes.UI.Editors
{
	public abstract class ColorEditor : UITypeEditor
	{
		private IWindowsFormsEditorService _editorService;
		private ListBox _ListBox;

		#region UITypeEditor implementation

		public abstract override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value);

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(e.Value != null ? ValueToColor(e.Value) : Color.White), e.Bounds);
		}

		#endregion

		#region Event handlers

		private void listBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			Rectangle bounds = e.Bounds;
			Graphics g = e.Graphics;
			ColorList.ColorItem item = (ColorList.ColorItem)_ListBox.Items[e.Index];
			e.DrawBackground();
			g.DrawImage(item.Icon, bounds.X, bounds.Y, bounds.Height, bounds.Height);
			bounds.X += bounds.Height;
			SolidBrush brush = new SolidBrush(e.ForeColor);
			StringFormat stringFlags = new StringFormat();
			stringFlags.Alignment = StringAlignment.Near;
			stringFlags.LineAlignment = StringAlignment.Center;
			g.DrawString(item.Name, e.Font, brush, bounds, new StringFormat(stringFlags));
		}

		private void listBox_SelectedValueChanged(object sender, EventArgs e)
		{
			_editorService.CloseDropDown();
		}

		#endregion

		#region Utility methods

		protected static Color ValueToColor(object value)
		{
			return ColorTranslator.FromHtml(value.ToString());
		}

		protected static string ColorToValue(Color color)
		{
			return ColorTranslator.ToHtml(color);
		}

		protected object DoEditValue(IServiceProvider provider, object value, ColorList list)
		{
			_editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			// Build and display ListView
			_ListBox = new ListBox();
			_ListBox.SelectionMode = SelectionMode.One;
			_ListBox.SelectedValueChanged += listBox_SelectedValueChanged;
			_ListBox.DrawMode = DrawMode.OwnerDrawFixed;
			_ListBox.DrawItem += listBox_DrawItem;
			bool foundStandardColor = false;
			foreach (ColorList.ColorItem item in list.Items)
			{
				int index = _ListBox.Items.Add(item);
				if (value != null && value.ToString() == item.HexColor)
				{
					_ListBox.SelectedIndex = index;
					foundStandardColor = true;
				}
			}
			ColorList.ColorItem customListItem = new ColorList.ColorItem(Resources.CustomMenuItem);
			customListItem.HexColor = !foundStandardColor && value != null ? value.ToString() : null;
			_ListBox.Items.Add(customListItem);
			_editorService.DropDownControl(_ListBox);

			// Process result
			if (_ListBox.SelectedIndex > -1)
			{
				ColorList.ColorItem selectedItem = (ColorList.ColorItem)_ListBox.Items[_ListBox.SelectedIndex];
				if (selectedItem == customListItem)
				{
					ColorDialog colorPicker = new ColorDialog();
					colorPicker.FullOpen = true;
					if (value != null)
					{
						colorPicker.Color = ValueToColor(value);
					}
					if (colorPicker.ShowDialog() == DialogResult.OK)
					{
						return ColorToValue(colorPicker.Color);
					}
				}
				else
				{
					return selectedItem.HexColor;
				}
			}
			return value;
		}

		#endregion
	}
}
