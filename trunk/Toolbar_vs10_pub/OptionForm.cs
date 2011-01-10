using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ImageRakerToolbar
{
	public partial class OptionForm : Form
	{
		public OptionForm()
		{
			InitializeComponent();
		}

		private void OptionForm_Load(object sender, EventArgs e)
		{
			Logger.Log("open option form.");

			// load from config
			// basics
			thumbnailSizeNumericUpDown.Value = Config.Instance.GetConfigInt("ThumbnailSize");
			minSizeNumericUpDown.Value = Config.Instance.GetConfigInt("MinSize");

			switch(Config.Instance.GetConfig("ThumbnailQuality"))
			{
				case "High":
					thumbnailQualityComboBox.SelectedIndex = 0;
					break;

				case "Low":
					thumbnailQualityComboBox.SelectedIndex = 1;
					break;
			}

			// file name method
			string usePartialUrl = FilePathMaker.FileNameMakingMethod.PartialUrl.ToString();
			string useUrl = FilePathMaker.FileNameMakingMethod.FullUrl.ToString();
			string useFileName = FilePathMaker.FileNameMakingMethod.FileName.ToString();
//			string useFileNameNotAllowed = FilePathMaker.FileNameMakingMethod.FileNameDuplicateNotAllowed.ToString();
			string useFileNameOrUrl = FilePathMaker.FileNameMakingMethod.FileNameOrUrl.ToString();

			string fileNameMakingMethod = Config.Instance.GetConfig("FileNameMakingMethod");

			if(fileNameMakingMethod == usePartialUrl)
			{
				partialUrlRadioButton.Checked = true;
			}
			else if (fileNameMakingMethod == useUrl)
			{
				urlRadioButton.Checked = true;
			}
			else if (fileNameMakingMethod == useFileName)	// default
			{
				fileNameRadioButton.Checked = true;
			}
				// 사라진 옵션
			//else if (fileNameMakingMethod == useFileNameNotAllowed)
			//{
			//    fileNameDuplicateNotAllowedRadioButton.Checked = true;
			//}
			else if(fileNameMakingMethod == useFileNameOrUrl)
			{
				fileNameOrUrlRadioButton.Checked = true;
			}
			else
			{
				// set default
				Logger.Warn("Cannot recognize file name making method config value {0}. default value used.", fileNameMakingMethod);
				partialUrlRadioButton.Checked = true;
			}

			showAutoSaveCheckBox.Checked = Config.Instance.GetConfigBool("ShowAutoSaveButton");
			useBlockUrlsCheckBox.Checked = Config.Instance.GetConfigBool("UseBlockUrls");
			markFailedCheckBox.Checked = Config.Instance.GetConfigBool("MarkFailed");
			exitOnCompleteCheckBox.Checked = Config.Instance.GetConfigBool("ExitOnComplete");
			checkForUpdateCheckBox.Checked = Config.Instance.GetConfigBool("CheckForUpdate");

			// improve program
			oldImproveProgram = improveAgreeRadioButton.Checked = Config.Instance.GetConfigBool("ImproveProgram");
			improveDisagreeRadioButton.Checked = !Config.Instance.GetConfigBool("ImproveProgram");

			// show about
			SetAboutText();
		}

		private void SetAboutText()
		{
			aboutRichTextBox.Text = string.Format(
@"
이미지 레이커 v{0} ({1})
ImageRaker by ljh131

mail : ljh131@gmail.com
blog : http://ljh131.tistory.com/

1. 이 프로그램은 이윤 추구를 위한 상업적 목적으로 사용할 수 없으며 개인적 용도로만 사용할 수 있습니다.
2. 이 프로그램을 설치해서 발생될 수 있는 시스템 오류 및 오작동의 어떠한 책임도 제작자에게 있지 않습니다.
3. 이 프로그램을 이용하여 저작권이 있는 내용을 저작권자의 허가 없이 사용하는 행위 등, 다른 사람의 법적 권리를 침해하는 경우 모든 책임은 사용자의 책임입니다. 만일 본 프로그램을 사용하여 위법한 일이 발생될 경우 이는 전적으로 사용자의 책임입니다.
4. 이 프로그램을 사용하는 것은 위의 조항들에 동의하는 것으로 간주합니다.",
								About.Version, About.GetBuildDate());
			try
			{
				ExceptionTester.Instance.Throw("showabout");

				aboutRichTextBox.Font = new Font("맑은 고딕", 10, FontStyle.Regular);
			}
			catch (Exception ex)
			{
				Logger.Warn("cannot set about font - {0}", ex.Message);
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			Logger.Log("save option.");

			// basics
			Config.Instance.SetConfig("ThumbnailSize", thumbnailSizeNumericUpDown.Value.ToString());
			Config.Instance.SetConfig("MinSize", minSizeNumericUpDown.Value.ToString());

			switch(thumbnailQualityComboBox.SelectedIndex)
			{
				case 0:
					Config.Instance.SetConfig("ThumbnailQuality", "High");
					break;

				case 1:
					Config.Instance.SetConfig("ThumbnailQuality", "Low");
					break;
			}

			// file name method
			if(partialUrlRadioButton.Checked)
			{
				Config.Instance.SetConfig("FileNameMakingMethod", FilePathMaker.FileNameMakingMethod.PartialUrl.ToString());
			}
			else if(urlRadioButton.Checked)
			{
				Config.Instance.SetConfig("FileNameMakingMethod", FilePathMaker.FileNameMakingMethod.FullUrl.ToString());
			}
			else if (fileNameRadioButton.Checked)
			{
				Config.Instance.SetConfig("FileNameMakingMethod", FilePathMaker.FileNameMakingMethod.FileName.ToString());
			}
			//else if (fileNameDuplicateNotAllowedRadioButton.Checked)
			//{
			//    Config.Instance.SetConfig("FileNameMakingMethod", FilePathMaker.FileNameMakingMethod.FileNameDuplicateNotAllowed.ToString());
			//}
			else if (fileNameOrUrlRadioButton.Checked)
			{
				Config.Instance.SetConfig("FileNameMakingMethod", FilePathMaker.FileNameMakingMethod.FileNameOrUrl.ToString());
			}

			Config.Instance.SetConfig("ShowAutoSaveButton", showAutoSaveCheckBox.Checked.ToString());
			Config.Instance.SetConfig("UseBlockUrls", useBlockUrlsCheckBox.Checked.ToString());
			Config.Instance.SetConfig("MarkFailed", markFailedCheckBox.Checked.ToString());
			Config.Instance.SetConfig("ExitOnComplete", exitOnCompleteCheckBox.Checked.ToString());
			Config.Instance.SetConfig("CheckForUpdate", checkForUpdateCheckBox.Checked.ToString());

			// improve
			Config.Instance.SetConfig("ImproveProgram", improveAgreeRadioButton.Checked.ToString());

			// show message
			//MessageBox.Show("일부 옵션은 프로그램 재시작시 적용됩니다.", AboutForm.AppName);

			// check changes
			bool currentImproveProgram = Config.Instance.GetConfigBool("ImproveProgram");

			if(currentImproveProgram != oldImproveProgram)
			{
				Logger.Log("improve program value changed to: {0}", currentImproveProgram);

				// report
				UsageReporter.Instance.SendAgreedOnImproveProgramReport(currentImproveProgram);
			}

			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Logger.Log("ignore save.");

			this.Close();
		}
		
		private void clearBlockListButton_Click(object sender, EventArgs e)
		{
			Config.Instance.ClearBlockUrls();
			Config.Instance.SaveBlockUrls();

			//MessageBox.Show("자동 선택 금지 목록이 삭제되었습니다.", 
			//    About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

			Message.Info("자동 선택 금지 목록이 삭제되었습니다.");
		}

		private void checkUpdateNowButton_Click(object sender, EventArgs e)
		{
			updateChecker.CheckForUpdate(this, true, true);
		}

		private bool oldImproveProgram = true;

		UpdateChecker updateChecker = new UpdateChecker();

	}
}
