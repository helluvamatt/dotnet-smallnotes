namespace SmallNotes.UI
{
	partial class TagForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagForm));
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.titleLabel = new System.Windows.Forms.Label();
			this.colorLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.colorButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// titleTextBox
			// 
			resources.ApplyResources(this.titleTextBox, "titleTextBox");
			this.titleTextBox.Name = "titleTextBox";
			// 
			// titleLabel
			// 
			resources.ApplyResources(this.titleLabel, "titleLabel");
			this.titleLabel.Name = "titleLabel";
			// 
			// colorLabel
			// 
			resources.ApplyResources(this.colorLabel, "colorLabel");
			this.colorLabel.Name = "colorLabel";
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Image = global::SmallNotes.Properties.Resources.ic_action_done;
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Image = global::SmallNotes.Properties.Resources.ic_content_clear;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// colorDialog
			// 
			this.colorDialog.FullOpen = true;
			// 
			// colorButton
			// 
			resources.ApplyResources(this.colorButton, "colorButton");
			this.colorButton.Name = "colorButton";
			this.colorButton.UseVisualStyleBackColor = false;
			this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// TagForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.colorLabel);
			this.Controls.Add(this.titleLabel);
			this.Controls.Add(this.titleTextBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TagForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button colorButton;
	}
}