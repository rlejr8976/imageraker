using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ImageRakerToolbar
{
	public class Logger
	{
#if DEBUG
		public static string LogFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
		public static string LogFileName = "ImageRakerToolbar.log";
#else
		public static string LogFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		public static string LogFileName = "ImageRakerToolbarRelease.log";	// 인스턴스가 여러개일 수 있지만 그냥 이걸로 한다.
#endif

		public static string LogPath = LogFolder + '\\' + LogFileName;

		public static void DLog(string format, params object[] args)
		{
#if DEBUG
			Log("[ DEBUG ]", format, args);
#endif
		}

		public static void Log(string format, params object[] args)
		{
			Log("[GENERAL]", format, args);
		}

		public static void Info(string format, params object[] args)
		{
			Log("[ INFO  ]", format, args);
		}

		public static void Warn(string format, params object[] args)
		{
			Log("[WARNING]", format, args);
		}

		public static void Error(string format, params object[] args)
		{
			Log("[ ERROR ]", format, args);
		}

		private static void Log(string header, string format, params object[] args)
		{
			lock(thisLock)
			{
				int tid = Thread.CurrentThread.ManagedThreadId;
				//AppDomain.GetCurrentThreadId();

				DateTime now = DateTime.Now;
				//	string time = now.ToShortDateString() + " " + now.ToShortTimeString();
				string time = now.ToShortDateString() + " " + string.Format("{0:00}:{1:00}:{2:00}:{3:000}", now.Hour, now.Minute, now.Second, now.Millisecond);
				string msg = time + " " + header + string.Format(" [{0}]", tid) + ": " + string.Format(format, args);

#if DEBUG
				// write to output
				Debug.WriteLine(msg);
#endif

				// write to file
				WriteToFile(msg);
			}
		}

		public static void ResetLogFile()
		{
			using (StreamWriter sw = new StreamWriter(LogPath, false))
			{
				sw.Close();
			}
		}

		private static void WriteToFile(string log)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(LogPath, true))
				{
					sw.WriteLine(log);
				}

			}
			catch (Exception)
			{
				// ignore exception
			}
		}    
		
		private static Object thisLock = new Object();

	}
}
