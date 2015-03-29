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
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.markdownTextBox = new System.Windows.Forms.TextBox();
			this.displayBrowser = new System.Windows.Forms.WebBrowser();
			this.editorPanel.SuspendLayout();
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
			this.editorPanel.Controls.Add(this.titleTextBox);
			this.editorPanel.Controls.Add(this.markdownTextBox);
			this.editorPanel.Controls.Add(this.saveButton);
			this.editorPanel.Controls.Add(this.cancelButton);
			resources.ApplyResources(this.editorPanel, "editorPanel");
			this.editorPanel.Name = "editorPanel";
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
			// displayBrowser
			// 
			resources.ApplyResources(this.displayBrowser, "displayBrowser");
			this.displayBrowser.IsWebBrowserContextMenuEnabled = false;
			this.displayBrowser.Name = "displayBrowser";
			this.displayBrowser.ScriptErrorsSuppressed = true;
			this.displayBrowser.ScrollBarsEnabled = false;
			// 
			// NoteForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.displayBrowser);
			this.Controls.Add(this.editorPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "NoteForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.DoubleClick += new System.EventHandler(this.NoteForm_DoubleClick);
			this.editorPanel.ResumeLayout(false);
			this.editorPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Panel editorPanel;
		private System.Windows.Forms.TextBox markdownTextBox;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.WebBrowser displayBrowser;
	}
}