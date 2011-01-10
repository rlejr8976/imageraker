namespace ImageRakerToolbar
{
	partial class ImageRakerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageRakerForm));
			this.selectAllButton = new System.Windows.Forms.Button();
			this.deselectAllButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.saveFolderTextBox = new System.Windows.Forms.TextBox();
			this.browseFolderButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.byOriginalRadioButton = new System.Windows.Forms.RadioButton();
			this.byNameRadioButton = new System.Windows.Forms.RadioButton();
			this.byExtRadioButton = new System.Windows.Forms.RadioButton();
			this.bySizeRadioButton = new System.Windows.Forms.RadioButton();
			this.autoSelectButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.descOrderRadioButton = new System.Windows.Forms.RadioButton();
			this.ascOrderRadioButton = new System.Windows.Forms.RadioButton();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label4 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.jpgCheckBox = new System.Windows.Forms.CheckBox();
			this.gifCheckBox = new System.Windows.Forms.CheckBox();
			this.pngCheckBox = new System.Windows.Forms.CheckBox();
			this.bmpCheckBox = new System.Windows.Forms.CheckBox();
			this.etcCheckBox = new System.Windows.Forms.CheckBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.optionButton = new System.Windows.Forms.Button();
			this.numOfSelectedLabel = new System.Windows.Forms.Label();
			this.showSmallsCheckBox = new System.Windows.Forms.CheckBox();
			this.thumbnailListView = new ThumbnailListView();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectAllButton
			// 
			this.selectAllButton.Location = new System.Drawing.Point(12, 12);
			this.selectAllButton.Name = "selectAllButton";
			this.selectAllButton.Size = new System.Drawing.Size(75, 23);
			this.selectAllButton.TabIndex = 1;
			this.selectAllButton.Text = "모두 선택";
			this.selectAllButton.UseVisualStyleBackColor = true;
			this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
			// 
			// deselectAllButton
			// 
			this.deselectAllButton.Location = new System.Drawing.Point(93, 12);
			this.deselectAllButton.Name = "deselectAllButton";
			this.deselectAllButton.Size = new System.Drawing.Size(75, 23);
			this.deselectAllButton.TabIndex = 2;
			this.deselectAllButton.Text = "모두 해제";
			this.deselectAllButton.UseVisualStyleBackColor = true;
			this.deselectAllButton.Click += new System.EventHandler(this.deselectAllButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(678, 530);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(94, 31);
			this.saveButton.TabIndex = 3;
			this.saveButton.Text = "저장";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 519);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "저장 위치";
			// 
			// saveFolderTextBox
			// 
			this.saveFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.saveFolderTextBox.Location = new System.Drawing.Point(87, 514);
			this.saveFolderTextBox.Name = "saveFolderTextBox";
			this.saveFolderTextBox.ReadOnly = true;
			this.saveFolderTextBox.Size = new System.Drawing.Size(470, 21);
			this.saveFolderTextBox.TabIndex = 5;
			// 
			// browseFolderButton
			// 
			this.browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.browseFolderButton.Location = new System.Drawing.Point(561, 512);
			this.browseFolderButton.Name = "browseFolderButton";
			this.browseFolderButton.Size = new System.Drawing.Size(37, 23);
			this.browseFolderButton.TabIndex = 6;
			this.browseFolderButton.Text = "...";
			this.browseFolderButton.UseVisualStyleBackColor = true;
			this.browseFolderButton.Click += new System.EventHandler(this.browseFolderButton_Click);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 544);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 12);
			this.label3.TabIndex = 13;
			this.label3.Text = "정렬 방식";
			// 
			// byOriginalRadioButton
			// 
			this.byOriginalRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.byOriginalRadioButton.AutoSize = true;
			this.byOriginalRadioButton.Location = new System.Drawing.Point(0, 4);
			this.byOriginalRadioButton.Name = "byOriginalRadioButton";
			this.byOriginalRadioButton.Size = new System.Drawing.Size(75, 16);
			this.byOriginalRadioButton.TabIndex = 14;
			this.byOriginalRadioButton.TabStop = true;
			this.byOriginalRadioButton.Text = "원래 순서";
			this.byOriginalRadioButton.UseVisualStyleBackColor = true;
			this.byOriginalRadioButton.CheckedChanged += new System.EventHandler(this.byOriginalRadioButton_CheckedChanged);
			// 
			// byNameRadioButton
			// 
			this.byNameRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.byNameRadioButton.AutoSize = true;
			this.byNameRadioButton.Location = new System.Drawing.Point(81, 4);
			this.byNameRadioButton.Name = "byNameRadioButton";
			this.byNameRadioButton.Size = new System.Drawing.Size(59, 16);
			this.byNameRadioButton.TabIndex = 15;
			this.byNameRadioButton.TabStop = true;
			this.byNameRadioButton.Text = "이름순";
			this.byNameRadioButton.UseVisualStyleBackColor = true;
			this.byNameRadioButton.CheckedChanged += new System.EventHandler(this.byNameRadioButton_CheckedChanged);
			// 
			// byExtRadioButton
			// 
			this.byExtRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.byExtRadioButton.AutoSize = true;
			this.byExtRadioButton.Location = new System.Drawing.Point(146, 4);
			this.byExtRadioButton.Name = "byExtRadioButton";
			this.byExtRadioButton.Size = new System.Drawing.Size(71, 16);
			this.byExtRadioButton.TabIndex = 16;
			this.byExtRadioButton.TabStop = true;
			this.byExtRadioButton.Text = "확장자순";
			this.byExtRadioButton.UseVisualStyleBackColor = true;
			this.byExtRadioButton.CheckedChanged += new System.EventHandler(this.byExtRadioButton_CheckedChanged);
			// 
			// bySizeRadioButton
			// 
			this.bySizeRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bySizeRadioButton.AutoSize = true;
			this.bySizeRadioButton.Location = new System.Drawing.Point(223, 4);
			this.bySizeRadioButton.Name = "bySizeRadioButton";
			this.bySizeRadioButton.Size = new System.Drawing.Size(59, 16);
			this.bySizeRadioButton.TabIndex = 17;
			this.bySizeRadioButton.TabStop = true;
			this.bySizeRadioButton.Text = "크기순";
			this.bySizeRadioButton.UseVisualStyleBackColor = true;
			this.bySizeRadioButton.CheckedChanged += new System.EventHandler(this.bySizeRadioButton_CheckedChanged);
			// 
			// autoSelectButton
			// 
			this.autoSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.autoSelectButton.Location = new System.Drawing.Point(697, 12);
			this.autoSelectButton.Name = "autoSelectButton";
			this.autoSelectButton.Size = new System.Drawing.Size(75, 23);
			this.autoSelectButton.TabIndex = 18;
			this.autoSelectButton.Text = "자동 선택";
			this.autoSelectButton.UseVisualStyleBackColor = true;
			this.autoSelectButton.Click += new System.EventHandler(this.autoSelectButton_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.Controls.Add(this.descOrderRadioButton);
			this.panel1.Controls.Add(this.ascOrderRadioButton);
			this.panel1.Location = new System.Drawing.Point(409, 539);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(162, 22);
			this.panel1.TabIndex = 22;
			// 
			// descOrderRadioButton
			// 
			this.descOrderRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.descOrderRadioButton.AutoSize = true;
			this.descOrderRadioButton.Location = new System.Drawing.Point(77, 6);
			this.descOrderRadioButton.Name = "descOrderRadioButton";
			this.descOrderRadioButton.Size = new System.Drawing.Size(71, 16);
			this.descOrderRadioButton.TabIndex = 20;
			this.descOrderRadioButton.TabStop = true;
			this.descOrderRadioButton.Text = "내림차순";
			this.descOrderRadioButton.UseVisualStyleBackColor = true;
			this.descOrderRadioButton.CheckedChanged += new System.EventHandler(this.descOrderRadioButton_CheckedChanged);
			// 
			// ascOrderRadioButton
			// 
			this.ascOrderRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ascOrderRadioButton.AutoSize = true;
			this.ascOrderRadioButton.Location = new System.Drawing.Point(0, 6);
			this.ascOrderRadioButton.Name = "ascOrderRadioButton";
			this.ascOrderRadioButton.Size = new System.Drawing.Size(71, 16);
			this.ascOrderRadioButton.TabIndex = 19;
			this.ascOrderRadioButton.TabStop = true;
			this.ascOrderRadioButton.Text = "오름차순";
			this.ascOrderRadioButton.UseVisualStyleBackColor = true;
			this.ascOrderRadioButton.CheckedChanged += new System.EventHandler(this.ascOrderRadioButton_CheckedChanged);
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.Description = "이미지를 저장할 폴더를 선택하세요.";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 572);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 12);
			this.label4.TabIndex = 28;
			this.label4.Text = "확장자 필터";
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel3.Controls.Add(this.byOriginalRadioButton);
			this.panel3.Controls.Add(this.byNameRadioButton);
			this.panel3.Controls.Add(this.byExtRadioButton);
			this.panel3.Controls.Add(this.bySizeRadioButton);
			this.panel3.Location = new System.Drawing.Point(87, 541);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(316, 20);
			this.panel3.TabIndex = 29;
			// 
			// jpgCheckBox
			// 
			this.jpgCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.jpgCheckBox.AutoSize = true;
			this.jpgCheckBox.Location = new System.Drawing.Point(0, 5);
			this.jpgCheckBox.Name = "jpgCheckBox";
			this.jpgCheckBox.Size = new System.Drawing.Size(47, 16);
			this.jpgCheckBox.TabIndex = 30;
			this.jpgCheckBox.Text = "JPG";
			this.jpgCheckBox.UseVisualStyleBackColor = true;
			this.jpgCheckBox.CheckedChanged += new System.EventHandler(this.jpgCheckBox_CheckedChanged);
			// 
			// gifCheckBox
			// 
			this.gifCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.gifCheckBox.AutoSize = true;
			this.gifCheckBox.Location = new System.Drawing.Point(53, 5);
			this.gifCheckBox.Name = "gifCheckBox";
			this.gifCheckBox.Size = new System.Drawing.Size(43, 16);
			this.gifCheckBox.TabIndex = 31;
			this.gifCheckBox.Text = "GIF";
			this.gifCheckBox.UseVisualStyleBackColor = true;
			this.gifCheckBox.CheckedChanged += new System.EventHandler(this.gifCheckBox_CheckedChanged);
			// 
			// pngCheckBox
			// 
			this.pngCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pngCheckBox.AutoSize = true;
			this.pngCheckBox.Location = new System.Drawing.Point(102, 5);
			this.pngCheckBox.Name = "pngCheckBox";
			this.pngCheckBox.Size = new System.Drawing.Size(50, 16);
			this.pngCheckBox.TabIndex = 32;
			this.pngCheckBox.Text = "PNG";
			this.pngCheckBox.UseVisualStyleBackColor = true;
			this.pngCheckBox.CheckedChanged += new System.EventHandler(this.pngCheckBox_CheckedChanged);
			// 
			// bmpCheckBox
			// 
			this.bmpCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bmpCheckBox.AutoSize = true;
			this.bmpCheckBox.Location = new System.Drawing.Point(158, 5);
			this.bmpCheckBox.Name = "bmpCheckBox";
			this.bmpCheckBox.Size = new System.Drawing.Size(51, 16);
			this.bmpCheckBox.TabIndex = 33;
			this.bmpCheckBox.Text = "BMP";
			this.bmpCheckBox.UseVisualStyleBackColor = true;
			this.bmpCheckBox.CheckedChanged += new System.EventHandler(this.bmpCheckBox_CheckedChanged);
			// 
			// etcCheckBox
			// 
			this.etcCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.etcCheckBox.AutoSize = true;
			this.etcCheckBox.Location = new System.Drawing.Point(215, 5);
			this.etcCheckBox.Name = "etcCheckBox";
			this.etcCheckBox.Size = new System.Drawing.Size(42, 16);
			this.etcCheckBox.TabIndex = 34;
			this.etcCheckBox.Text = "Etc";
			this.etcCheckBox.UseVisualStyleBackColor = true;
			this.etcCheckBox.CheckedChanged += new System.EventHandler(this.etcCheckBox_CheckedChanged);
			// 
			// panel4
			// 
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel4.Controls.Add(this.jpgCheckBox);
			this.panel4.Controls.Add(this.etcCheckBox);
			this.panel4.Controls.Add(this.gifCheckBox);
			this.panel4.Controls.Add(this.bmpCheckBox);
			this.panel4.Controls.Add(this.pngCheckBox);
			this.panel4.Location = new System.Drawing.Point(87, 566);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(280, 21);
			this.panel4.TabIndex = 35;
			// 
			// optionButton
			// 
			this.optionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.optionButton.Location = new System.Drawing.Point(678, 567);
			this.optionButton.Name = "optionButton";
			this.optionButton.Size = new System.Drawing.Size(94, 23);
			this.optionButton.TabIndex = 38;
			this.optionButton.Text = "옵션";
			this.optionButton.UseVisualStyleBackColor = true;
			this.optionButton.Click += new System.EventHandler(this.optionButton_Click);
			// 
			// numOfSelectedLabel
			// 
			this.numOfSelectedLabel.Location = new System.Drawing.Point(174, 17);
			this.numOfSelectedLabel.Name = "numOfSelectedLabel";
			this.numOfSelectedLabel.Size = new System.Drawing.Size(82, 12);
			this.numOfSelectedLabel.TabIndex = 39;
			this.numOfSelectedLabel.Text = "NN개 선택됨";
			this.numOfSelectedLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// showSmallsCheckBox
			// 
			this.showSmallsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.showSmallsCheckBox.AutoSize = true;
			this.showSmallsCheckBox.Location = new System.Drawing.Point(575, 16);
			this.showSmallsCheckBox.Name = "showSmallsCheckBox";
			this.showSmallsCheckBox.Size = new System.Drawing.Size(116, 16);
			this.showSmallsCheckBox.TabIndex = 40;
			this.showSmallsCheckBox.Text = "작은 이미지 표시";
			this.showSmallsCheckBox.UseVisualStyleBackColor = true;
			this.showSmallsCheckBox.CheckedChanged += new System.EventHandler(this.showSmallsCheckBox_CheckedChanged);
			// 
			// thumbnailListView
			// 
			this.thumbnailListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.thumbnailListView.CheckBoxes = true;
			this.thumbnailListView.Location = new System.Drawing.Point(12, 41);
			this.thumbnailListView.Name = "thumbnailListView";
			this.thumbnailListView.Size = new System.Drawing.Size(760, 455);
			this.thumbnailListView.TabIndex = 0;
			this.thumbnailListView.ThumbBorderColor = System.Drawing.Color.Wheat;
			this.thumbnailListView.ThumbnailSize = 95;
			this.thumbnailListView.UseCompatibleStateImageBehavior = false;
			this.thumbnailListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.thumbnailListView_ItemChecked);
			// 
			// ImageRakerForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 602);
			this.Controls.Add(this.showSmallsCheckBox);
			this.Controls.Add(this.numOfSelectedLabel);
			this.Controls.Add(this.optionButton);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.autoSelectButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.browseFolderButton);
			this.Controls.Add(this.saveFolderTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.deselectAllButton);
			this.Controls.Add(this.selectAllButton);
			this.Controls.Add(this.thumbnailListView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(800, 640);
			this.Name = "ImageRakerForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "이미지 레이커";
			this.Load += new System.EventHandler(this.ImageRakerForm_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImageRakerForm_FormClosed);
			this.ResizeEnd += new System.EventHandler(this.ImageRakerForm_ResizeEnd);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ThumbnailListView thumbnailListView;
		private System.Windows.Forms.Button selectAllButton;
		private System.Windows.Forms.Button deselectAllButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox saveFolderTextBox;
		private System.Windows.Forms.Button browseFolderButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton byOriginalRadioButton;
		private System.Windows.Forms.RadioButton byNameRadioButton;
		private System.Windows.Forms.RadioButton byExtRadioButton;
		private System.Windows.Forms.RadioButton bySizeRadioButton;
		private System.Windows.Forms.Button autoSelectButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton descOrderRadioButton;
		private System.Windows.Forms.RadioButton ascOrderRadioButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.CheckBox jpgCheckBox;
		private System.Windows.Forms.CheckBox gifCheckBox;
		private System.Windows.Forms.CheckBox pngCheckBox;
		private System.Windows.Forms.CheckBox bmpCheckBox;
		private System.Windows.Forms.CheckBox etcCheckBox;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button optionButton;
		private System.Windows.Forms.Label numOfSelectedLabel;
		private System.Windows.Forms.CheckBox showSmallsCheckBox;



	}
}