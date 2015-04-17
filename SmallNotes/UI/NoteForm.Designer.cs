using SmallNotes.UI.Controls;
using SmallNotes.UI.Utils;
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
			this.automaticForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.blackForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.whiteForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.customForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tagsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.markdownTextBox = new System.Windows.Forms.TextBox();
			this.colorPickerDialog = new System.Windows.Forms.ColorDialog();
			this.displayPanel = new System.Windows.Forms.Panel();
			this.displayScrollPanel = new System.Windows.Forms.Panel();
			this.displayBrowser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel();
			this.titleDisplayLabel = new System.Windows.Forms.Label();
			this.editorPanel.SuspendLayout();
			this.optionsToolStrip.SuspendLayout();
			this.displayPanel.SuspendLayout();
			this.displayScrollPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Image = global::SmallNotes.Properties.Resources.ic_content_clear;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// saveButton
			// 
			resources.ApplyResources(this.saveButton, "saveButton");
			this.saveButton.Image = global::SmallNotes.Properties.Resources.ic_action_done;
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
			this.optionsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.optionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundColorDropDownButton,
            this.foregroundColorDropDownButton,
            this.tagsToolStripButton});
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
			// tagsToolStripButton
			// 
			this.tagsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tagsToolStripButton.Image = global::SmallNotes.Properties.Resources.tag_blue;
			resources.ApplyResources(this.tagsToolStripButton, "tagsToolStripButton");
			this.tagsToolStripButton.Name = "tagsToolStripButton";
			this.tagsToolStripButton.Click += new System.EventHandler(this.tagsToolStripButton_Click);
			// 
			// titleTextBox
			// 
			resources.ApplyResources(this.titleTextBox, "titleTextBox");
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.titleTextBox_KeyPress);
			// 
			// markdownTextBox
			// 
			resources.ApplyResources(this.markdownTextBox, "markdownTextBox");
			this.markdownTextBox.Name = "markdownTextBox";
			// 
			// displayPanel
			// 
			this.displayPanel.BackColor = System.Drawing.Color.Transparent;
			this.displayPanel.Controls.Add(this.displayScrollPanel);
			this.displayPanel.Controls.Add(this.titleDisplayLabel);
			resources.ApplyResources(this.displayPanel, "displayPanel");
			this.displayPanel.Name = "displayPanel";
			// 
			// displayScrollPanel
			// 
			resources.ApplyResources(this.displayScrollPanel, "displayScrollPanel");
			this.displayScrollPanel.Controls.Add(this.displayBrowser);
			this.displayScrollPanel.Name = "displayScrollPanel";
			this.displayScrollPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDoubleClick);
			this.displayScrollPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDown);
			this.displayScrollPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseMove);
			this.displayScrollPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseUp);
			// 
			// displayBrowser
			// 
			this.displayBrowser.BackColor = System.Drawing.SystemColors.Window;
			this.displayBrowser.BaseStylesheet = null;
			resources.ApplyResources(this.displayBrowser, "displayBrowser");
			this.displayBrowser.Name = "displayBrowser";
			this.displayBrowser.LinkClicked += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlLinkClickedEventArgs>(this.displayBrowser_LinkClicked);
			this.displayBrowser.ImageLoad += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlImageLoadEventArgs>(this.displayBrowser_ImageLoad);
			this.displayBrowser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDoubleClick);
			this.displayBrowser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseDown);
			this.displayBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseMove);
			this.displayBrowser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.displayBrowser_MouseUp);
			// 
			// titleDisplayLabel
			// 
			resources.ApplyResources(this.titleDisplayLabel, "titleDisplayLabel");
			this.titleDisplayLabel.Name = "titleDisplayLabel";
			this.titleDisplayLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.titleDisplayLabel_Paint);
			this.titleDisplayLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.titleDisplayLabel_MouseDoubleClick);
			this.titleDisplayLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleDisplayLabel_MouseDown);
			this.titleDisplayLabel.MouseEnter += new System.EventHandler(this.titleDisplayLabel_MouseEnter);
			this.titleDisplayLabel.MouseLeave += new System.EventHandler(this.titleDisplayLabel_MouseLeave);
			this.titleDisplayLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleDisplayLabel_MouseMove);
			this.titleDisplayLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleDisplayLabel_MouseUp);
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
			this.Activated += new System.EventHandler(this.NoteForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoteForm_FormClosing);
			this.BackColorChanged += new System.EventHandler(this.NoteForm_BackColorChanged);
			this.ForeColorChanged += new System.EventHandler(this.NoteForm_ForeColorChanged);
			this.editorPanel.ResumeLayout(false);
			this.editorPanel.PerformLayout();
			this.optionsToolStrip.ResumeLayout(false);
			this.optionsToolStrip.PerformLayout();
			this.displayPanel.ResumeLayout(false);
			this.displayScrollPanel.ResumeLayout(false);
			this.displayScrollPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Panel editorPanel;
		private System.Windows.Forms.TextBox markdownTextBox;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.ColorDialog colorPickerDialog;
		private System.Windows.Forms.ToolStrip optionsToolStrip;
		private System.Windows.Forms.ToolStripDropDownButton backgroundColorDropDownButton;
		private System.Windows.Forms.ToolStripDropDownButton foregroundColorDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem automaticForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem blackForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem whiteForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem customForegroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton tagsToolStripButton;
		private System.Windows.Forms.Panel displayPanel;
		private TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel displayBrowser;
		private System.Windows.Forms.Label titleDisplayLabel;
		private System.Windows.Forms.Panel displayScrollPanel;
	}
}