namespace ImageRakerToolbar
{
	partial class OptionForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.minSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.checkUpdateNowButton = new System.Windows.Forms.Button();
			this.clearBlockListButton = new System.Windows.Forms.Button();
			this.checkForUpdateCheckBox = new System.Windows.Forms.CheckBox();
			this.exitOnCompleteCheckBox = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.urlRadioButton = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.thumbnailSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.markFailedCheckBox = new System.Windows.Forms.CheckBox();
			this.useBlockUrlsCheckBox = new System.Windows.Forms.CheckBox();
			this.showAutoSaveCheckBox = new System.Windows.Forms.CheckBox();
			this.thumbnailQualityComboBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.improveDisagreeRadioButton = new System.Windows.Forms.RadioButton();
			this.improveAgreeRadioButton = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.aboutRichTextBox = new System.Windows.Forms.RichTextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.partialUrlRadioButton = new System.Windows.Forms.RadioButton();
			this.fileNameOrUrlRadioButton = new System.Windows.Forms.RadioButton();
			this.fileNameRadioButton = new System.Windows.Forms.RadioButton();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.minSizeNumericUpDown)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailSizeNumericUpDown)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Controls.Add(this.tabPage3);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(547, 384);
			this.tabControl.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.minSizeNumericUpDown);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.checkUpdateNowButton);
			this.tabPage1.Controls.Add(this.clearBlockListButton);
			this.tabPage1.Controls.Add(this.checkForUpdateCheckBox);
			this.tabPage1.Controls.Add(this.exitOnCompleteCheckBox);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.thumbnailSizeNumericUpDown);
			this.tabPage1.Controls.Add(this.markFailedCheckBox);
			this.tabPage1.Controls.Add(this.useBlockUrlsCheckBox);
			this.tabPage1.Controls.Add(this.showAutoSaveCheckBox);
			this.tabPage1.Controls.Add(this.thumbnailQualityComboBox);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(539, 358);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "   일반   ";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(18, 256);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(289, 12);
			this.label9.TabIndex = 37;
			this.label9.Text = "최소 크기 이하의 이미지는 자동 저장 되지 않습니다.";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(198, 230);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 12);
			this.label7.TabIndex = 36;
			this.label7.Text = "(범위: 0~500)";
			// 
			// minSizeNumericUpDown
			// 
			this.minSizeNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.minSizeNumericUpDown.Location = new System.Drawing.Point(93, 228);
			this.minSizeNumericUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.minSizeNumericUpDown.Name = "minSizeNumericUpDown";
			this.minSizeNumericUpDown.Size = new System.Drawing.Size(99, 21);
			this.minSizeNumericUpDown.TabIndex = 35;
			this.minSizeNumericUpDown.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(18, 230);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(57, 12);
			this.label8.TabIndex = 34;
			this.label8.Text = "최소 크기";
			// 
			// checkUpdateNowButton
			// 
			this.checkUpdateNowButton.Location = new System.Drawing.Point(20, 317);
			this.checkUpdateNowButton.Name = "checkUpdateNowButton";
			this.checkUpdateNowButton.Size = new System.Drawing.Size(184, 23);
			this.checkUpdateNowButton.TabIndex = 33;
			this.checkUpdateNowButton.Text = "지금 업데이트 확인";
			this.checkUpdateNowButton.UseVisualStyleBackColor = true;
			this.checkUpdateNowButton.Click += new System.EventHandler(this.checkUpdateNowButton_Click);
			// 
			// clearBlockListButton
			// 
			this.clearBlockListButton.Location = new System.Drawing.Point(20, 288);
			this.clearBlockListButton.Name = "clearBlockListButton";
			this.clearBlockListButton.Size = new System.Drawing.Size(184, 23);
			this.clearBlockListButton.TabIndex = 32;
			this.clearBlockListButton.Text = "자동 선택 금지 목록 삭제";
			this.clearBlockListButton.UseVisualStyleBackColor = true;
			this.clearBlockListButton.Click += new System.EventHandler(this.clearBlockListButton_Click);
			// 
			// checkForUpdateCheckBox
			// 
			this.checkForUpdateCheckBox.AutoSize = true;
			this.checkForUpdateCheckBox.Location = new System.Drawing.Point(20, 200);
			this.checkForUpdateCheckBox.Name = "checkForUpdateCheckBox";
			this.checkForUpdateCheckBox.Size = new System.Drawing.Size(196, 16);
			this.checkForUpdateCheckBox.TabIndex = 31;
			this.checkForUpdateCheckBox.Text = "프로그램 실행 시 업데이트 확인";
			this.checkForUpdateCheckBox.UseVisualStyleBackColor = true;
			// 
			// exitOnCompleteCheckBox
			// 
			this.exitOnCompleteCheckBox.AutoSize = true;
			this.exitOnCompleteCheckBox.Location = new System.Drawing.Point(20, 178);
			this.exitOnCompleteCheckBox.Name = "exitOnCompleteCheckBox";
			this.exitOnCompleteCheckBox.Size = new System.Drawing.Size(172, 16);
			this.exitOnCompleteCheckBox.TabIndex = 30;
			this.exitOnCompleteCheckBox.Text = "저장 완료 시 프로그램 종료";
			this.exitOnCompleteCheckBox.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel2.Controls.Add(this.fileNameOrUrlRadioButton);
			this.panel2.Controls.Add(this.fileNameRadioButton);
			this.panel2.Controls.Add(this.partialUrlRadioButton);
			this.panel2.Controls.Add(this.urlRadioButton);
			this.panel2.Location = new System.Drawing.Point(93, 80);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(400, 19);
			this.panel2.TabIndex = 29;
			// 
			// urlRadioButton
			// 
			this.urlRadioButton.AutoSize = true;
			this.urlRadioButton.Location = new System.Drawing.Point(80, 2);
			this.urlRadioButton.Name = "urlRadioButton";
			this.urlRadioButton.Size = new System.Drawing.Size(46, 16);
			this.urlRadioButton.TabIndex = 25;
			this.urlRadioButton.TabStop = true;
			this.urlRadioButton.Text = "URL";
			this.urlRadioButton.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(18, 84);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(57, 12);
			this.label6.TabIndex = 28;
			this.label6.Text = "파일 이름";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(198, 26);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "(범위: 90~200)";
			// 
			// thumbnailSizeNumericUpDown
			// 
			this.thumbnailSizeNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.thumbnailSizeNumericUpDown.Location = new System.Drawing.Point(93, 24);
			this.thumbnailSizeNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.thumbnailSizeNumericUpDown.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.thumbnailSizeNumericUpDown.Name = "thumbnailSizeNumericUpDown";
			this.thumbnailSizeNumericUpDown.Size = new System.Drawing.Size(99, 21);
			this.thumbnailSizeNumericUpDown.TabIndex = 7;
			this.thumbnailSizeNumericUpDown.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
			// 
			// markFailedCheckBox
			// 
			this.markFailedCheckBox.AutoSize = true;
			this.markFailedCheckBox.Location = new System.Drawing.Point(20, 156);
			this.markFailedCheckBox.Name = "markFailedCheckBox";
			this.markFailedCheckBox.Size = new System.Drawing.Size(212, 16);
			this.markFailedCheckBox.TabIndex = 6;
			this.markFailedCheckBox.Text = "저장 실패 시 실패한 이미지만 체크";
			this.markFailedCheckBox.UseVisualStyleBackColor = true;
			// 
			// useBlockUrlsCheckBox
			// 
			this.useBlockUrlsCheckBox.AutoSize = true;
			this.useBlockUrlsCheckBox.Location = new System.Drawing.Point(20, 134);
			this.useBlockUrlsCheckBox.Name = "useBlockUrlsCheckBox";
			this.useBlockUrlsCheckBox.Size = new System.Drawing.Size(280, 16);
			this.useBlockUrlsCheckBox.TabIndex = 5;
			this.useBlockUrlsCheckBox.Text = "자동 선택 시 해제한 이미지는 다음에 선택 안함";
			this.useBlockUrlsCheckBox.UseVisualStyleBackColor = true;
			// 
			// showAutoSaveCheckBox
			// 
			this.showAutoSaveCheckBox.AutoSize = true;
			this.showAutoSaveCheckBox.Location = new System.Drawing.Point(20, 112);
			this.showAutoSaveCheckBox.Name = "showAutoSaveCheckBox";
			this.showAutoSaveCheckBox.Size = new System.Drawing.Size(174, 16);
			this.showAutoSaveCheckBox.TabIndex = 4;
			this.showAutoSaveCheckBox.Text = "바로/자동 저장 버튼 활성화";
			this.showAutoSaveCheckBox.UseVisualStyleBackColor = true;
			// 
			// thumbnailQualityComboBox
			// 
			this.thumbnailQualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.thumbnailQualityComboBox.FormattingEnabled = true;
			this.thumbnailQualityComboBox.Items.AddRange(new object[] {
            "높음",
            "낮음"});
			this.thumbnailQualityComboBox.Location = new System.Drawing.Point(93, 50);
			this.thumbnailQualityComboBox.Name = "thumbnailQualityComboBox";
			this.thumbnailQualityComboBox.Size = new System.Drawing.Size(100, 20);
			this.thumbnailQualityComboBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "썸네일 화질";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "썸네일 크기";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.improveDisagreeRadioButton);
			this.tabPage2.Controls.Add(this.improveAgreeRadioButton);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(539, 358);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "   프로그램 개선   ";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// improveDisagreeRadioButton
			// 
			this.improveDisagreeRadioButton.AutoSize = true;
			this.improveDisagreeRadioButton.Location = new System.Drawing.Point(20, 310);
			this.improveDisagreeRadioButton.Name = "improveDisagreeRadioButton";
			this.improveDisagreeRadioButton.Size = new System.Drawing.Size(263, 16);
			this.improveDisagreeRadioButton.TabIndex = 3;
			this.improveDisagreeRadioButton.TabStop = true;
			this.improveDisagreeRadioButton.Text = "아니오. 프로그램 개선에 참여하지 않습니다.";
			this.improveDisagreeRadioButton.UseVisualStyleBackColor = true;
			// 
			// improveAgreeRadioButton
			// 
			this.improveAgreeRadioButton.AutoSize = true;
			this.improveAgreeRadioButton.Location = new System.Drawing.Point(20, 288);
			this.improveAgreeRadioButton.Name = "improveAgreeRadioButton";
			this.improveAgreeRadioButton.Size = new System.Drawing.Size(199, 16);
			this.improveAgreeRadioButton.TabIndex = 2;
			this.improveAgreeRadioButton.TabStop = true;
			this.improveAgreeRadioButton.Text = "예. 프로그램 개선에 참여합니다.";
			this.improveAgreeRadioButton.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(18, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(500, 240);
			this.label3.TabIndex = 0;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.aboutRichTextBox);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(539, 358);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "   이미지 레이커 정보   ";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// aboutRichTextBox
			// 
			this.aboutRichTextBox.BackColor = System.Drawing.Color.White;
			this.aboutRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.aboutRichTextBox.Location = new System.Drawing.Point(0, 0);
			this.aboutRichTextBox.Margin = new System.Windows.Forms.Padding(13);
			this.aboutRichTextBox.Name = "aboutRichTextBox";
			this.aboutRichTextBox.ReadOnly = true;
			this.aboutRichTextBox.Size = new System.Drawing.Size(539, 363);
			this.aboutRichTextBox.TabIndex = 2;
			this.aboutRichTextBox.Text = "";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(403, 412);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "확인";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(484, 412);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "취소";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 417);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(193, 12);
			this.label5.TabIndex = 3;
			this.label5.Text = "일부 옵션은 재시작 후 적용됩니다.";
			// 
			// partialUrlRadioButton
			// 
			this.partialUrlRadioButton.AutoSize = true;
			this.partialUrlRadioButton.Location = new System.Drawing.Point(0, 1);
			this.partialUrlRadioButton.Name = "partialUrlRadioButton";
			this.partialUrlRadioButton.Size = new System.Drawing.Size(74, 16);
			this.partialUrlRadioButton.TabIndex = 29;
			this.partialUrlRadioButton.TabStop = true;
			this.partialUrlRadioButton.Text = "부분 URL";
			this.partialUrlRadioButton.UseVisualStyleBackColor = true;
			// 
			// fileNameOrUrlRadioButton
			// 
			this.fileNameOrUrlRadioButton.AutoSize = true;
			this.fileNameOrUrlRadioButton.Location = new System.Drawing.Point(197, 1);
			this.fileNameOrUrlRadioButton.Name = "fileNameOrUrlRadioButton";
			this.fileNameOrUrlRadioButton.Size = new System.Drawing.Size(140, 16);
			this.fileNameOrUrlRadioButton.TabIndex = 31;
			this.fileNameOrUrlRadioButton.TabStop = true;
			this.fileNameOrUrlRadioButton.Text = "파일명 (중복 시 URL)";
			this.fileNameOrUrlRadioButton.UseVisualStyleBackColor = true;
			// 
			// fileNameRadioButton
			// 
			this.fileNameRadioButton.AutoSize = true;
			this.fileNameRadioButton.Location = new System.Drawing.Point(132, 1);
			this.fileNameRadioButton.Name = "fileNameRadioButton";
			this.fileNameRadioButton.Size = new System.Drawing.Size(59, 16);
			this.fileNameRadioButton.TabIndex = 30;
			this.fileNameRadioButton.TabStop = true;
			this.fileNameRadioButton.Text = "파일명";
			this.fileNameRadioButton.UseVisualStyleBackColor = true;
			// 
			// OptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(571, 447);
			this.ControlBox = false;
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "이미지 레이커 옵션";
			this.Load += new System.EventHandler(this.OptionForm_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.minSizeNumericUpDown)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailSizeNumericUpDown)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.CheckBox useBlockUrlsCheckBox;
		private System.Windows.Forms.CheckBox showAutoSaveCheckBox;
		private System.Windows.Forms.ComboBox thumbnailQualityComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton improveDisagreeRadioButton;
		private System.Windows.Forms.RadioButton improveAgreeRadioButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox markFailedCheckBox;
		private System.Windows.Forms.NumericUpDown thumbnailSizeNumericUpDown;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton urlRadioButton;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RichTextBox aboutRichTextBox;
		private System.Windows.Forms.CheckBox exitOnCompleteCheckBox;
		private System.Windows.Forms.CheckBox checkForUpdateCheckBox;
		private System.Windows.Forms.Button checkUpdateNowButton;
		private System.Windows.Forms.Button clearBlockListButton;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown minSizeNumericUpDown;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.RadioButton partialUrlRadioButton;
		private System.Windows.Forms.RadioButton fileNameOrUrlRadioButton;
		private System.Windows.Forms.RadioButton fileNameRadioButton;
	}
}