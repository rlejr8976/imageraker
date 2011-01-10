using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using SHDocVw;
using mshtml;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ImageRakerToolbar
{
	/// <summary>
	/// url과 refererUrl을 같이 저장
	/// </summary>
	public class UrlPair
	{
		public enum SaveResult
		{
			None,
			Saved,
			Failed,
			Duplicated
		}

		public UrlPair(string url, string referer)
		{
			this.url = url;
			this.referer = referer;
		}

		public string Url
		{
			get { return url; }
		}

		public string RefererUrl
		{
			get { return referer; }
		}

		// 아래부터는 저장이 시작되면 추가
		public string Filename
		{
			set { filename = value; }
			get { return filename; }
		}

		public SaveResult Result
		{
			set { result = value; }
			get { return result; }
		}

		private string url = "";
		private string referer = "";

		private string filename = "";
		private SaveResult result = SaveResult.None;
	}

	public class ImageRaker
	{
		public ImageRaker(string saveFolder, FilePathMaker.FileNameMakingMethod fileNameMakingMethod, SaveType saveType)
		{
			this.saveFolder = saveFolder;
			this.fileNameMakingMethod = fileNameMakingMethod;
			//this.overwrite = overwrite;
			this.saveType = saveType;

			Directory.CreateDirectory(saveFolder);

			filePathMaker = new FilePathMaker(saveFolder);

			Logger.Log("Imageraker created. savefolder: {0}, filenamemakingmethod: {1}", 
						saveFolder, fileNameMakingMethod);
		}

		public enum SaveType
		{
			None = 0,
			ByForm = 1,
			ByInstant = 2,
			ByAuto = 3
		}

		// events
		//public delegate void ProgressEventHandler(string currentUrl, bool succeed, int current, int total);
		//public event ProgressEventHandler Progress;

		public delegate void ProgressEventHandlerDelegate(string url, bool succeed, int total);
		public event ProgressEventHandlerDelegate Progress;

		public delegate void SaveComleteEventHandlerDelegate(int total, int succeed, int failed, int duplicated, int timeElapsed);
		public event SaveComleteEventHandlerDelegate SaveComplete;

		// methods
		public void SaveImages(List<UrlPair> urls, int numOfThreads, int timeout)
		{
			Logger.Log("save images: numofurls: {0}, numofthreads: {1}, timeout: {2}", urls.Count, numOfThreads, timeout);

			this.numOfThreads = numOfThreads;
			this.urls = urls;
			this.timeout = timeout;

			this.currentUrlIndex = 0;
			this.numOfSucceed = 0;
			this.numOfFailed = 0;

			mainThread = new Thread(new ParameterizedThreadStart(MainThread));
			mainThread.Start();
		}

		public bool Abort()
		{
			mainThread.Abort();
			bool re = mainThread.Join(timeout);

			if (re)
			{
				Logger.Log("Imageraker thread aborted successfully.");
			}
			else
			{
				Logger.Log("Imageraker thread abort failed. timeout: {0}", timeout);
			}

			return re;
		}

		public bool IsComplete()
		{
			if (mainThread != null)
			{
				return (!mainThread.IsAlive);
			}

			return true;
		}

		private void MainThread(object param)
		{
			try
			{
				Logger.Log("main thread started. numOfThreads: {0}", numOfThreads);

				Stopwatch sw = new Stopwatch();

				sw.Start();

				this.workerThreads = new Thread[numOfThreads];
				this.manualEvents = new ManualResetEvent[numOfThreads];

				for (int i = 0; i < numOfThreads; i++)
				{
					workerThreads[i] = new Thread(new ParameterizedThreadStart(RakerThread));
					manualEvents[i] = new ManualResetEvent(false);

					workerThreads[i].Start(i);
				}

				WaitHandle.WaitAll(this.manualEvents);

				sw.Stop();

				Logger.Log("all thread terminated! duration: {0}", sw.ElapsedMilliseconds); 

				// report
				string url = "";

				if (urls.Count != 0)
				{
					url = urls[0].RefererUrl;
				}

				UsageReporter.Instance.SendUsageReport(url, urls.Count, numOfFailed, (int)sw.ElapsedMilliseconds, saveType);

				// report
				if (SaveComplete != null)
				{
					SaveComplete(urls.Count, numOfSucceed, numOfFailed, numOfDuplicated, (int)sw.ElapsedMilliseconds);
				}
			}
			catch (ThreadAbortException ex)
			{
				Logger.Warn("mainthread: ThreadAbortException raised. aborting main thread and worker threads.");

				// abort를 app thread에서 할 수 없다. sta기 때문에 waitall이 작동하지 않음.
				for (int i = 0; i < numOfThreads; i++)
				{
					workerThreads[i].Abort();
				}

				bool re = WaitHandle.WaitAll(this.manualEvents, timeout, true);

				Logger.Log("mainthread: all worker thread terminated in {0}ms: {1}", timeout, re);
			}
		}

		private UrlPair GetNextUrl()
		{
			UrlPair url = null;

			lock (lockThis)
			{
				if (this.currentUrlIndex < this.urls.Count)
				{
					Logger.Info("get url at count: {0}/{1}", this.currentUrlIndex, this.urls.Count);

					url = this.urls[this.currentUrlIndex++];
				}
			}

			return url;
		}

		private bool AllowOverwrite()
		{
			if(saveType == SaveType.ByAuto)
			{
				return false;
			}

			if(fileNameMakingMethod == FilePathMaker.FileNameMakingMethod.FullUrl
				|| fileNameMakingMethod == FilePathMaker.FileNameMakingMethod.FileNameOrUrl
				|| fileNameMakingMethod == FilePathMaker.FileNameMakingMethod.PartialUrl)
//				|| overwrite)
			{
				return true;
			}

			return false;
		}

		private void RakerThread(object param)
		{
			int threadId = (int)param;

			Logger.Log("thread{0}: started", threadId);

			try
			{
				using(HttpWebUtility http = new HttpWebUtility())
				{
					UrlPair urlpair = null;

					while((urlpair = GetNextUrl()) != null)
					{
						string filepath = filePathMaker.MakeFilePath(urlpair.Url, fileNameMakingMethod);
						bool reportSucceed = true;

						urlpair.Filename = filepath;

						Logger.Log("    thread{0}: saving url: {1} to file: {2} (referer: {3})", threadId, urlpair.Url, filepath, urlpair.RefererUrl);

						if (!AllowOverwrite() && File.Exists(filepath))
						{
							// duplicated
							Logger.Log("    thread{0}: file already exists! continue to next url.", threadId);

							urlpair.Result = UrlPair.SaveResult.Duplicated;
							numOfDuplicated++;
							reportSucceed = false;
						}
						else
						{
							try
							{
								http.SaveContentToFile(urlpair.Url, filepath, urlpair.RefererUrl);

								ExceptionTester.Instance.Throw("exception");
							}
							catch (ThreadAbortException ex)
							{
								Logger.Warn("	thread{0}: ThreadAbortException raised.", threadId);

								throw;
							}
							catch (Exception ex)
							{
								Logger.Warn("	thread{0}: exception has occured during saving image. '{1}'. try again, using encoded refererurl",
									threadId, ex.Message);

								// try again, using encoded refererurl
								try
								{
									// refererUrl에 한글이 포함되어있으면 exception이 발생하므로 인코딩 해줘야 함.
									http.SaveContentToFile(urlpair.Url, filepath, HttpUtility.UrlEncode(urlpair.RefererUrl));

									ExceptionTester.Instance.Throw("savecontenttofile");
								}
								catch (ThreadAbortException ex2)
								{
									Logger.Warn("	thread{0}: ThreadAbortException raised.", threadId);

									throw;
								}
								catch (Exception ex2)
								{
									Logger.Warn("	thread{0}: exception has occured during saving image. '{1}'", threadId, ex2.Message);

									reportSucceed = false;
								}
							}

							if (reportSucceed)
							{
								Logger.Log("	thread{0}: {1} saved.", threadId, filepath);

								urlpair.Result = UrlPair.SaveResult.Saved;

								lock (lockThis)
								{
									numOfSucceed++;
								}
							}
							else
							{
								Logger.Warn("	thread{0}: {1} save failed.", threadId, filepath);

								urlpair.Result = UrlPair.SaveResult.Failed;

								lock (lockThis)
								{
									numOfFailed++;
								}
							}
						}

						// notify
						lock(lockThis)
						{
							if (Progress != null)
							{
								// this.currentUrlIndex를 사용하지 않는 이유는, 전부 다 돼서 current == url.length인데, 
								// 락에 걸려서 progressbar performstep은 아직 다 안됐을 경우 자연스럽게 보이기 위함.
								Progress(urlpair.Url, reportSucceed, urls.Count);
							}
						}
					}
				}
			}
			catch (ThreadAbortException ex)
			{
				Logger.Warn("thread{0}: ThreadAbortException raised. aborting thread.", threadId);
			}
			catch (Exception ex)
			{
				Logger.Warn("thread{0}: exception has occured in work thread. '{1}'", threadId, ex.Message);
			}
			finally
			{
				Logger.Log("thread{0}: terminated.", threadId);
				this.manualEvents[threadId].Set();
			}
		}

		private Thread mainThread;
		private Thread[] workerThreads;
		private int numOfThreads;
		private ManualResetEvent[] manualEvents;
		private int timeout;

		private List<UrlPair> urls;
		private int currentUrlIndex;
		private int numOfSucceed;
		private int numOfFailed;
		private int numOfDuplicated;
		private object lockThis = new object();

		// options
		private FilePathMaker filePathMaker;
		private string saveFolder;
		private FilePathMaker.FileNameMakingMethod fileNameMakingMethod;
//		private bool overwrite;
		private SaveType saveType;

	}
}
