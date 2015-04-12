namespace SmallNotes.UI
{
	partial class SmallNotesOptionsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmallNotesOptionsForm));
			this.settingsPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.optionsFormTabControl = new System.Windows.Forms.TabControl();
			this.notesTabPage = new System.Windows.Forms.TabPage();
			this.notesTabProgressBar = new System.Windows.Forms.ProgressBar();
			this.notesListView = new System.Windows.Forms.ListView();
			this.titleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lastModifiedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.createdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.visibleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tagsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._largeImageList = new System.Windows.Forms.ImageList(this.components);
			this._iconImageList = new System.Windows.Forms.ImageList(this.components);
			this.notesToolStrip = new System.Windows.Forms.ToolStrip();
			this.newNoteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.exportToMarkdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToPlaintextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.viewToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tagsTabPage = new System.Windows.Forms.TabPage();
			this.tagsTabProgressBar = new System.Windows.Forms.ProgressBar();
			this.tagsListBox = new SmallNotes.UI.Controls.TagListBox();
			this.tagsPageToolStrip = new System.Windows.Forms.ToolStrip();
			this.newTagToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.deleteTagToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.settingsTabPage = new System.Windows.Forms.TabPage();
			this.databaseTabPage = new System.Windows.Forms.TabPage();
			this.refreshButton = new System.Windows.Forms.Button();
			this.databaseTypeComboBox = new System.Windows.Forms.ComboBox();
			this.databasePropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.noPropertiesLabel = new System.Windows.Forms.Label();
			this._smallImageList = new System.Windows.Forms.ImageList(this.components);
			this.optionsFormTabControl.SuspendLayout();
			this.notesTabPage.SuspendLayout();
			this.notesToolStrip.SuspendLayout();
			this.tagsTabPage.SuspendLayout();
			this.tagsPageToolStrip.SuspendLayout();
			this.settingsTabPage.SuspendLayout();
			this.databaseTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// settingsPropertyGrid
			// 
			resources.ApplyResources(this.settingsPropertyGrid, "settingsPropertyGrid");
			this.settingsPropertyGrid.Name = "settingsPropertyGrid";
			this.settingsPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.settingsPropertyGrid_PropertyValueChanged);
			// 
			// optionsFormTabControl
			// 
			resources.ApplyResources(this.optionsFormTabControl, "optionsFormTabControl");
			this.optionsFormTabControl.Controls.Add(this.notesTabPage);
			this.optionsFormTabControl.Controls.Add(this.tagsTabPage);
			this.optionsFormTabControl.Controls.Add(this.settingsTabPage);
			this.optionsFormTabControl.Controls.Add(this.databaseTabPage);
			this.optionsFormTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.optionsFormTabControl.Multiline = true;
			this.optionsFormTabControl.Name = "optionsFormTabControl";
			this.optionsFormTabControl.SelectedIndex = 0;
			this.optionsFormTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.optionsFormTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.optionsFormTabControl_DrawItem);
			// 
			// notesTabPage
			// 
			this.notesTabPage.BackColor = System.Drawing.SystemColors.Window;
			this.notesTabPage.Controls.Add(this.notesTabProgressBar);
			this.notesTabPage.Controls.Add(this.notesListView);
			this.notesTabPage.Controls.Add(this.notesToolStrip);
			resources.ApplyResources(this.notesTabPage, "notesTabPage");
			this.notesTabPage.Name = "notesTabPage";
			// 
			// notesTabProgressBar
			// 
			resources.ApplyResources(this.notesTabProgressBar, "notesTabProgressBar");
			this.notesTabProgressBar.Name = "notesTabProgressBar";
			this.notesTabProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// notesListView
			// 
			this.notesListView.AllowColumnReorder = true;
			resources.ApplyResources(this.notesListView, "notesListView");
			this.notesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.notesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumnHeader,
            this.lastModifiedColumnHeader,
            this.createdColumnHeader,
            this.visibleColumnHeader,
            this.tagsColumnHeader});
			this.notesListView.LargeImageList = this._largeImageList;
			this.notesListView.Name = "notesListView";
			this.notesListView.SmallImageList = this._iconImageList;
			this.notesListView.UseCompatibleStateImageBehavior = false;
			this.notesListView.ItemActivate += new System.EventHandler(this.notesListView_ItemActivate);
			this.notesListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.notesListView_KeyUp);
			// 
			// titleColumnHeader
			// 
			resources.ApplyResources(this.titleColumnHeader, "titleColumnHeader");
			// 
			// lastModifiedColumnHeader
			// 
			resources.ApplyResources(this.lastModifiedColumnHeader, "lastModifiedColumnHeader");
			// 
			// createdColumnHeader
			// 
			resources.ApplyResources(this.createdColumnHeader, "createdColumnHeader");
			// 
			// visibleColumnHeader
			// 
			resources.ApplyResources(this.visibleColumnHeader, "visibleColumnHeader");
			// 
			// tagsColumnHeader
			// 
			resources.ApplyResources(this.tagsColumnHeader, "tagsColumnHeader");
			// 
			// _largeImageList
			// 
			this._largeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			resources.ApplyResources(this._largeImageList, "_largeImageList");
			this._largeImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _iconImageList
			// 
			this._iconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this._iconImageList, "_iconImageList");
			this._iconImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// notesToolStrip
			// 
			this.notesToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.notesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newNoteToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.copyToolStripButton,
            this.deleteToolStripButton,
            this.toolStripSeparator4,
            this.viewToolStripDropDownButton});
			resources.ApplyResources(this.notesToolStrip, "notesToolStrip");
			this.notesToolStrip.Name = "notesToolStrip";
			// 
			// newNoteToolStripButton
			// 
			this.newNoteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newNoteToolStripButton.Image = global::SmallNotes.Properties.Resources.note_add;
			resources.ApplyResources(this.newNoteToolStripButton, "newNoteToolStripButton");
			this.newNoteToolStripButton.Name = "newNoteToolStripButton";
			this.newNoteToolStripButton.Click += new System.EventHandler(this.newNoteToolStripButton_Click);
			// 
			// saveToolStripButton
			// 
			this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToMarkdownToolStripMenuItem,
            this.exportToHTMLToolStripMenuItem,
            this.exportToPlaintextToolStripMenuItem});
			this.saveToolStripButton.Image = global::SmallNotes.Properties.Resources.disk;
			resources.ApplyResources(this.saveToolStripButton, "saveToolStripButton");
			this.saveToolStripButton.Name = "saveToolStripButton";
			// 
			// exportToMarkdownToolStripMenuItem
			// 
			this.exportToMarkdownToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.page_code;
			this.exportToMarkdownToolStripMenuItem.Name = "exportToMarkdownToolStripMenuItem";
			resources.ApplyResources(this.exportToMarkdownToolStripMenuItem, "exportToMarkdownToolStripMenuItem");
			this.exportToMarkdownToolStripMenuItem.Click += new System.EventHandler(this.exportToMarkdownToolStripMenuItem_Click);
			// 
			// exportToHTMLToolStripMenuItem
			// 
			this.exportToHTMLToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.html;
			this.exportToHTMLToolStripMenuItem.Name = "exportToHTMLToolStripMenuItem";
			resources.ApplyResources(this.exportToHTMLToolStripMenuItem, "exportToHTMLToolStripMenuItem");
			this.exportToHTMLToolStripMenuItem.Click += new System.EventHandler(this.exportToHTMLToolStripMenuItem_Click);
			// 
			// exportToPlaintextToolStripMenuItem
			// 
			this.exportToPlaintextToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.page_white_text;
			this.exportToPlaintextToolStripMenuItem.Name = "exportToPlaintextToolStripMenuItem";
			resources.ApplyResources(this.exportToPlaintextToolStripMenuItem, "exportToPlaintextToolStripMenuItem");
			this.exportToPlaintextToolStripMenuItem.Click += new System.EventHandler(this.exportToPlaintextToolStripMenuItem_Click);
			// 
			// printToolStripButton
			// 
			this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.printToolStripButton.Image = global::SmallNotes.Properties.Resources.printer;
			resources.ApplyResources(this.printToolStripButton, "printToolStripButton");
			this.printToolStripButton.Name = "printToolStripButton";
			this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.copyToolStripButton.Image = global::SmallNotes.Properties.Resources.page_white_copy;
			resources.ApplyResources(this.copyToolStripButton, "copyToolStripButton");
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
			// 
			// deleteToolStripButton
			// 
			this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.deleteToolStripButton.Image = global::SmallNotes.Properties.Resources.note_delete;
			resources.ApplyResources(this.deleteToolStripButton, "deleteToolStripButton");
			this.deleteToolStripButton.Name = "deleteToolStripButton";
			this.deleteToolStripButton.Click += new System.EventHandler(this.deleteToolStripButton_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// viewToolStripDropDownButton
			// 
			this.viewToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.viewToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem,
            this.listToolStripMenuItem,
            this.smallIconsToolStripMenuItem,
            this.largeIconsToolStripMenuItem,
            this.tilesToolStripMenuItem});
			this.viewToolStripDropDownButton.Image = global::SmallNotes.Properties.Resources.application_view_icons;
			resources.ApplyResources(this.viewToolStripDropDownButton, "viewToolStripDropDownButton");
			this.viewToolStripDropDownButton.Name = "viewToolStripDropDownButton";
			// 
			// detailsToolStripMenuItem
			// 
			this.detailsToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.application_view_detail;
			this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
			resources.ApplyResources(this.detailsToolStripMenuItem, "detailsToolStripMenuItem");
			this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
			// 
			// listToolStripMenuItem
			// 
			this.listToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.application_view_list;
			this.listToolStripMenuItem.Name = "listToolStripMenuItem";
			resources.ApplyResources(this.listToolStripMenuItem, "listToolStripMenuItem");
			this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
			// 
			// smallIconsToolStripMenuItem
			// 
			this.smallIconsToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.application_view_columns;
			this.smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
			resources.ApplyResources(this.smallIconsToolStripMenuItem, "smallIconsToolStripMenuItem");
			this.smallIconsToolStripMenuItem.Click += new System.EventHandler(this.smallIconsToolStripMenuItem_Click);
			// 
			// largeIconsToolStripMenuItem
			// 
			this.largeIconsToolStripMenuItem.Checked = true;
			this.largeIconsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.largeIconsToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.application_view_icons;
			this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
			resources.ApplyResources(this.largeIconsToolStripMenuItem, "largeIconsToolStripMenuItem");
			this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.largeIconsToolStripMenuItem_Click);
			// 
			// tilesToolStripMenuItem
			// 
			this.tilesToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.application_view_tile;
			this.tilesToolStripMenuItem.Name = "tilesToolStripMenuItem";
			resources.ApplyResources(this.tilesToolStripMenuItem, "tilesToolStripMenuItem");
			this.tilesToolStripMenuItem.Click += new System.EventHandler(this.tilesToolStripMenuItem_Click);
			// 
			// tagsTabPage
			// 
			this.tagsTabPage.Controls.Add(this.tagsTabProgressBar);
			this.tagsTabPage.Controls.Add(this.tagsListBox);
			this.tagsTabPage.Controls.Add(this.tagsPageToolStrip);
			resources.ApplyResources(this.tagsTabPage, "tagsTabPage");
			this.tagsTabPage.Name = "tagsTabPage";
			this.tagsTabPage.UseVisualStyleBackColor = true;
			// 
			// tagsTabProgressBar
			// 
			resources.ApplyResources(this.tagsTabProgressBar, "tagsTabProgressBar");
			this.tagsTabProgressBar.Name = "tagsTabProgressBar";
			this.tagsTabProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// tagsListBox
			// 
			resources.ApplyResources(this.tagsListBox, "tagsListBox");
			this.tagsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tagsListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.tagsListBox.DrawNoteCount = true;
			this.tagsListBox.Name = "tagsListBox";
			this.tagsListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tagsListBox_KeyUp);
			this.tagsListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tagsListBox_MouseDoubleClick);
			// 
			// tagsPageToolStrip
			// 
			this.tagsPageToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tagsPageToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTagToolStripButton,
            this.deleteTagToolStripButton});
			resources.ApplyResources(this.tagsPageToolStrip, "tagsPageToolStrip");
			this.tagsPageToolStrip.Name = "tagsPageToolStrip";
			// 
			// newTagToolStripButton
			// 
			this.newTagToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newTagToolStripButton.Image = global::SmallNotes.Properties.Resources.tag_blue_add;
			resources.ApplyResources(this.newTagToolStripButton, "newTagToolStripButton");
			this.newTagToolStripButton.Name = "newTagToolStripButton";
			this.newTagToolStripButton.Click += new System.EventHandler(this.newTagToolStripButton_Click);
			// 
			// deleteTagToolStripButton
			// 
			this.deleteTagToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.deleteTagToolStripButton.Image = global::SmallNotes.Properties.Resources.tag_blue_delete;
			resources.ApplyResources(this.deleteTagToolStripButton, "deleteTagToolStripButton");
			this.deleteTagToolStripButton.Name = "deleteTagToolStripButton";
			this.deleteTagToolStripButton.Click += new System.EventHandler(this.deleteTagToolStripButton_Click);
			// 
			// settingsTabPage
			// 
			this.settingsTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.settingsTabPage.Controls.Add(this.settingsPropertyGrid);
			resources.ApplyResources(this.settingsTabPage, "settingsTabPage");
			this.settingsTabPage.Name = "settingsTabPage";
			// 
			// databaseTabPage
			// 
			this.databaseTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.databaseTabPage.Controls.Add(this.refreshButton);
			this.databaseTabPage.Controls.Add(this.databaseTypeComboBox);
			this.databaseTabPage.Controls.Add(this.databasePropertyGrid);
			this.databaseTabPage.Controls.Add(this.noPropertiesLabel);
			resources.ApplyResources(this.databaseTabPage, "databaseTabPage");
			this.databaseTabPage.Name = "databaseTabPage";
			// 
			// refreshButton
			// 
			resources.ApplyResources(this.refreshButton, "refreshButton");
			this.refreshButton.Image = global::SmallNotes.Properties.Resources.arrow_refresh;
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// databaseTypeComboBox
			// 
			resources.ApplyResources(this.databaseTypeComboBox, "databaseTypeComboBox");
			this.databaseTypeComboBox.FormattingEnabled = true;
			this.databaseTypeComboBox.Name = "databaseTypeComboBox";
			this.databaseTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.databaseTypeComboBox_SelectedIndexChanged);
			// 
			// databasePropertyGrid
			// 
			resources.ApplyResources(this.databasePropertyGrid, "databasePropertyGrid");
			this.databasePropertyGrid.Name = "databasePropertyGrid";
			this.databasePropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.databasePropertyGrid_PropertyValueChanged);
			// 
			// noPropertiesLabel
			// 
			resources.ApplyResources(this.noPropertiesLabel, "noPropertiesLabel");
			this.noPropertiesLabel.Name = "noPropertiesLabel";
			// 
			// _smallImageList
			// 
			this._smallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this._smallImageList, "_smallImageList");
			this._smallImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// SmallNotesOptionsForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.optionsFormTabControl);
			this.Name = "SmallNotesOptionsForm";
			this.optionsFormTabControl.ResumeLayout(false);
			this.notesTabPage.ResumeLayout(false);
			this.notesTabPage.PerformLayout();
			this.notesToolStrip.ResumeLayout(false);
			this.notesToolStrip.PerformLayout();
			this.tagsTabPage.ResumeLayout(false);
			this.tagsTabPage.PerformLayout();
			this.tagsPageToolStrip.ResumeLayout(false);
			this.tagsPageToolStrip.PerformLayout();
			this.settingsTabPage.ResumeLayout(false);
			this.databaseTabPage.ResumeLayout(false);
			this.databaseTabPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid settingsPropertyGrid;
		private System.Windows.Forms.TabControl optionsFormTabControl;
		private System.Windows.Forms.TabPage notesTabPage;
		private System.Windows.Forms.TabPage settingsTabPage;
		private System.Windows.Forms.ToolStrip notesToolStrip;
		private System.Windows.Forms.ToolStripDropDownButton viewToolStripDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton saveToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem exportToMarkdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToHTMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToPlaintextToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton printToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
		private System.Windows.Forms.ListView notesListView;
		private System.Windows.Forms.ColumnHeader titleColumnHeader;
		private System.Windows.Forms.ColumnHeader lastModifiedColumnHeader;
		private System.Windows.Forms.ColumnHeader createdColumnHeader;
		private System.Windows.Forms.ImageList _largeImageList;
		private System.Windows.Forms.ImageList _iconImageList;
		private System.Windows.Forms.ImageList _smallImageList;
		private System.Windows.Forms.ToolStripMenuItem tilesToolStripMenuItem;
		private System.Windows.Forms.TabPage databaseTabPage;
		private System.Windows.Forms.PropertyGrid databasePropertyGrid;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.ComboBox databaseTypeComboBox;
		private System.Windows.Forms.ProgressBar notesTabProgressBar;
		private System.Windows.Forms.ToolStripButton newNoteToolStripButton;
		private System.Windows.Forms.ColumnHeader visibleColumnHeader;
		private System.Windows.Forms.ColumnHeader tagsColumnHeader;
		private System.Windows.Forms.TabPage tagsTabPage;
		private System.Windows.Forms.ToolStrip tagsPageToolStrip;
		private System.Windows.Forms.ToolStripButton newTagToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteTagToolStripButton;
		private SmallNotes.UI.Controls.TagListBox tagsListBox;
		private System.Windows.Forms.ProgressBar tagsTabProgressBar;
		private System.Windows.Forms.Label noPropertiesLabel;
	}
}

