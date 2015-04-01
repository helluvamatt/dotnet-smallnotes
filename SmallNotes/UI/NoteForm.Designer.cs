namespace SmallNotes.UI
{
	partial class NoteForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteForm));
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.editorPanel = new System.Windows.Forms.Panel();
			this.optionsToolStrip = new System.Windows.Forms.ToolStrip();
			this.backgroundColorDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.foregroundColorDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.markdownTextBox = new System.Windows.Forms.TextBox();
			this.colorPickerDialog = new System.Windows.Forms.ColorDialog();
			this.displayPanel = new SmallNotes.UI.ResizePanel();
			this.titleDisplayLabel = new SmallNotes.UI.NoMouseLabel();
			this.displayBrowser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
			this.automaticForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.blackForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.whiteForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.customForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editorPanel.SuspendLayout();
			this.optionsToolStrip.SuspendLayout();
			this.displayPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// saveButton
			// 
			resources.ApplyResources(this.saveButton, "saveButton");
			this.saveButton.Name = "saveButton";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// editorPanel
			// 
			this.editorPanel.BackColor = System.Drawing.Color.Transparent;
			this.editorPanel.Controls.Add(this.optionsToolStrip);
			this.editorPanel.Controls.Add(this.titleTextBox);
			this.editorPanel.Controls.Add(this.markdownTextBox);
			this.editorPanel.Controls.Add(this.saveButton);
			this.editorPanel.Controls.Add(this.cancelButton);
			resources.ApplyResources(this.editorPanel, "editorPanel");
			this.editorPanel.Name = "editorPanel";
			// 
			// optionsToolStrip
			// 
			this.optionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundColorDropDownButton,
            this.foregroundColorDropDownButton});
			resources.ApplyResources(this.optionsToolStrip, "optionsToolStrip");
			this.optionsToolStrip.Name = "optionsToolStrip";
			// 
			// backgroundColorDropDownButton
			// 
			this.backgroundColorDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.backgroundColorDropDownButton, "backgroundColorDropDownButton");
			this.backgroundColorDropDownButton.Name = "backgroundColorDropDownButton";
			// 
			// foregroundColorDropDownButton
			// 
			this.foregroundColorDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.foregroundColorDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automaticForegroundColorToolStripMenuItem,
            this.toolStripSeparator1,
            this.blackForegroundColorToolStripMenuItem,
            this.whiteForegroundColorToolStripMenuItem,
            this.toolStripSeparator2,
            this.customForegroundColorToolStripMenuItem});
			resources.ApplyResources(this.foregroundColorDropDownButton, "foregroundColorDropDownButton");
			this.foregroundColorDropDownButton.Name = "foregroundColorDropDownButton";
			// 
			// titleTextBox
			// 
			resources.ApplyResources(this.titleTextBox, "titleTextBox");
			this.titleTextBox.Name = "titleTextBox";
			// 
			// markdownTextBox
			// 
			resources.ApplyResources(this.markdownTextBox, "markdownTextBox");
			this.markdownTextBox.Name = "markdownTextBox";
			// 
			// displayPanel
			// 
			this.displayPanel.BackColor = System.Drawing.Color.Transparent;
			this.displayPanel.Controls.Add(this.titleDisplayLabel);
			this.displayPanel.Controls.Add(this.displayBrowser);
			resources.ApplyResources(this.displayPanel, "displayPanel");
			this.displayPanel.Name = "displayPanel";
			this.displayPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseDoubleClick);
			this.displayPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseDown);
			this.displayPanel.MouseEnter += new System.EventHandler(this.displayPanel_MouseEnter);
			this.displayPanel.MouseLeave += new System.EventHandler(this.displayPanel_MouseLeave);
			this.displayPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseMove);
			this.displayPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.displayPanel_MouseUp);
			// 
			// titleDisplayLabel
			// 
			resources.ApplyResources(this.titleDisplayLabel, "titleDisplayLabel");
			this.titleDisplayLabel.Name = "titleDisplayLabel";
			// 
			// displayBrowser
			// 
			resources.ApplyResources(this.displayBrowser, "displayBrowser");
			this.displayBrowser.BackColor = System.Drawing.SystemColors.Window;
			this.displayBrowser.BaseStylesheet = null;
			this.displayBrowser.Name = "displayBrowser";
			this.displayBrowser.LinkClicked += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlLinkClickedEventArgs>(this.displayBrowser_LinkClicked);
			this.displayBrowser.ImageLoad += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlImageLoadEventArgs>(this.displayBrowser_ImageLoad);
			this.displayBrowser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDoubleClick);
			this.displayBrowser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDown);
			this.displayBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseMove);
			this.displayBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseUp);
			// 
			// automaticForegroundColorToolStripMenuItem
			// 
			this.automaticForegroundColorToolStripMenuItem.Name = "automaticForegroundColorToolStripMenuItem";
			resources.ApplyResources(this.automaticForegroundColorToolStripMenuItem, "automaticForegroundColorToolStripMenuItem");
			this.automaticForegroundColorToolStripMenuItem.Click += new System.EventHandler(this.automaticForegroundColorToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// blackForegroundColorToolStripMenuItem
			// 
			this.blackForegroundColorToolStripMenuItem.Name = "blackForegroundColorToolStripMenuItem";
			resources.ApplyResources(this.blackForegroundColorToolStripMenuItem, "blackForegroundColorToolStripMenuItem");
			this.blackForegroundColorToolStripMenuItem.Click += new System.EventHandler(this.blackForegroundColorToolStripMenuItem_Click);
			// 
			// whiteForegroundColorToolStripMenuItem
			// 
			this.whiteForegroundColorToolStripMenuItem.Name = "whiteForegroundColorToolStripMenuItem";
			resources.ApplyResources(this.whiteForegroundColorToolStripMenuItem, "whiteForegroundColorToolStripMenuItem");
			this.whiteForegroundColorToolStripMenuItem.Click += new System.EventHandler(this.whiteForegroundColorToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// customForegroundColorToolStripMenuItem
			// 
			this.customForegroundColorToolStripMenuItem.Name = "customForegroundColorToolStripMenuItem";
			resources.ApplyResources(this.customForegroundColorToolStripMenuItem, "customForegroundColorToolStripMenuItem");
			this.customForegroundColorToolStripMenuItem.Click += new System.EventHandler(this.customForegroundColorToolStripMenuItem_Click);
			// 
			// NoteForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.editorPanel);
			this.Controls.Add(this.displayPanel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "NoteForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoteForm_FormClosing);
			this.BackColorChanged += new System.EventHandler(this.NoteForm_BackColorChanged);
			this.ForeColorChanged += new System.EventHandler(this.NoteForm_ForeColorChanged);
			this.editorPanel.ResumeLayout(false);
			this.editorPanel.PerformLayout();
			this.optionsToolStrip.ResumeLayout(false);
			this.optionsToolStrip.PerformLayout();
			this.displayPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Panel editorPanel;
		private System.Windows.Forms.TextBox markdownTextBox;
		private System.Windows.Forms.TextBox titleTextBox;
		private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel displayBrowser;
		private System.Windows.Forms.ColorDialog colorPickerDialog;
		private System.Windows.Forms.ToolStrip optionsToolStrip;
		private System.Windows.Forms.ToolStripDropDownButton backgroundColorDropDownButton;
		private ResizePanel displayPanel;
		private NoMouseLabel titleDisplayLabel;
		private System.Windows.Forms.ToolStripDropDownButton foregroundColorDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem automaticForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem blackForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem whiteForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem customForegroundColorToolStripMenuItem;
	}
}