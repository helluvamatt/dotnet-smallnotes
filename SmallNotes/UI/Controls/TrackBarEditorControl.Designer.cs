namespace SmallNotes.UI.Controls
{
	partial class TrackBarEditorControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.displayLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// trackBar
			// 
			this.trackBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.trackBar.Location = new System.Drawing.Point(0, 0);
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(192, 45);
			this.trackBar.TabIndex = 0;
			this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// displayLabel
			// 
			this.displayLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.displayLabel.Location = new System.Drawing.Point(0, 52);
			this.displayLabel.Name = "displayLabel";
			this.displayLabel.Size = new System.Drawing.Size(192, 13);
			this.displayLabel.TabIndex = 1;
			this.displayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TrackBarEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.displayLabel);
			this.Controls.Add(this.trackBar);
			this.Name = "TrackBarEditorControl";
			this.Size = new System.Drawing.Size(192, 65);
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TrackBar trackBar;
		private System.Windows.Forms.Label displayLabel;
	}
}
