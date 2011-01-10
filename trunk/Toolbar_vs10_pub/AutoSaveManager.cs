using System;
using System.Collections.Generic;
using System.Text;
using mshtml;
using System.Windows.Forms;
using System.IO;

namespace ImageRakerToolbar
{
	public class AutoSaveManager
	{
		// returns num of images to save
		public int OnDocumentComplete(IHTMLDocument2 docRoot, ImageRakerDownloadForm.SaveCompleteDelegate saveCompleteDelegate)
		{
			if (docRoot != null)
			{
				// 프레임의 경우 여러 url으로 이루어져있으므로 우선 combined url을 구한다.
				int numOfDocs = 0;
				string combinedUrl = GetCombinedUrl(docRoot, out numOfDocs);

				Logger.Log("autosavemanager - doc complete - numofdocs: {0}, combined url: {1}", numOfDocs, combinedUrl);

				// check visited
				if (savedPages.ContainsKey(combinedUrl))
				{
					// already visited, skip
					Logger.Warn("already saved in this url: {0}", combinedUrl);

					// 이미 방문한 경우... 
					saveCompleteDelegate(ImageRakerDownloadForm.SaveCompleteState.AlreadySaved, 0, 0, 0, 0, 0);

					return 0;
				}

				savedPages.Add(combinedUrl, 0);

				ImgSrcExtractor extractor = new ImgSrcExtractor();
				extractor.Extract(docRoot);
				ImageRakerThumbnailListViewItem[] items = extractor.GetItems();

				ImageSelecter selecter = new ImageSelecter();
				Dictionary<string, string> selected = selecter.AutoSelect(items, items, true);	// url, referer

				//List<UrlPair> urlpairs = new List<UrlPair>();
				urlPairs.Clear();

				// make url pair
				foreach (KeyValuePair<string, string> sel in selected)
				{
					string url = sel.Key;
					string referer = sel.Value;

					// check img url duplication
					if(!savedUrls.ContainsKey(url))
					{
						urlPairs.Add(new UrlPair(url, referer));

						savedUrls.Add(url, 0);
					}
				}

				if (urlPairs.Count > 0)
				{
					Logger.Info("auto save manager - starting ir downloadform...");

					UsageReporter.Instance.FormLoadTime = -1;
					UsageReporter.Instance.SaveCountInSession = -1;

					FilePathMaker.FileNameMakingMethod fileNameMakingMethod = FilePathMaker.GetFileNameMakingMethodFromConfig();

					downloadForm = new ImageRakerDownloadForm(saveFolder, urlPairs,
															 fileNameMakingMethod, true, ImageRaker.SaveType.ByAuto);

//					downloadForm.TopMost = true;	// 사용할 수 없음.
					downloadForm.WindowState = FormWindowState.Minimized;
//					downloadForm.Opacity = 0.65;
					downloadForm.StartPosition = FormStartPosition.CenterScreen;
					downloadForm.SaveComplete += saveCompleteDelegate;

					downloadForm.Show();

					Logger.Log("auto save complete asynchronously");

					return urlPairs.Count;
				}
				else
				{
					Logger.Info("nothing to auto save!");

					saveCompleteDelegate(ImageRakerDownloadForm.SaveCompleteState.NothingToSave, 0, 0, 0, 0, 0);

					return 0;
				}
			}
			else
			{
				Logger.DLog("invalid doc!");
			}

			return 0;
		}

		public void CancelSaving()
		{
			Logger.Log("cancel saving in auto mode");

			if(downloadForm != null)
			{
				downloadForm.CancelSaving();
			}
		}

		public void DeleteSaved()
		{
			Logger.Log("delete saved in auto mode");

			if (downloadForm != null)
			{
				downloadForm.DeleteSaved();
			}
		}


		public static string GetCombinedUrl(IHTMLDocument2 docRoot, out int numOfDocs)
		{
			SortedDictionary<string, int> pageUrls = new SortedDictionary<string, int>();	// 중복삽입을 피하기 위해.
			string combinedUrl = "";

			IHTMLDocument2[] docs = ImgSrcExtractor.GetHtmlDocumentsByOle(docRoot);

			foreach (IHTMLDocument2 doc in docs)
			{
//				Logger.DLog("page url found: {0}", doc.url);

				if (doc.url != "about:blank")
				{
					if(!pageUrls.ContainsKey(doc.url))
					{
						pageUrls.Add(doc.url, 0);
					}
				}
			}

			// combined url은 중복 삭제, 정렬 후 만든다.
			foreach(KeyValuePair<string, int> url in pageUrls)
			{
				combinedUrl += url.Key;
			}

			numOfDocs = pageUrls.Count;

			// 더이상 사용하지 않는다...
			numOfDocs = numOfDocs * 4 / 5;

			return combinedUrl;
		}

		public string CurrentCombinedUrl
		{
			get { return currentCombinedUrl; }
		}

		// 프레임에서 페이지 로드 완료를 정확히 파악하기 위해서... -> 필요 없어졌음.
		private string currentCombinedUrl = "";
//		private int numOfDocuments = 0;
//		private int numOfLoaded = 0;

		private ImageRakerDownloadForm downloadForm = null;
		private string saveFolder = Config.Instance.GetConfig("SaveFolder");

		private Dictionary<string, int> savedPages = new Dictionary<string, int>();		// int not used
		private Dictionary<string, int> savedUrls = new Dictionary<string, int>();		// int not used
		private List<UrlPair> urlPairs = new List<UrlPair>();

	}
}
