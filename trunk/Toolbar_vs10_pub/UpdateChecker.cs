using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace ImageRakerToolbar
{
	class UpdateChecker
	{
		public void CheckForUpdate(Form invokeForm, bool checkNow, bool showIfNoUpdateRequied)
		{
			this.invokeForm = invokeForm;
			this.showIfNoUpdateRequied = showIfNoUpdateRequied;

			if (Config.Instance.GetConfigBool("CheckForUpdate"))
			{
				string last = Config.Instance.GetConfig("UpdateLastCheckedDate");
				DateTime lastChecked;

				if (DateTime.TryParse(last, out lastChecked))
				{
					TimeSpan ts = DateTime.Now - lastChecked;

					if (ts.Days >= UpdateCheckPeriod)
					{
						Logger.Log("checking for update");

						checkNow = true;
					}
					else
					{
						Logger.Log("skipping update. time span: {0}", ts.ToString());
					}
				}
				else
				{
					Logger.Warn("cannot parse last checked date! - {0}", last);

					// recovery
					Config.Instance.SetConfig("UpdateLastCheckedDate", DateTime.Now.ToShortDateString());
				}
			}

			if (checkNow)
			{
				// start checking
				Logger.Log("CheckForUpdate - starting thread");

				checkForUpdateThread = new Thread(new ParameterizedThreadStart(CheckForUpdateThread));
				checkForUpdateThread.Start(CheckForUpdateUrl);

				// update last checked date
				Config.Instance.SetConfig("UpdateLastCheckedDate", DateTime.Now.ToShortDateString());
			}
		}

		/// <summary>
		/// call this function in result of CheckForUpdate
		/// </summary>
		public void ShowUpdateMessage(string url)
		{
			if (MessageBox.Show(string.Format("새 버전의 이미지 레이커가 있습니다.\n\n지금 다운로드 페이지로 이동하시겠습니까?"),
					About.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
			{
				//Use no more than one assignment when you test this code. 
				//string target = "ftp://ftp.microsoft.com";
				//string target = "C:\\Program Files\\Microsoft Visual Studio\\INSTALL.HTM"; 

				try
				{
					ExceptionTester.Instance.Throw("showupdatemessage");

					System.Diagnostics.Process.Start(url);
				}
				catch (System.ComponentModel.Win32Exception noBrowser)
				{
					if (noBrowser.ErrorCode == -2147467259)
					{
						Logger.Warn("cannot run to url! - no browser");
						Message.Warn("브라우저를 찾을 수 없습니다.");

//						MessageBox.Show("브라우저를 찾을 수 없습니다.", About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				catch (System.Exception other)
				{
					Logger.Warn("cannot run to url! - unknown error!");
					Message.Warn("알 수 없는 문제가 발생했습니다.");

//					MessageBox.Show("알 수 없는 문제가 발생했습니다.", About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void CheckForUpdateThread(object param)
		{
			Logger.Log("CheckForUpdate - thread started");

			Stopwatch sw = new Stopwatch();
			sw.Start();

			bool succeed = false;
			double version = 0;
			bool updateRequired = false;
			string url = "";
			string message = "";

			try
			{
				string requrl = param.ToString();

				using (HttpWebUtility http = new HttpWebUtility(5 * 1000, false, 0))
				{
					Logger.Log("CheckForUpdate - open url: {0}", requrl);

					ExceptionTester.Instance.Throw("checkforupdatethread");

					http.OpenUrl(requrl, null);

					string res = http.GetHtmlContent();

					Logger.DLog("CheckForUpdate - http: {0}", res);

					// parse file
					string[] lines = res.Split('\n');

					if (lines.Length == 4)
					{
						if (double.TryParse(lines[1], out version))
						{
							succeed = true;
							url = lines[2];
							message = lines[3];

							// check version
							double curVersion = 0;

							if (double.TryParse(About.Version, out curVersion))
							{
								if (version > curVersion)
								{
									updateRequired = true;
								}
							}
							else
							{
								Logger.Warn("cannot parse current version: {0}", version);
							}
						}
						else
						{
							Logger.Warn("cannot parse version number: {0}", lines[1]);
						}
					}

				}
			}
			catch (Exception)
			{
				Logger.Warn("CheckForUpdate - exception while http!");
			}

			CheckForUpdateResultHandler(succeed, updateRequired, version, url, message);

			sw.Stop();

			Logger.Log("CheckForUpdate - elapsed: {0}", sw.ElapsedMilliseconds);
		}

		public delegate void CheckForUpdateResultHandlerDelegate(bool succeed, bool updateRequired, double version, string url, string message);
		public void CheckForUpdateResultHandler(bool succeed, bool updateRequired, double version, string url, string message)
		{
			if(invokeForm == null)
			{
				Logger.Warn("no form to invoke");
				return;
			}

			if (invokeForm.InvokeRequired)
			{
				invokeForm.BeginInvoke(new CheckForUpdateResultHandlerDelegate(CheckForUpdateResultHandler), 
					succeed, updateRequired, version, url, message);
			}
			else
			{
				Logger.Log("check for update result: succeed: {0}, updaterequired: {1}, version: {2}, url: {3}, message: {4}, showifnoupdatereq: {5}",
					succeed, updateRequired, version, url, message, showIfNoUpdateRequied);

				if (succeed)
				{
					if (updateRequired)
					{
						ShowUpdateMessage(url);
					}
					else if (showIfNoUpdateRequied)
					{
						Message.Info("현재 버전이 최신버전입니다.", About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					Logger.Warn("check for update failed!");
					Message.Warn("업데이트 확인 실패");
				}
			}
		}

		private const int UpdateCheckPeriod = 7;	// in day

		private bool showIfNoUpdateRequied = false;
		private Form invokeForm = null;

		private Thread checkForUpdateThread;

#if DEBUG
		private static string CheckForUpdateUrl = "http://127.0.0.1/version.txt";
#else
		private static string CheckForUpdateUrl = "http://127.0.0.1/version.txt";
#endif

    }
}
