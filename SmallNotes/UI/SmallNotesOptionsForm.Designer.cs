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
			this.notesListView = new System.Windows.Forms.ListView();
			this.titleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lastModifiedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.createdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._largeImageList = new System.Windows.Forms.ImageList(this.components);
			this._iconImageList = new System.Windows.Forms.ImageList(this.components);
			this.notesToolStrip = new System.Windows.Forms.ToolStrip();
			this.newToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.noteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.exportToMarkdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToPlaintextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.viewToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsTabPage = new System.Windows.Forms.TabPage();
			this.databaseTabPage = new System.Windows.Forms.TabPage();
			this.refreshButton = new System.Windows.Forms.Button();
			this.databaseTypeComboBox = new System.Windows.Forms.ComboBox();
			this.databasePropertyGrid = new System.Windows.Forms.PropertyGrid();
			this._smallImageList = new System.Windows.Forms.ImageList(this.components);
			this.notesTabProgressBar = new System.Windows.Forms.ProgressBar();
			this.optionsFormTabControl.SuspendLayout();
			this.notesTabPage.SuspendLayout();
			this.notesToolStrip.SuspendLayout();
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
			// notesListView
			// 
			this.notesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.notesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumnHeader,
            this.lastModifiedColumnHeader,
            this.createdColumnHeader});
			this.notesListView.LargeImageList = this._largeImageList;
			resources.ApplyResources(this.notesListView, "notesListView");
			this.notesListView.Name = "notesListView";
			this.notesListView.SmallImageList = this._iconImageList;
			this.notesListView.UseCompatibleStateImageBehavior = false;
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
			this.notesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton1,
            this.pasteToolStripButton,
            this.toolStripSeparator1,
            this.deleteToolStripButton,
            this.toolStripSeparator4,
            this.viewToolStripDropDownButton});
			resources.ApplyResources(this.notesToolStrip, "notesToolStrip");
			this.notesToolStrip.Name = "notesToolStrip";
			// 
			// newToolStripButton
			// 
			this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.folderToolStripMenuItem,
            this.toolStripSeparator2,
            this.noteToolStripMenuItem});
			this.newToolStripButton.Image = global::SmallNotes.Properties.Resources.page_white;
			resources.ApplyResources(this.newToolStripButton, "newToolStripButton");
			this.newToolStripButton.Name = "newToolStripButton";
			// 
			// folderToolStripMenuItem
			// 
			this.folderToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.folder_add;
			this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
			resources.ApplyResources(this.folderToolStripMenuItem, "folderToolStripMenuItem");
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// noteToolStripMenuItem
			// 
			this.noteToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.note_add;
			this.noteToolStripMenuItem.Name = "noteToolStripMenuItem";
			resources.ApplyResources(this.noteToolStripMenuItem, "noteToolStripMenuItem");
			this.noteToolStripMenuItem.Click += new System.EventHandler(this.noteToolStripMenuItem_Click);
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
			// 
			// exportToHTMLToolStripMenuItem
			// 
			this.exportToHTMLToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.html;
			this.exportToHTMLToolStripMenuItem.Name = "exportToHTMLToolStripMenuItem";
			resources.ApplyResources(this.exportToHTMLToolStripMenuItem, "exportToHTMLToolStripMenuItem");
			// 
			// exportToPlaintextToolStripMenuItem
			// 
			this.exportToPlaintextToolStripMenuItem.Image = global::SmallNotes.Properties.Resources.page_white_text;
			this.exportToPlaintextToolStripMenuItem.Name = "exportToPlaintextToolStripMenuItem";
			resources.ApplyResources(this.exportToPlaintextToolStripMenuItem, "exportToPlaintextToolStripMenuItem");
			// 
			// printToolStripButton
			// 
			this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.printToolStripButton.Image = global::SmallNotes.Properties.Resources.printer;
			resources.ApplyResources(this.printToolStripButton, "printToolStripButton");
			this.printToolStripButton.Name = "printToolStripButton";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
			// 
			// cutToolStripButton
			// 
			this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cutToolStripButton.Image = global::SmallNotes.Properties.Resources.cut;
			resources.ApplyResources(this.cutToolStripButton, "cutToolStripButton");
			this.cutToolStripButton.Name = "cutToolStripButton";
			// 
			// copyToolStripButton1
			// 
			this.copyToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.copyToolStripButton1.Image = global::SmallNotes.Properties.Resources.page_white_copy;
			resources.ApplyResources(this.copyToolStripButton1, "copyToolStripButton1");
			this.copyToolStripButton1.Name = "copyToolStripButton1";
			// 
			// pasteToolStripButton
			// 
			this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pasteToolStripButton.Image = global::SmallNotes.Properties.Resources.paste_plain;
			resources.ApplyResources(this.pasteToolStripButton, "pasteToolStripButton");
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// deleteToolStripButton
			// 
			this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.deleteToolStripButton.Image = global::SmallNotes.Properties.Resources.delete;
			resources.ApplyResources(this.deleteToolStripButton, "deleteToolStripButton");
			this.deleteToolStripButton.Name = "deleteToolStripButton";
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
			// _smallImageList
			// 
			this._smallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this._smallImageList, "_smallImageList");
			this._smallImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// notesTabProgressBar
			// 
			resources.ApplyResources(this.notesTabProgressBar, "notesTabProgressBar");
			this.notesTabProgressBar.Name = "notesTabProgressBar";
			this.notesTabProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
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
			this.settingsTabPage.ResumeLayout(false);
			this.databaseTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PropertyGrid settingsPropertyGrid;
		private System.Windows.Forms.TabControl optionsFormTabControl;
		private System.Windows.Forms.TabPage notesTabPage;
		private System.Windows.Forms.TabPage settingsTabPage;
		private System.Windows.Forms.ToolStrip notesToolStrip;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton viewToolStripDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton saveToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem exportToMarkdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToHTMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToPlaintextToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton printToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton cutToolStripButton;
		private System.Windows.Forms.ToolStripButton copyToolStripButton1;
		private System.Windows.Forms.ToolStripButton pasteToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripDropDownButton newToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem noteToolStripMenuItem;
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
	}
}

