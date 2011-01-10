using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Windows7.DesktopIntegration.WindowsForms;

namespace ImageRakerToolbar
{
	public partial class ImageRakerDownloadForm : Form
	{
		public ImageRakerDownloadForm(string saveFolder, List<UrlPair> urls,
			 FilePathMaker.FileNameMakingMethod fileNameMakingMethod, bool closeOnSucceed, ImageRaker.SaveType saveType)
		{
			InitializeComponent();

			this.saveFolder = saveFolder;
			this.urls = urls;
			this.fileNameMakingMethod = fileNameMakingMethod;
			this.closeOnSucceed = closeOnSucceed;
//			this.overwrite = overwrite;
//			this.transparent = transparent;
			this.saveType = saveType;

			messageLabel.Text = "이미지 다운로드 준비 중";
			progressLabel.Text = "0 / 0";
		}

		public void CancelSaving()
		{
			Logger.Info("cancel saving...");

			okOrAbortButton.PerformClick();
		}

		public void DeleteSaved()
		{
			Logger.Log("delete saved");

			foreach (UrlPair url in urls)
			{
				if (url.Result == UrlPair.SaveResult.Saved
					|| url.Result == UrlPair.SaveResult.Failed)
				{
					Logger.DLog("delete {0}", url.Filename);

					File.Delete(url.Filename);
				}
			}
		}

		public enum SaveCompleteState
		{
			// used in form
			SaveComplete,			// 모두 성공
			OneOrMoreFailed,		// 1개 이상 실패
			Canceled,
			
			// used in auto
			AlreadySaved,
			NothingToSave			// 저장할 이미지가 없음
		}

		// autosavemanager에서 사용
		public delegate void SaveCompleteDelegate(SaveCompleteState state, int total, int succeed, int failed, int duplicated, int timeElapsed);
		public event SaveCompleteDelegate SaveComplete;

		// 이걸 사용할 경우 topmost를 사용할 수 없다. (.net 2.0버그)
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		private void ImageRakerDownloadForm_Load(object sender, EventArgs e)
		{
			progressBar.Minimum = 0;
			progressBar.Maximum = urls.Count;
			progressBar.Value = 0;
			progressBar.Step = 1;

			// 별도 저장
			currentProgress = 0;

			bool close = false;

			try
			{
				Logger.Log("create imageraker! savefolder: {0}, filenamemakingmethod: {1}", saveFolder, fileNameMakingMethod);

				raker = new ImageRaker(saveFolder, fileNameMakingMethod, saveType);
			}
			catch(UnauthorizedAccessException ex)
			{
				Logger.Warn("cannot create imageraker!, UnauthorizedAccessException");

				exceptionMessage = "권한이 없습니다. 익스플로러를 관리자 권한으로 실행하십시오.";
				close = true;
			}
			catch(DirectoryNotFoundException ex)
			{
				Logger.Warn("cannot create imageraker!, DirectoryNotFoundException");

				exceptionMessage = "잘못된 경로입니다. 경로를 다시 지정하십시오.";
				close = true;
			}
			catch(IOException ex)
			{
				Logger.Warn("cannot create imageraker!, IOException");

				exceptionMessage = "잘못된 경로입니다. 경로를 다시 지정하십시오.";
				close = true;
			}
			//catch (Exception ex)
			//{
			//    Logger.Warn("cannot create imageraker!, savefolder: {0}, filenamemakingmethod: {1}", saveFolder, fileNameMakingMethod);

			//    DialogResult = DialogResult.Abort;
			//    this.Close();
			//}

			if(close)
			{
				DialogResult = DialogResult.Abort;
				this.Close();
			}
			else
			{
				raker.Progress += imageRaker_Progress;
				raker.SaveComplete += imageRaker_SaveComlete;

				int numOfThreads = Config.Instance.GetConfigInt("NumOfThreads");

				if (numOfThreads < MinNumOfThreads)
					numOfThreads = MinNumOfThreads;
				if (numOfThreads > MaxNumOfThreads)
					numOfThreads = MaxNumOfThreads;

				raker.SaveImages(urls, numOfThreads, AbortTimeout);
			}
		}

		private void okOrAbortButton_Click(object sender, EventArgs e)
		{
			if(raker != null && raker.IsComplete())
			{
				// 확인 버튼
				DialogResult = DialogResult.None;
			}
			else
			{
				// 취소 버튼
				raker.Abort();

				// delete saved
				DeleteSaved();

				DialogResult = DialogResult.Cancel;

				// notify
				if (SaveComplete != null)
				{
					SaveComplete(SaveCompleteState.Canceled, 0, 0, 0, 0, 0);
				}
			}

			this.Close();
		}

		private void imageRaker_Progress(string url, bool succeed, int total)
		{
			if(this.InvokeRequired)
			{
				this.BeginInvoke(new ImageRaker.ProgressEventHandlerDelegate(imageRaker_Progress), url, succeed, total);
			}
			else
			{
				currentProgress++;

				messageLabel.Text = string.Format("저장 중\n\n{0}", url);
				progressLabel.Text = string.Format("{0} / {1}", currentProgress, total);

				progressBar.PerformStep();

				Logger.DLog("imageRaker_Progress - succeed: {0}, total: {1}", succeed, total);

				// windows 7
				if(succeed == false)
				{
					WindowsFormsExtensions.SetTaskbarProgressState(this, Windows7.DesktopIntegration.Windows7Taskbar.ThumbnailProgressState.Error);
				}

				WindowsFormsExtensions.SetTaskbarProgress(progressBar);
			}
		}

		private void imageRaker_SaveComlete(int total, int succeed, int failed, int duplicated, int timeElapsed)
		{
			if(this.InvokeRequired)
			{
				this.BeginInvoke(new ImageRaker.SaveComleteEventHandlerDelegate(imageRaker_SaveComlete), 
					total, succeed, failed, duplicated, timeElapsed);
			}
			else
			{
				//numOfImages = total;
				//numOfSucceed = succeed;
				//numOfFailed = failed;

				if (timeElapsed == 0)
				{
					timeElapsed = 1;
				}

				if(total == succeed)
				{
					messageLabel.Text = string.Format("이미지 저장이 완료되었습니다. 소요시간: {0}초\n\n성공: {1}",
						(double)timeElapsed / 1000, succeed);
				}
				else
				{
					messageLabel.Text = string.Format("이미지 저장이 완료되었습니다. 소요시간: {0}초\n\n성공: {1}, 실패: {2}, 중복: {3}",
						(double)timeElapsed / 1000, succeed, failed, duplicated);
				}

				okOrAbortButton.Text = "확인";

				// notify
				if (SaveComplete != null)
				{
					SaveCompleteState state = SaveCompleteState.SaveComplete;

					if(failed > 0)
					{
						state = SaveCompleteState.OneOrMoreFailed;
					}

					SaveComplete(state, total, succeed, failed, duplicated, timeElapsed);
				}

				// 닫기
				// overwrite한 경우 succeed나 failed로 집계되지 않기 때문에. 실패한게 있을때로 체크한다.
				if (closeOnSucceed && failed == 0 && duplicated == 0
					|| saveType == ImageRaker.SaveType.ByAuto)		// 자동 저장은 무조건 닫는다.
				{
					Logger.Log("closing download form...");

					DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}
		
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
		
		private const int AbortTimeout = 1000 * 10;
		private const int MinNumOfThreads = 1;
		private const int MaxNumOfThreads = 8;

		private ImageRaker raker;
		private string saveFolder;
		private List<UrlPair> urls;
		
		private FilePathMaker.FileNameMakingMethod fileNameMakingMethod;
		
		private bool closeOnSucceed;
		private ImageRaker.SaveType saveType;
		private int currentProgress;

		private string exceptionMessage = "";
	}
}