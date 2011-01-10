using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Web;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ImageRakerToolbar
{
	class UsageReporter
	{
		public static UsageReporter Instance
		{
			get { return instance; }
		}

		static UsageReporter()
		{
			Instance.Initialize();
		}

		public void SendUsageReport(string pageUrl, int total, int failed, int elapsed, ImageRaker.SaveType saveType)
		{
			Dictionary<string, string> post = new Dictionary<string, string>();

			FillPostBasics(post);

			post.Add("PageUrl", pageUrl);
			post.Add("Total", total.ToString());
			post.Add("Failed", failed.ToString());
			post.Add("Elapsed", elapsed.ToString());
			post.Add("SaveType", ((int)saveType).ToString());

			// from previous value
			post.Add("SaveCountInSession", saveCountInSession.ToString());
			post.Add("FormLoadTime", formLoadTime.ToString());

			Report(post, UsageReportUrl, Config.Instance.GetConfigBool("ImproveProgram"));
		}

		public void SendAgreedOnImproveProgramReport(bool agreed)
		{
			Dictionary<string, string> post = new Dictionary<string, string>();

			FillPostBasics(post);

			post.Add("Agreed", (agreed ? 1 : 0).ToString());

			Report(post, AgreedReportUrl, true);
		}


		public void SendExceptionReport(Exception ex)
		{
			if (ex == null)
			{
				return;
			}

			Logger.DLog("collect exception info");

			Dictionary<string, string> post = new Dictionary<string, string>();

			// collect exception info
			FillPostBasics(post);
			
			post.Add("ExType", ex.GetType().ToString());
			post.Add("ExMsg", ex.Message);
			post.Add("ExSrc", ex.Source);
			post.Add("ExStack", ex.StackTrace);
			post.Add("ExTarget", ex.TargetSite.ToString());

			Logger.DLog("begin attaching");

			// attatch config, log files
			string configFile = ReadLastTextFromFile(Config.Instance.GetConfigFilePath(), MaxErrorReportFileSize);
			string logFile = ReadLastTextFromFile(Logger.LogPath, MaxErrorReportFileSize);

			post.Add("ConfigFile", configFile);
			post.Add("LogFile", logFile);

			Logger.DLog("begin reporting");

			Report(post, ErrorReportUrl, true);
		}

		public void OnException(Exception ex)
		{
			if (ShowExceptionMessageBox())
			{
				SendExceptionReport(ex);

				Application.Exit();	// imageraker만 닫히긴 하나, unhandled exception이 다시 발생하면 이 핸들러로 처리가 불가능하다.
			}
			else
			{
				Logger.Log("don't send error report");
			}
		}

		/*
		public void OnFatal(string message)
		{
			Logger.Error("FATAL OCCURED! message: {0}", message);

			// show alert message
			MessageBox.Show(message, About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

			// exit program
			Application.Exit();	// imageraker만 닫히긴 하나, unhandled exception이 다시 발생하면 이 핸들러로 처리가 불가능하다.
		}

		public void ShowAlertMessage(string message)
		{
			// show alert message
			MessageBox.Show(message, About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		*/

		/*
		private readonly int MaxNumOfWarnValue = 4;

		public void SendWarnReport(string type, params string[] args)
		{
			if (args.Length > MaxNumOfWarnValue)
			{
				Logger.Warn("Too many args in warn! type: {0}", type);
				return;
			}

			if (agreed)
			{
				Logger.DLog("sending warn report: {0}", type);

				Dictionary<string, string> post = new Dictionary<string, string>();

				FillPostBasics(post);

				post.Add("Type", type);

				for (int i = 0; i < args.Length; i++)
				{
					post.Add(string.Format("Value{0}", i + 1), args[i]);
				}

				Logger.DLog("begin attaching");

				// attatch config, log files
				string configFile = ReadLastTextFromFile(Config.Instance.GetConfigFilePath(), MaxErrorReportFileSize);
				string logFile = ReadLastTextFromFile(Logger.LogPath, MaxErrorReportFileSize);

				post.Add("ConfigFile", configFile);
				post.Add("LogFile", logFile);

				Logger.DLog("begin reporting");

				Report(post, WarnReportUrl, true);
			}
			else
			{
				Logger.Log("don't send warn report");
			}
		}
		*/

		private void FillPostBasics(Dictionary<string, string> post)
		{
			DateTime datetime = DateTime.Now;

			post.Add("IRVer", About.Version);
			post.Add("MacAddress", macAddress);
			post.Add("OsInfo", osInfo);
			post.Add("IeInfo", ieInfo);
			post.Add("DateTime", string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}:{6:000}",
				datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond));
		}

		private void Report(Dictionary<string, string> post, string url, bool send)
		{
			Logger.Log("UsageReporter - report to url: {0}", url);

			if (send)
			{
				Pair<Dictionary<string, string>, string> param = new Pair<Dictionary<string, string>, string>(post, url);

				reportThread = new Thread(new ParameterizedThreadStart(ReportThread));
				reportThread.Start(param);
			}
			else
			{
				Logger.Log("UsageReporter - NOT send");
			}
		}

		private void ReportThread(object param)
		{
			Logger.Log("UsageReporter - thread started");

			// send info to server
			Stopwatch sw = new Stopwatch();
			sw.Start();

			// make post string
			Pair<Dictionary<string, string>, string> pairparam = param as Pair<Dictionary<string, string>, string>;
			Dictionary<string, string> post = null;
			string reportUrl = null;

			if(pairparam != null)
			{
				post = pairparam.First as Dictionary<string, string>;
				reportUrl = pairparam.Second as string;
			}

			if(post == null || reportUrl == null)
			{
				Logger.Warn("invalid report param");

				return;
			}

			string postString = "";

			int count = 0;
			foreach (KeyValuePair<string, string> kvp in post)
			{
				string key = kvp.Key;
				string value = kvp.Value;

				postString += string.Format("{0}={1}", key, HttpUtility.UrlEncode(value));

				if (count != post.Count - 1)
				{
					postString += "&";
				}

				count++;
			}

			//Logger.DLog("UsageReporter - url: {0}, poststring: {1}", reportUrl, postString);
			Logger.DLog("UsageReporter - url: {0}, post: {1}", reportUrl, postString);

			bool uploaded = false;

			// upload
			try
			{
				using(HttpWebUtility http = new HttpWebUtility(5 * 1000, false, 0))
				{
					http.OpenUrl(reportUrl, null, postString);

					ExceptionTester.Instance.Throw("reportthread");

					string res = http.GetHtmlContent();

					Logger.DLog("UsageReporter - http: {0}", res);

					if (res.Contains("<OK>"))
					{
						uploaded = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Warn("UsageReporter - cannot upload usage report!");
			}

			sw.Stop();

			Logger.Log("UsageReporter - report result: {0}, elapsed: {1}", uploaded, sw.ElapsedMilliseconds);
		}

		private readonly int MaxErrorReportFileSize = 64 * 1024;	// 64kb

		/// <summary>
		/// 텍스트 파일의 마지막 n바이트만 읽는다.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="bytesToRead"></param>
		/// <returns></returns>
		private string ReadLastTextFromFile(string path, int lastBytesToRead)
		{
			using (StreamReader sr = new StreamReader(path))
			{
				string text = sr.ReadToEnd();
				text = text.Substring(Math.Max(text.Length - lastBytesToRead, 0), Math.Min(lastBytesToRead, text.Length));

				return text;
			}
		}

		private void Initialize()
		{
			// get mac address
			macAddress = SystemInfo.FindMACAddress();
			Logger.Info("UsageReporter - mac address: {0}", macAddress);

			// get system info
			OperatingSystem os = Environment.OSVersion;

			osInfo = string.Format("{0}|{1}|{2}", os.Platform.ToString(), os.Version.ToString(), os.ServicePack);
			ieInfo = SystemInfo.GetIEVersion();

			// get agreed
			agreed = Config.Instance.GetConfigBool("ImproveProgram");

			Logger.Info("UsageReporter - system info: {0}, {1} - agreed: {2}", osInfo, ieInfo, agreed);
		}

		private bool ShowExceptionMessageBox()
		{
			string AppName = About.AppName;
			DialogResult dr = DialogResult.OK;

			if (agreed)
			{
				dr = MessageBox.Show(ExceptionMessageBoxStringForAgreed, AppName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				dr = MessageBox.Show(ExceptionMessageBoxStringForDisagreed, AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
			}

			if (dr == DialogResult.Yes || dr == DialogResult.OK)
			{
				return true;
			}

			return false;
		}

#if DEBUG
		private string UsageReportUrl = "http://127.0.0.1/reportusage.php";
		private string AgreedReportUrl = "http://127.0.0.1/reportagreed.php";
		private string ErrorReportUrl = "http://127.0.0.1/reporterror.php";
		private string WarnReportUrl = "http://127.0.0.1/reportwarn.php";
#else
		private string UsageReportUrl = "http://127.0.0.1/reportusage.php";
		private string AgreedReportUrl = "http://127.0.0.1/reportagreed.php";
		private string ErrorReportUrl = "http://127.0.0.1/reporterror.php";
		private string WarnReportUrl = "http://127.0.0.1/reportwarn.php";
#endif

        private static string ExceptionMessageBoxStringForAgreed =
@"알 수 없는 오류가 발생하여 프로그램을 종료합니다. 
이미지 레이커의 정상 작동을 위해서는 인터넷 익스플로러를 재시작 하는 것이 좋습니다.

프로그램 개선 동의에 따라 오류 정보를 전송하고 프로그램을 종료합니다.
(이 프로그램은 프로그램 개선을 위한 오류 정보를 제외한 어떠한 개인 정보도 수집하지 않습니다.)
";

		private static string ExceptionMessageBoxStringForDisagreed =
@"알 수 없는 오류가 발생하여 프로그램을 종료합니다. 
이미지 레이커의 정상 작동을 위해서는 인터넷 익스플로러를 재시작 하는 것이 좋습니다.

'예' 버튼을 누르면 오류 정보를 전송하고 프로그램을 종료합니다.
'아니오' 버튼을 누르면 오류 정보를 전송하지 않고 프로그램을 종료합니다.

(이 프로그램은 프로그램 개선을 위한 오류 정보를 제외한 어떠한 개인 정보도 수집하지 않습니다.)
";

		private bool agreed = false;

		private int saveCountInSession = -1;
		private int formLoadTime = -1;

		public int SaveCountInSession
		{
			set { saveCountInSession = value; }
		}

		public int FormLoadTime
		{
			set { formLoadTime = value; }
		}

		private Thread reportThread = null;

		private string macAddress = "";
		private string osInfo = "";
		private string ieInfo = "";

		private static UsageReporter instance = new UsageReporter();


	}
}
