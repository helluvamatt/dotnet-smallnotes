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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.richTextTabPage = new System.Windows.Forms.TabPage();
			this.markdownTabPage = new System.Windows.Forms.TabPage();
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.plainTextTabPage = new System.Windows.Forms.TabPage();
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.markdownTextBox = new System.Windows.Forms.TextBox();
			this.plainTextBox = new System.Windows.Forms.TextBox();
			this.tabControl.SuspendLayout();
			this.richTextTabPage.SuspendLayout();
			this.markdownTabPage.SuspendLayout();
			this.plainTextTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.richTextTabPage);
			this.tabControl.Controls.Add(this.markdownTabPage);
			this.tabControl.Controls.Add(this.plainTextTabPage);
			resources.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			// 
			// richTextTabPage
			// 
			this.richTextTabPage.Controls.Add(this.richTextBox);
			resources.ApplyResources(this.richTextTabPage, "richTextTabPage");
			this.richTextTabPage.Name = "richTextTabPage";
			this.richTextTabPage.UseVisualStyleBackColor = true;
			// 
			// markdownTabPage
			// 
			this.markdownTabPage.Controls.Add(this.markdownTextBox);
			resources.ApplyResources(this.markdownTabPage, "markdownTabPage");
			this.markdownTabPage.Name = "markdownTabPage";
			this.markdownTabPage.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// saveButton
			// 
			resources.ApplyResources(this.saveButton, "saveButton");
			this.saveButton.Name = "saveButton";
			this.saveButton.UseVisualStyleBackColor = true;
			// 
			// plainTextTabPage
			// 
			this.plainTextTabPage.Controls.Add(this.plainTextBox);
			resources.ApplyResources(this.plainTextTabPage, "plainTextTabPage");
			this.plainTextTabPage.Name = "plainTextTabPage";
			this.plainTextTabPage.UseVisualStyleBackColor = true;
			// 
			// richTextBox
			// 
			this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.richTextBox, "richTextBox");
			this.richTextBox.Name = "richTextBox";
			// 
			// markdownTextBox
			// 
			this.markdownTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.markdownTextBox, "markdownTextBox");
			this.markdownTextBox.Name = "markdownTextBox";
			// 
			// plainTextBox
			// 
			this.plainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.plainTextBox, "plainTextBox");
			this.plainTextBox.Name = "plainTextBox";
			// 
			// NoteForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "NoteForm";
			this.tabControl.ResumeLayout(false);
			this.richTextTabPage.ResumeLayout(false);
			this.markdownTabPage.ResumeLayout(false);
			this.markdownTabPage.PerformLayout();
			this.plainTextTabPage.ResumeLayout(false);
			this.plainTextTabPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage richTextTabPage;
		private System.Windows.Forms.TabPage markdownTabPage;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TabPage plainTextTabPage;
		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.TextBox markdownTextBox;
		private System.Windows.Forms.TextBox plainTextBox;
	}
}