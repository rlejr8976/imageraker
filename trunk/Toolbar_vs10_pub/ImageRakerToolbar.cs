using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;
using mshtml;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Permissions;
using System.Net;
using System.Threading;
using System.Security.Principal;

namespace ImageRakerToolbar
{
	[Guid("65E08873-B6D6-4f5f-B886-97F883FE9AD2")]
	[BandObject("Image Raker", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar, HelpText = "Image Raker")]
	public class Toolbar : BandObject
	{
		private Button instantSaveButton;
		private System.Windows.Forms.Timer debugTimer;
		private IContainer components;
		private CheckBox autoCheckBox;
		//private Label autoSaveLabel;
		private TransparentLabel autoSaveLabel;
		private Button deleteAutoSavedButton;
		private Button grabItButton;

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.grabItButton = new System.Windows.Forms.Button();
			this.instantSaveButton = new System.Windows.Forms.Button();
			this.debugTimer = new System.Windows.Forms.Timer(this.components);
			this.autoCheckBox = new System.Windows.Forms.CheckBox();
			this.autoSaveLabel = new ImageRakerToolbar.TransparentLabel();
			this.deleteAutoSavedButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// grabItButton
			// 
			this.grabItButton.Location = new System.Drawing.Point(3, 3);
			this.grabItButton.Name = "grabItButton";
			this.grabItButton.Size = new System.Drawing.Size(82, 23);
			this.grabItButton.TabIndex = 0;
			this.grabItButton.Text = "이미지 저장";
			this.grabItButton.UseVisualStyleBackColor = true;
			this.grabItButton.Click += new System.EventHandler(this.grabItButton_Click);
			// 
			// instantSaveButton
			// 
			this.instantSaveButton.Location = new System.Drawing.Point(91, 3);
			this.instantSaveButton.Name = "instantSaveButton";
			this.instantSaveButton.Size = new System.Drawing.Size(67, 23);
			this.instantSaveButton.TabIndex = 1;
			this.instantSaveButton.Text = "바로 저장";
			this.instantSaveButton.UseVisualStyleBackColor = true;
			this.instantSaveButton.Click += new System.EventHandler(this.instantSaveButton_Click);
			// 
			// debugTimer
			// 
			this.debugTimer.Enabled = true;
			this.debugTimer.Interval = 30000;
			this.debugTimer.Tick += new System.EventHandler(this.debugTimer_Tick);
			// 
			// autoCheckBox
			// 
			this.autoCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
			this.autoCheckBox.Location = new System.Drawing.Point(164, 3);
			this.autoCheckBox.Name = "autoCheckBox";
			this.autoCheckBox.Size = new System.Drawing.Size(67, 22);
			this.autoCheckBox.TabIndex = 3;
			this.autoCheckBox.Text = "자동 저장";
			this.autoCheckBox.UseVisualStyleBackColor = true;
			this.autoCheckBox.CheckedChanged += new System.EventHandler(this.autoCheckBox_CheckedChanged);
			// 
			// autoSaveLabel
			// 
			this.autoSaveLabel.Location = new System.Drawing.Point(246, 8);
			this.autoSaveLabel.Name = "autoSaveLabel";
			this.autoSaveLabel.Size = new System.Drawing.Size(179, 12);
			this.autoSaveLabel.TabIndex = 4;
			this.autoSaveLabel.Text = "NN개의 이미지를 저장했습니다.";
			// 
			// deleteAutoSavedButton
			// 
			this.deleteAutoSavedButton.Location = new System.Drawing.Point(431, 3);
			this.deleteAutoSavedButton.Name = "deleteAutoSavedButton";
			this.deleteAutoSavedButton.Size = new System.Drawing.Size(38, 23);
			this.deleteAutoSavedButton.TabIndex = 5;
			this.deleteAutoSavedButton.Text = "취소";
			this.deleteAutoSavedButton.UseVisualStyleBackColor = true;
			this.deleteAutoSavedButton.Click += new System.EventHandler(this.deleteAutoSavedButton_Click);
			// 
			// Toolbar
			// 
			this.Controls.Add(this.deleteAutoSavedButton);
			this.Controls.Add(this.autoSaveLabel);
			this.Controls.Add(this.autoCheckBox);
			this.Controls.Add(this.instantSaveButton);
			this.Controls.Add(this.grabItButton);
			this.Name = "Toolbar";
			this.Size = new System.Drawing.Size(799, 102);
			this.ResumeLayout(false);

		}

		public static string AppFolder
		{
			get { return Environment.SpecialFolder.ProgramFiles + "\\ImageRaker"; }
		}

		public Toolbar()
		{
#if (!DEBUG)
			Logger.ResetLogFile();
#endif

			InitUnhandledExceptionHandler();

			Logger.Log("");
			Logger.Log("");
#if DEBUG
			Logger.Log("=== STARTING IMAGERAKERTOOLBAR DEBUG {0} ===", About.Version);
#else
			Logger.Log("=== starting ImageRakerToolbar {0}, {1} ===", About.Version, About.GetBuildDate());
#endif
			Logger.Log("");
			Logger.Log("");

			Application.EnableVisualStyles();

			// init ui
			InitializeComponent();

			minSize = new Size(185, 27); // 최소 크기를 반드시 적어줘야 합니다.

			SetSiteEvent += new SetSiteEventHandler(ImageRakerToolbar_SetSiteEvent);
			ShowDWEvent += new ShowDWEventHandler(ImageRakerToolbar_ShowDWEvent);

			// hide or show instant save button
			UpdateOtherSaveButtonState();

			// hide auto label, del button
			//ShowAutoMessage(false, false);
			HideAllAutoMessage();
		}

		public void UpdateOtherSaveButtonState()
		{
			bool showOtherSaveButton = Config.Instance.GetConfigBool("ShowAutoSaveButton");

			Logger.Log("show instant/auto save button: {0}", showOtherSaveButton);

			instantSaveButton.Visible = showOtherSaveButton;
			autoCheckBox.Visible = showOtherSaveButton;

			// 숨기기만 한다.
			if(showOtherSaveButton == false)
			{
//				ShowAutoMessage(false, false);
				HideAllAutoMessage();
			}
		}

		private void HideAllAutoMessage()
		{
			autoSaveLabel.Visible = false;
			deleteAutoSavedButton.Visible = false;
		}

		/// <summary>
		/// autosavelabel, deleteautosavedbutton의 show/hide
		/// </summary>
		private void ShowAutoMessage(string msg, bool show, bool showDelButton)
		{
			autoSaveLabel.Text = msg;

			autoSaveLabel.Visible = show;
			deleteAutoSavedButton.Visible = showDelButton;

			// move del button
			int width = 0;

			using (Graphics g = autoSaveLabel.CreateGraphics())
			{
				width = (int)g.MeasureString(autoSaveLabel.Text, autoSaveLabel.Font).Width;

				Logger.DLog("string width of auto save label: {0}", width);
			}

			autoSaveLabel.Size = new Size(width, autoSaveLabel.Size.Height);
			deleteAutoSavedButton.Location = new Point(autoSaveLabel.Location.X + autoSaveLabel.Size.Width + 6, deleteAutoSavedButton.Location.Y);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (this.BackColor != Color.Transparent)
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Rectangle rec = new Rectangle(e.ClipRectangle.Left,
				e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
				try
				{
					UxTheme.DrawThemeParentBackground(this.Handle, hdc, ref rec);
				}
				catch (Exception ex)
				{
					Logger.Warn("cannot draw toolbar background! {0}", ex.Message);
				}
				e.Graphics.ReleaseHdc(hdc);
			}
			else
			{
				base.OnPaintBackground(e);
			}
		} 


		private void ImageRakerToolbar_ShowDWEvent(bool fShow)
		{
			Logger.DLog("ImageRakerToolbar_ShowDWEvent");
		}

		private void ImageRakerToolbar_SetSiteEvent(object pUnkSite)
		{
			if (pUnkSite != null)
			{
				Logger.DLog("ImageRakerToolbar_SetSiteEvent, add events");

				if (webBrowserClass != null)
				{
					Logger.DLog("add WindowStateChanged event");

					webBrowserClass.WindowStateChanged += new SHDocVw.DWebBrowserEvents2_WindowStateChangedEventHandler(webBrowser_WindowStateChanged);

					// add events
//					webBrowserClass.BeforeNavigate2 +=	new DWebBrowserEvents2_BeforeNavigate2EventHandler(webBrowser_BeforeNavigate2);
					webBrowserClass.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(webBrowser_NavigateComplete2);
					webBrowserClass.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(webBrowser_DocumentComplete);				
				}
			}			
			else
			{
				Logger.DLog("ImageRakerToolbar_SetSiteEvent, remove events");

				if (webBrowserClass != null)
				{
					Logger.DLog("remove WindowStateChanged event");

					webBrowserClass.WindowStateChanged -= new SHDocVw.DWebBrowserEvents2_WindowStateChangedEventHandler(webBrowser_WindowStateChanged);

					// remove events
//					webBrowserClass.BeforeNavigate2 -= new DWebBrowserEvents2_BeforeNavigate2EventHandler(webBrowser_BeforeNavigate2);
					webBrowserClass.NavigateComplete2 -= new DWebBrowserEvents2_NavigateComplete2EventHandler(webBrowser_NavigateComplete2);
					webBrowserClass.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(webBrowser_DocumentComplete);
				}
			}
		}

		private void webBrowser_WindowStateChanged(uint dwWindowStateFlags, uint dwValidFlagsMask)
		{
			Logger.DLog("Explorer_WindowStateChanged: {0}", dwWindowStateFlags);

			// used on tab browser
			if (dwWindowStateFlags == 3)
			{
				Logger.DLog("Explorer_WindowStateChanged - tab changed");

				UpdateOtherSaveButtonState();
			}
			else
			{
			}
		}

		private void webBrowser_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName,
				  ref object PostData, ref object Headers, ref bool Cancel)
		{
			Logger.Log("webBrowser_BeforeNavigate2");
		}

		private object topdisp = null;
		private bool allowAutoCompleteMessage = false;		// 자동저장이 느려서 페이지 전환 후 완료 메시지가 뜨지 않도록 하기 위함.
		private bool autoSaving = false;				// 저장중일때 취소하면 cancel, 이후에는 del

		private void webBrowser_NavigateComplete2(object pDisp, ref object URL)
		{
			if (!autoCheckBox.Checked)
			{
				return;
			}

			Logger.Log("webBrowser_NavigateComplete2 - url: {0}", URL);

			if (topdisp == null)
			{
				topdisp = pDisp;

				Logger.Info("CHANGING PAGE... reset auto message!");

				ShowAutoMessage(string.Format("준비 중..."), true, false);

				allowAutoCompleteMessage = false;
			}
		}

		private void webBrowser_DocumentComplete(object pDisp, ref object URL)
		{
			if (!autoCheckBox.Checked)
			{
				return;
			}

			Logger.Log("webBrowser_DocumentComplete - auto mode: {0}, url: {1}", autoCheckBox.Checked, URL);

			if (topdisp != null && topdisp == pDisp)
			{
				Logger.Info("ALL DOC COMPLETE!!!");
				topdisp = null;

				// start auto save
				if (autoCheckBox.Checked)
				{
					IHTMLDocument2 doc = GetSafeHTMLDocument2();

					int numOfImagesToSave = autoSaveManager.OnDocumentComplete(doc, autoSaveManager_SaveCompleteDelegate);

					//
					if(numOfImagesToSave > 0)
					{
						ShowAutoMessage(string.Format("{0}개의 이미지 저장 중...", numOfImagesToSave), true, true);

//						MoveDelButton();

						// set state
						allowAutoCompleteMessage = true;
						autoSaving = true;
					}
				}
			}
		}

		private void autoSaveManager_SaveCompleteDelegate(ImageRakerDownloadForm.SaveCompleteState state, 
			int total, int succeed, int failed, int duplicated, int timeElapsed)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new ImageRakerDownloadForm.SaveCompleteDelegate(autoSaveManager_SaveCompleteDelegate), 
					state, total, succeed, failed, duplicated, timeElapsed);
			}
			else
			{
				autoSaving = false;

				UpdateAutoSaveLabel(state, total, succeed, failed);
			}
		}

		private void UpdateAutoSaveLabel(ImageRakerDownloadForm.SaveCompleteState state, int total, int succeed, int failed)
		{
			switch(state)
			{
				case ImageRakerDownloadForm.SaveCompleteState.SaveComplete:
					break;

				case ImageRakerDownloadForm.SaveCompleteState.OneOrMoreFailed:
					break;

				case ImageRakerDownloadForm.SaveCompleteState.AlreadySaved:
					{
						ShowAutoMessage("이미 저장된 페이지입니다.", true, false);
						return;
					}
					break;

				case ImageRakerDownloadForm.SaveCompleteState.NothingToSave:
					{
						ShowAutoMessage("저장할 이미지가 없습니다.", true, false);
						return;
					}
					break;

				case ImageRakerDownloadForm.SaveCompleteState.Canceled:
					{
						ShowAutoMessage("취소되었습니다.", true, false);
						return;
					}
					break;
			}

			if (!allowAutoCompleteMessage)
			{
				Logger.Warn("show auto message NOT ALLOWED!");

				return;
			}

			Logger.Log("UpdateAutoSaveLabel - num of saved: {0}", succeed);

			// show
			ShowAutoMessage(string.Format("{0}개의 이미지를 저장했습니다.", succeed), true, succeed > 0);
		}

		//private void MoveDelButton()
		//{
		//    int width = 0;

		//    // move del button
		//    using (Graphics g = autoSaveLabel.CreateGraphics())
		//    {
		//        width = (int)g.MeasureString(autoSaveLabel.Text, autoSaveLabel.Font).Width;

		//        Logger.DLog("string width of auto save label: {0}", width);
		//    }

		//    autoSaveLabel.Size = new Size(width, autoSaveLabel.Size.Height);
		//    deleteAutoSavedButton.Location = new Point(autoSaveLabel.Location.X + autoSaveLabel.Size.Width + 6, deleteAutoSavedButton.Location.Y);
		//}

		private void deleteAutoSavedButton_Click(object sender, EventArgs e)
		{
			// determine cancel or delete
			if(autoSaving)
			{
				autoSaveManager.CancelSaving();				
			}
			else
			{
				autoSaveManager.DeleteSaved();
			}

			// hide
			HideAllAutoMessage();
//			ShowAutoMessage(false, false);
		}

		private bool IsAdmin()
		{
			WindowsIdentity id = WindowsIdentity.GetCurrent();
			WindowsPrincipal p = new WindowsPrincipal(id);

			return p.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private bool CheckUAC()
		{
			if (!IsAdmin())
			{
				Message.Warn("익스플로러를 관리자 권한으로 실행해야 합니다.");

				return false;
			}

			return true;
		}

		private void grabItButton_Click(object sender, EventArgs e)
		{
			Logger.Log("Grab it button clicked. open imageraker form.");

			if(!CheckUAC())
			{
				return;
			}

			OpenImageRakerForm(false);
		}

		private void instantSaveButton_Click(object sender, EventArgs e)
		{
			Logger.Log("instant save button clicked");

			if (!CheckUAC())
			{
				return;
			}

			OpenImageRakerForm(true);
		}

		private void autoCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (!CheckUAC())
			{
				return;
			}

			if (autoCheckBox.Checked)
			{
				if(Config.Instance.GetConfigBool("HelpAboutAutoSave"))
				{
					// show help
					Message.Info("자동 저장은 탐색하는 모든 웹 페이지에서 자동으로 이미지를 저장하는 기능입니다.\n\n다음 탐색시부터 자동 저장됩니다.");

					Config.Instance.SetConfig("HelpAboutAutoSave", false.ToString());
				}
			}
			else
			{
//				ShowAutoMessage(false, false);
				HideAllAutoMessage();
			}
		}

		private void OpenImageRakerForm(bool instantMode)
		{
			CheckFirstRun();

			// open imageraker form!
			IHTMLDocument2 doc = GetSafeHTMLDocument2();

			if (doc != null)
			{
				Logger.Log("current url: {0}", webBrowserClass.LocationURL);

				ImageRakerForm form = new ImageRakerForm(this, doc, instantMode);
				form.Show();
			}
			else
			{
				Logger.Warn("IHTMLDocument2 is null!");
				Message.Warn("이미지 레이커를 실행할 수 없습니다. HTML 문서가 아닙니다.");
			}
		}

		private static void CheckFirstRun()
		{
			// first run notice
			if (Config.Instance.GetConfigBool("FirstRun"))
			{
				Logger.Log("say hello to user!");

				Message.Info("이미지 레이커를 사용해주셔서 감사합니다.\n\n이미지 레이커는 프로그램 개선을 위해 프로그램 사용 정보 및 시스템 정보를 수집하고 있습니다. \n프로그램 개선에 참여하지 않으려면 '옵션 - 프로그램 개선'에서 참여하지 않음을 선택해 주십시오.");

				Config.Instance.SetConfig("FirstRun", false.ToString());
			}
		}

		private IHTMLDocument2 GetSafeHTMLDocument2()
		{
			IHTMLDocument2 doc = null;

			try
			{
				ExceptionTester.Instance.Throw("openimagerakerform");

				doc = webBrowserClass.Document as IHTMLDocument2;
			}
			catch (Exception e)
			{
				Logger.Warn("exception on getting HTMLDocument {0}", e.Message);
			}

			return doc;
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private void InitUnhandledExceptionHandler()
		{
//#if (!DEBUG) || (DEBUG && TEST_ERROR_REPORTER)
			Application.ThreadException += new ThreadExceptionEventHandler(UnhandledThreadExceptionHandler);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

			Logger.Log("unhandled exception handler added");
//#endif
		}

		private static void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs t)
		{
			//ErrorReporter.ReportException(t.Exception);
			UsageReporter.Instance.OnException(t.Exception);

			//Application.Exit();	// imageraker만 닫히긴 하나, unhandled exception이 다시 발생하면 이 핸들러로 처리가 불가능하다.
		}

		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			//ErrorReporter.ReportException(args.ExceptionObject as Exception);
			UsageReporter.Instance.OnException(args.ExceptionObject as Exception);

			//Application.Exit();
		}

		private void debugTimer_Tick(object sender, EventArgs e)
		{
			ExceptionTester.Instance.Dump();
		}


		private AutoSaveManager autoSaveManager = new AutoSaveManager();

	}

}
