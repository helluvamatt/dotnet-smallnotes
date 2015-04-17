using Common.Data;
using Common.Data.Async;
using Common.TrayApplication;
using log4net;
using SmallNotes.Data;
using SmallNotes.Data.Entities;
using SmallNotes.Properties;
using SmallNotes.UI.Controls;
using SmallNotes.UI.Utils;
using SmallNotes.UI.Utils.Win32Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace SmallNotes.UI
{
	internal partial class SmallNotesOptionsForm : OptionsForm
	{
		#region Properties

		public IDatabaseDescriptor SelectedDatabase { get; set; }

		#endregion

		#region Private members

		private SettingsManager<Settings> _SettingsManager;
		private DatabaseManager _DatabaseManager;
		private HotkeyManager _HotkeyManager;
		private Dictionary<string, IDatabaseDescriptor> _Types = new Dictionary<string, IDatabaseDescriptor>();
		private Dictionary<string, Note> _NoteList = new Dictionary<string, Note>();
		private Dictionary<string, Tag> _TagList = new Dictionary<string, Tag>();

		private ILog Logger { get; set; }

		#endregion

		public SmallNotesOptionsForm(SettingsManager<Settings> sm, DatabaseManager dm, HotkeyManager hm) : base()
		{
			// Init logger
			Logger = LogManager.GetLogger(GetType());

			// Init form
			InitializeComponent();

			// Populate settings
			_SettingsManager = sm;
			_DatabaseManager = dm;
			_HotkeyManager = hm;

			// Bind events
			_DatabaseManager.NotesLoading += _DatabaseManager_NotesLoading;
			_DatabaseManager.NotesLoaded += _DatabaseManager_NotesLoaded;
			_DatabaseManager.NoteSaved += _DatabaseManager_NoteSaved;
			_DatabaseManager.NoteDeleted += _DatabaseManager_NoteDeleted;
			_DatabaseManager.TagsLoading += _DatabaseManager_TagsLoading;
			_DatabaseManager.TagsLoaded += _DatabaseManager_TagsLoaded;
			_DatabaseManager.TagSaved += _DatabaseManager_TagSaved;
			_DatabaseManager.TagDeleted += _DatabaseManager_TagDeleted;
		}

		#region Public members

		private void UpdateNote(Note note)
		{
			_NoteList[note.ID] = note;
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

			SelectedDatabase = _DatabaseManager.DatabaseDescriptor;
			PopulateDatabaseTypesComboBox();

			// Hotkeys
			hotkeyEnabledCheckBox.CheckedChanged -= hotkeyEnabledCheckBox_CheckedChanged;
			hotkeyEnabledCheckBox.Checked = _SettingsManager.SettingsObject.HotkeysEnabled;
			hotkeyEnabledCheckBox.CheckedChanged += hotkeyEnabledCheckBox_CheckedChanged;
			SetHotkeysEnabled(_SettingsManager.SettingsObject.HotkeysEnabled);

			// Populate hotkeys
			hotkeyListLayoutPanel.Controls.Clear();
			int index = 0;
			foreach (KeyValuePair<string, GlobalHotkeyHook> entry in _HotkeyManager.GetHooks())
			{
				Logger.InfoFormat("Adding hotkey definition: {0} - {1}", entry.Key, entry.Value.Name);
				HotkeyListItem item = new HotkeyListItem(entry.Key, entry.Value);
				item.Dock = DockStyle.Fill;
				item.HotkeyChanged += hotkeyListItems_HotkeyChanged;
				hotkeyListLayoutPanel.Controls.Add(item, 0, index);
				index++;
			}
		}

		#endregion

		#region Events

		private void OnNewNoteAction()
		{
			if (NewNoteAction != null) NewNoteAction();
		}

		public event Action NewNoteAction;

		private void OnShowNoteAction(Note note)
		{
			if (ShowNoteAction != null) ShowNoteAction(note);
		}

		public event Action<Note> ShowNoteAction;

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
			else if (_tabPage == tagsTabPage)
			{
				g.DrawImage(Resources.ic_tag, _tabBounds.X, _tabBounds.Y, _tabBounds.Height, _tabBounds.Height);
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
			else if (_tabPage == hotkeysTabPage)
			{
				g.DrawImage(Resources.ic_shortcuts, _tabBounds.X, _tabBounds.Y, _tabBounds.Height, _tabBounds.Height);
				_tabBounds.X += _tabBounds.Height;
			}

			// Draw string
			SolidBrush _brush = new SolidBrush(Color.Black);
			StringFormat _stringFlags = new StringFormat();
			_stringFlags.Alignment = StringAlignment.Near;
			_stringFlags.LineAlignment = StringAlignment.Center;
			g.DrawString(_tabPage.Text, e.Font, _brush, _tabBounds, new StringFormat(_stringFlags));
		}

		private void _DatabaseManager_NotesLoading(object sender, CancelEventArgs e)
		{
			notesTabProgressBar.Visible = true;
		}

		private void _DatabaseManager_NotesLoaded(DatabaseManager.LoadNotesResult result)
		{
			notesTabProgressBar.Visible = false;
			if (result.Success)
			{
				_NoteList = result.NoteList;
				PopulateListView();
			}
			else
			{
				// TODO Handle error
			}
		}

		private void _DatabaseManager_NoteSaved(DatabaseManager.SaveNoteResult result)
		{
			if (result.Success && result.SavedNote != null)
			{
				UpdateNote(result.SavedNote);
			}
		}

		private void _DatabaseManager_NoteDeleted(DatabaseManager.ObjectDeletedResult result)
		{
			if (result.Success)
			{
				_NoteList.Remove(result.DeletedId);
				PopulateListView();
			}
		}

		private void _DatabaseManager_TagsLoading(object sender, CancelEventArgs e)
		{
			tagsTabProgressBar.Visible = true;
			tagsListBox.Visible = false;
		}

		private void _DatabaseManager_TagsLoaded(DatabaseManager.LoadTagsResult result)
		{
			tagsTabProgressBar.Visible = false;
			tagsListBox.Visible = true;
			if (result.Success)
			{
				_TagList = result.TagList;
			}
			else
			{
				_TagList = new Dictionary<string, Tag>();
				// TODO Handle error
			}
			tagsListBox.Populate(_TagList.Values.ToList());
		}

		private void _DatabaseManager_TagSaved(DatabaseManager.SaveTagResult result)
		{
			_TagList[result.SavedTag.ID] = result.SavedTag;
			tagsListBox.Populate(_TagList.Values.ToList());
		}

		private void _DatabaseManager_TagDeleted(DatabaseManager.ObjectDeletedResult result)
		{
			if (_TagList.Remove(result.DeletedId))
			{
				tagsListBox.Populate(_TagList.Values.ToList());
			}
		}

		#endregion

		#region Notes tab

		private void newNoteToolStripButton_Click(object sender, EventArgs e)
		{
			OnNewNoteAction();
		}

		private void exportToMarkdownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO Export to Markdown
		}

		private void exportToHTMLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO Export to HTML
		}

		private void exportToPlaintextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO Export to Plaintext
		}

		private void printToolStripButton_Click(object sender, EventArgs e)
		{
			// TODO Print a note
			// How?
		}

		private void copyToolStripButton_Click(object sender, EventArgs e)
		{
			// Make a copy of the selected notes
			foreach (ListViewItem item in notesListView.SelectedItems)
			{
				Note note = (Note)item.Tag;
				Note clone = _DatabaseManager.CloneNote(note);
				_DatabaseManager.SaveNoteAsync(clone);
			}
		}

		private void deleteToolStripButton_Click(object sender, EventArgs e)
		{
			DoDeleteSelectedNotes();
		}

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

		private void notesListView_ItemActivate(object sender, EventArgs e)
		{
			foreach (ListViewItem item in notesListView.SelectedItems)
			{
				Note note = (Note)item.Tag;
				OnShowNoteAction(note);
			}
		}

		private void notesListView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				DoDeleteSelectedNotes();
			}
		}

		#endregion

		#region Tags tab

		private void newTagToolStripButton_Click(object sender, EventArgs e)
		{
			TagForm tagForm = new TagForm(_DatabaseManager.CreateNewTag);
			if (tagForm.ShowDialog() == DialogResult.OK)
			{
				_DatabaseManager.SaveTagAsync(tagForm.Data);
			}
		}

		private void deleteTagToolStripButton_Click(object sender, EventArgs e)
		{
			DoDeleteSelectedTags();
		}

		private void tagsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int clickedIndex = tagsListBox.IndexFromPoint(e.Location);
			Tag tag = (Tag)tagsListBox.Items[clickedIndex];
			TagForm tagForm = new TagForm(_DatabaseManager.CreateNewTag);
			tagForm.Data = tag;
			if (tagForm.ShowDialog() == DialogResult.OK)
			{
				_DatabaseManager.SaveTagAsync(tag);
			}
		}

		private void tagsListBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				DoDeleteSelectedTags();
			}
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
			SelectedDatabase = (IDatabaseDescriptor)databaseTypeComboBox.SelectedItem;
			PopulateDatabaseProperties();
			_DatabaseManager.SetDatabase(SelectedDatabase);
		}

		private void databasePropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			_DatabaseManager.SetDatabase(SelectedDatabase);
		}

		#endregion

		#region Hotkeys Tab

		private void hotkeyEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool enabled = hotkeyEnabledCheckBox.Checked;
			_SettingsManager.SettingsObject.HotkeysEnabled = enabled;
			_HotkeyManager.Enabled = enabled;
			OnOptionChanged("HotkeyEnabled", enabled);
			SetHotkeysEnabled(enabled);
		}

		private void hotkeyListItems_HotkeyChanged(object sender, HotkeyListItem.HotkeySetComboEventArgs e)
		{
			// Save hotkey settings
			if (_SettingsManager.SettingsObject.Hotkeys == null) _SettingsManager.SettingsObject.Hotkeys = new Dictionary<string, GlobalHotkeyHook.KeyCombo>();
			_SettingsManager.SettingsObject.Hotkeys[e.ItemID] = e.ItemHook.Key;
			if (_SettingsManager.SettingsObject.HotkeyEnabled == null) _SettingsManager.SettingsObject.HotkeyEnabled = new Dictionary<string, bool>();
			_SettingsManager.SettingsObject.HotkeyEnabled[e.ItemID] = e.ItemHook.Enabled;
			OnOptionChanged("Hotkeys", _SettingsManager.SettingsObject.Hotkeys);
		}

		#endregion

		#endregion

		#region Utility methods

		#region Form global

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

		#endregion

		#region Notes tab

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
			// Populate list view
			notesListView.SuspendLayout();
			notesListView.Items.Clear();
			_iconImageList.Images.Clear();
			_smallImageList.Images.Clear();
			_largeImageList.Images.Clear();

			int index = 0;

			// Display notes
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
				item.Tag = entry.Value;
				item.Text = entry.Value.Title;
				item.ImageIndex = index;
				item.SubItems.Add(entry.Value.Created.ToString());
				item.SubItems.Add(entry.Value.Modified.ToString());
				item.SubItems.Add(entry.Value.Visible ? Resources.Yes : Resources.No);
				string text = entry.Value.Tags.Count > 0 ? string.Join<string>(", ", entry.Value.Tags.Select(t => t.Title)) : Resources.None;
				Font font = new Font(notesListView.Font, entry.Value.Tags.Count > 0 ? FontStyle.Regular : FontStyle.Italic);
				ListViewItem.ListViewSubItem tagsItem = new ListViewItem.ListViewSubItem(item, text, notesListView.ForeColor, notesListView.BackColor, font);
				item.SubItems.Add(tagsItem);
				notesListView.Items.Add(item);

				index++;
			}

			titleColumnHeader.Width = -1;
			createdColumnHeader.Width = -1;
			lastModifiedColumnHeader.Width = -1;
			visibleColumnHeader.Width = -2;
			tagsColumnHeader.Width = -1;

			notesListView.PerformLayout();
		}

		private void DoDeleteSelectedNotes()
		{
			// Delete note(s)
			// Depends on backend, FileDatabase will do a DELETE on the SQLite database
			int count = notesListView.SelectedItems.Count;
			if (count < 1)
			{
				// Must have at least one thing to delete
				return;
			}
			else if (count > 1)
			{
				if (MessageBox.Show(string.Format(Resources.DeleteMessageMulti, count), Resources.AreYouSure, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					foreach (ListViewItem item in notesListView.SelectedItems)
					{
						Note note = (Note)item.Tag;
						_DatabaseManager.DeleteNoteAsync(note);
					}
				}
			}
			else
			{
				Note note = (Note)notesListView.SelectedItems[0].Tag;
				if (MessageBox.Show(string.Format(Resources.DeleteMessage, note.Title), Resources.AreYouSure, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					_DatabaseManager.DeleteNoteAsync(note);
				}
			}
		}

		#endregion

		#region Tags tab

		private void DoDeleteSelectedTags()
		{
			List<Tag> deletingTags = tagsListBox.SelectedItems.Cast<Tag>().ToList();
			int count = deletingTags.Count;
			if (count < 1)
			{
				// Must have at least one thing to delete
				return;
			}
			else if (count > 1)
			{
				if (MessageBox.Show(string.Format(Resources.DeleteTagsMessagePlural, count), Resources.AreYouSure, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					foreach (Tag tag in deletingTags)
					{
						_DatabaseManager.DeleteTagAsync(tag);
					}
				}
			}
			else
			{
				Tag tag = deletingTags[0];
				if (MessageBox.Show(string.Format(Resources.DeleteTagsMessage, tag.Title), Resources.AreYouSure, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				{
					_DatabaseManager.DeleteTagAsync(tag);
				}
			}
		}

		#endregion

		#region Database tab

		private void PopulateDatabaseTypesComboBox()
		{
			_Types = DatabaseManager.GetDatabaseTypes(true);
			if (SelectedDatabase != null)
			{
				_Types[SelectedDatabase.GetType().FullName] = SelectedDatabase;
			}
			databaseTypeComboBox.SelectedIndexChanged -= databaseTypeComboBox_SelectedIndexChanged;
			databaseTypeComboBox.DataSource = _Types.Values.ToList();
			databaseTypeComboBox.DisplayMember = "DisplayName";
			databaseTypeComboBox.SelectedIndexChanged += databaseTypeComboBox_SelectedIndexChanged;
			if (SelectedDatabase != null)
			{
				databaseTypeComboBox.SelectedIndexChanged -= databaseTypeComboBox_SelectedIndexChanged;
				databaseTypeComboBox.SelectedItem = SelectedDatabase;
				databaseTypeComboBox.SelectedIndexChanged += databaseTypeComboBox_SelectedIndexChanged;
				PopulateDatabaseProperties();
			}
		}

		private void PopulateDatabaseProperties()
		{
			databasePropertyGrid.SelectedObject = SelectedDatabase;
			databasePropertyGrid.Visible = DatabaseManager.GetPropertiesForType(SelectedDatabase).Count > 0;
		}

		#endregion

		#region Hotkeys tab

		private void SetHotkeysEnabled(bool enabled)
		{
			hotkeyListLayoutPanel.Enabled = enabled;
		}

		#endregion

		#endregion

	}
}
