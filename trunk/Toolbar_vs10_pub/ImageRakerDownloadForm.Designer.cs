namespace ImageRakerToolbar
{
	partial class ImageRakerDownloadForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageRakerDownloadForm));
			this.messageLabel = new System.Windows.Forms.Label();
			this.progressLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.okOrAbortButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// messageLabel
			// 
			this.messageLabel.Location = new System.Drawing.Point(2, 9);
			this.messageLabel.Name = "messageLabel";
			this.messageLabel.Size = new System.Drawing.Size(466, 58);
			this.messageLabel.TabIndex = 0;
			this.messageLabel.Text = "messageLabel";
			this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressLabel
			// 
			this.progressLabel.Location = new System.Drawing.Point(8, 69);
			this.progressLabel.Name = "progressLabel";
			this.progressLabel.Size = new System.Drawing.Size(454, 24);
			this.progressLabel.TabIndex = 2;
			this.progressLabel.Text = "10 / 10";
			this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 96);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(442, 23);
			this.progressBar.TabIndex = 3;
			// 
			// okOrAbortButton
			// 
			this.okOrAbortButton.Location = new System.Drawing.Point(198, 129);
			this.okOrAbortButton.Name = "okOrAbortButton";
			this.okOrAbortButton.Size = new System.Drawing.Size(75, 23);
			this.okOrAbortButton.TabIndex = 4;
			this.okOrAbortButton.Text = "취소";
			this.okOrAbortButton.UseVisualStyleBackColor = true;
			this.okOrAbortButton.Click += new System.EventHandler(this.okOrAbortButton_Click);
			// 
			// ImageRakerDownloadForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(462, 160);
			this.ControlBox = false;
			this.Controls.Add(this.okOrAbortButton);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.progressLabel);
			this.Controls.Add(this.messageLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImageRakerDownloadForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "이미지 레이커 다운로더";
			this.Load += new System.EventHandler(this.ImageRakerDownloadForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Label progressLabel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button okOrAbortButton;
	}
}