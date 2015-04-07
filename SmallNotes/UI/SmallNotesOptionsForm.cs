using Common.Data;
using Common.Data.Async;
using Common.TrayApplication;
using log4net;
using SmallNotes.Data;
using SmallNotes.Data.Entities;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace SmallNotes.UI
{
	public partial class SmallNotesOptionsForm : OptionsForm
	{
		#region NoteList Property

		private Dictionary<string, Note> _NoteList;
		public Dictionary<string, Note> NoteList
		{
			private get
			{
				return _NoteList;
			}
			set
			{
				_NoteList = value;
				PopulateListView();
			}
		}

		#endregion

		#region Private members

		private SettingsManager<Settings> _SettingsManager;

		private ILog Logger { get; set; }

		#endregion

		public SmallNotesOptionsForm(SettingsManager<Settings> sm) : base()
		{
			// Init logger
			Logger = LogManager.GetLogger(GetType());

			// Init form
			InitializeComponent();

			// Populate settings
			_SettingsManager = sm;
		}

		#region Public members

		public void UpdateNote(Note note)
		{
			NoteList[note.ID] = note;
			PopulateListView();
		}

		public void PopulateSettings()
		{
			settingsPropertyGrid.SelectedObject = _SettingsManager.SettingsObject;

			string notesListView = _SettingsManager.SettingsObject.NotesListView;
			if (!string.IsNullOrEmpty(notesListView))
			{
				View view = (View)Enum.Parse(typeof(View), notesListView);
				OnSetView(view, false);
			}
			else
			{
				OnSetView(View.SmallIcon, false);
			}

			PopulateDatabaseTypesComboBox();
			PopulateDatabaseProperties();
		}

		#endregion

		#region Event handlers

		#region Form global

		private void optionsFormTabControl_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			
			// Get the item from the collection.
			TabPage _tabPage = optionsFormTabControl.TabPages[e.Index];

			// Get the real bounds for the tab rectangle.
			Rectangle _tabBounds = optionsFormTabControl.GetTabRect(e.Index);

			// Draw background
			if (e.State == DrawItemState.Selected)
			{
				g.FillRectangle(Brushes.White, e.Bounds);
			}
			else
			{
				g.FillRectangle(new SolidBrush(BackColor), e.Bounds);
			}

			// Draw icon image
			if (_tabPage == notesTabPage)
			{
				g.DrawImage(Resources.ic_notes, _tabBounds.X, _tabBounds.Y, _tabBounds.Height, _tabBounds.Height);
				_tabBounds.X += _tabBounds.Height;
			}
			else if (_tabPage == settingsTabPage)
			{
				g.DrawImage(Resources.ic_action_settings, _tabBounds.X, _tabBounds.Y, _tabBounds.Height, _tabBounds.Height);
				_tabBounds.X += _tabBounds.Height;
			}
			else if (_tabPage == databaseTabPage)
			{
				g.DrawImage(Resources.ic_content_save, _tabBounds.X, _tabBounds.Y, _tabBounds.Height, _tabBounds.Height);
				_tabBounds.X += _tabBounds.Height;
			}

			// Draw string
			SolidBrush _brush = new SolidBrush(Color.Black);
			StringFormat _stringFlags = new StringFormat();
			_stringFlags.Alignment = StringAlignment.Near;
			_stringFlags.LineAlignment = StringAlignment.Center;
			g.DrawString(_tabPage.Text, e.Font, _brush, _tabBounds, new StringFormat(_stringFlags));
		}

		#endregion

		#region Notes tab

		private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnSetView(View.Details, true);
		}

		private void listToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnSetView(View.List, true);
		}

		private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnSetView(View.SmallIcon, true);
		}

		private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnSetView(View.LargeIcon, true);
		}

		private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnSetView(View.Tile, true);
		}

		#endregion

		#region Settings tab

		private void settingsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			OnOptionChanged(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
		}

		#endregion

		#region Database Tab

		private void refreshButton_Click(object sender, EventArgs e)
		{
			PopulateDatabaseTypesComboBox();
		}

		private void databaseTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_SettingsManager.SettingsObject.DatabaseInformation = (IDatabaseDescriptor)databaseTypeComboBox.SelectedItem;
			PopulateDatabaseProperties();
			OnOptionChanged("DatabaseInformation", _SettingsManager.SettingsObject.DatabaseInformation);
		}

		private void databasePropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			OnOptionChanged("DatabaseInformation", _SettingsManager.SettingsObject.DatabaseInformation);
		}

		#endregion

		#endregion

		#region Utility methods

		private void CheckRadioMenu(ToolStripDropDownItem menu, ToolStripItem selectedItem)
		{
			foreach (ToolStripItem item in menu.DropDownItems)
			{
				if (item != null && item is ToolStripMenuItem)
				{
					((ToolStripMenuItem)item).Checked = item == selectedItem;
				}
			}
		}

		private void OnSetView(View value, bool propagateSetting)
		{
			notesListView.View = value;
			switch (value)
			{
				case View.List:
					viewToolStripDropDownButton.Image = Resources.application_view_list;
					notesListView.SmallImageList = _iconImageList;
					CheckRadioMenu(viewToolStripDropDownButton, listToolStripMenuItem);
					break;
				case View.Details:
					viewToolStripDropDownButton.Image = Resources.application_view_detail;
					notesListView.SmallImageList = _iconImageList;
					CheckRadioMenu(viewToolStripDropDownButton, detailsToolStripMenuItem);
					break;
				case View.SmallIcon:
					viewToolStripDropDownButton.Image = Resources.application_view_columns;
					notesListView.SmallImageList = _smallImageList;
					CheckRadioMenu(viewToolStripDropDownButton, smallIconsToolStripMenuItem);
					break;
				case View.LargeIcon:
					viewToolStripDropDownButton.Image = Resources.application_view_icons;
					notesListView.SmallImageList = _smallImageList;
					CheckRadioMenu(viewToolStripDropDownButton, largeIconsToolStripMenuItem);
					break;
				case View.Tile:
					viewToolStripDropDownButton.Image = Resources.application_view_tile;
					notesListView.SmallImageList = _smallImageList;
					CheckRadioMenu(viewToolStripDropDownButton, tilesToolStripMenuItem);
					break;
			}
			if (propagateSetting)
			{
				_SettingsManager.SettingsObject.NotesListView = value.ToString();
				OnOptionChanged("NotesListView", value);
			}
		}

		private void PopulateListView()
		{
			// TODO Support organizing notes into folders

			// Populate list view
			notesListView.SuspendLayout();
			notesListView.Items.Clear();
			_iconImageList.Images.Clear();
			_smallImageList.Images.Clear();
			_largeImageList.Images.Clear();

			int index = 0;
			foreach (KeyValuePair<string, Note> entry in _NoteList)
			{
				try
				{
					using (Image noteRender = HtmlRender.RenderToImage(NoteForm.RenderNoteToHtml(entry.Value, _SettingsManager.SettingsObject.CustomCss), entry.Value.Dimensions, entry.Value.BackgroundColor))
					{
						int dimension = Math.Min(noteRender.Width, noteRender.Height);
						Rectangle clipRect = new Rectangle(0, 0, dimension, dimension);
						Rectangle tileSize = new Rectangle(new Point(0, 0), _largeImageList.ImageSize);
						Bitmap tile = new Bitmap(tileSize.Width, tileSize.Height);
						using (Graphics g = Graphics.FromImage(tile))
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.DrawImage(noteRender, tileSize, clipRect, GraphicsUnit.Pixel);
						}
						_largeImageList.Images.Add(tile);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("Failed to render note preview", ex);
					_largeImageList.Images.Add(Resources.ic_notes_large);
				}

				_smallImageList.Images.Add(Resources.ic_notes_large);
				_iconImageList.Images.Add(Resources.note);

				// Create the ListViewItem
				ListViewItem item = new ListViewItem();
				item.Text = entry.Value.Title;
				item.ImageIndex = index;
				notesListView.Items.Add(item);

				index++;
			}
			notesListView.PerformLayout();
		}

		private void PopulateDatabaseTypesComboBox()
		{
			Dictionary<string, IDatabaseDescriptor> types = DatabaseManager.GetDatabaseTypes(true);
			databaseTypeComboBox.DataSource = types.Values.ToList();
			databaseTypeComboBox.DisplayMember = "DisplayName";
		}

		private void PopulateDatabaseProperties()
		{
			databasePropertyGrid.SelectedObject = _SettingsManager.SettingsObject.DatabaseInformation;
		}

		#endregion
	}
}
