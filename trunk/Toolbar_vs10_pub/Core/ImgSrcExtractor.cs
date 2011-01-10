using System;
using System.Collections.Generic;
using System.Text;
using mshtml;
using System.Runtime.InteropServices;

namespace ImageRakerToolbar
{
	/// <summary>
	/// extract img src from IHTMLDocument2
	/// </summary>
	public class ImgSrcExtractor
	{
		public ImgSrcExtractor()
		{
		}

		public void Extract(IHTMLDocument2 rootDoc)
		{
			Logger.Log("starting img src extractor...");

			items = new List<ImageRakerThumbnailListViewItem>();
			imageDic = new Dictionary<string, IHTMLElement>();

			IHTMLDocument2[] docs = GetHtmlDocumentsByOle(rootDoc);
			int index = 0;

			foreach (IHTMLDocument2 d in docs)
			{
				IHTMLElementCollection images = d.images;

				for (int i = 0; i < images.length; i++)
				{
					try
					{
						ExceptionTester.Instance.Throw("makethumbnail");

						IHTMLElement elem = images.item(i, i) as IHTMLElement;

						string src = elem.getAttribute("src", 0).ToString();

						// 한번 추가된 이미지는 다시 추가하지 않는다.
						if (!imageDic.ContainsKey(src))
						{
							IHTMLElementRender render = elem as IHTMLElementRender;

							string filename = src.Substring(src.LastIndexOf('/') + 1);
							int orgW = elem.offsetWidth;
							int orgH = elem.offsetHeight;

							if (orgW == 0 || orgH == 0)
							{
//								Logger.DLog("	skip image filename {0}. width or height is zero. {1}*{2}", filename, orgW, orgH);
								continue;
							}

							// thumbnail은 null...
							ImageRakerThumbnailListViewItem item = new ImageRakerThumbnailListViewItem(
								filename, render, null, orgW, orgH, src, d.url, index++);

							Logger.DLog("img src extractor - img found - filename: {0}, size: {1}*{2}, url: {3}",
								filename, orgW, orgH, src);

							items.Add(item);
							imageDic.Add(src, elem);
						}

					}
					catch (Exception ex)
					{
						Logger.Warn("unknown error in MakeThumbnailListViewImages!");
					}
				}
			}

			Logger.Info("img src extractor found {0} images", items.Count);
		}

		public ImageRakerThumbnailListViewItem[] GetItems()
		{
			return items.ToArray();
		}

		// document를 일반적으로 가져올 수 없는 경우 (security issue) 이렇게 가져옴.
		public static IHTMLDocument2[] GetHtmlDocumentsByOle(IHTMLDocument2 doc)
		{
			try
			{
				ExceptionTester.Instance.Throw("gethtmlbyole");

				List<IHTMLDocument2> docs = new List<IHTMLDocument2>();

				// add main doc
				docs.Add(doc);

				//get the OLE container from the window's parent
				IOleContainer oc = doc as IOleContainer;      //OC ALLOC

				//get the OLE enumerator for the embedded objects
				int hr = 0;
				IEnumUnknown eu;
				hr = oc.EnumObjects(tagOLECONTF.OLECONTF_EMBEDDINGS, out eu); //EU ALLOC

				Guid IID_IWebBrowser2 = typeof(SHDocVw.IWebBrowser2).GUID;
				object pUnk = null;
				int fetched = 0;
				const int MAX_FETCH_COUNT = 1;

				for (int i = 0; eu.Next(MAX_FETCH_COUNT, out pUnk, out fetched) == hr; i++)
				{
					//QI pUnk for the IWebBrowser2 interface
					SHDocVw.IWebBrowser2 brow = pUnk as SHDocVw.IWebBrowser2;

					if (brow != null)
					{
						IHTMLDocument2 d = brow.Document as IHTMLDocument2;
						//	Logger.Log("wb found. doc's url: {0}", d.url);

						// if we found document, repeat recursively
						IHTMLDocument2[] docs2 = GetHtmlDocumentsByOle(d);

						foreach (IHTMLDocument2 doc2 in docs2)
						{
							docs.Add(doc2);
						}
					}
				}

				Marshal.ReleaseComObject(eu);                                 //EU FREE

				return docs.ToArray();
			}
			catch (Exception e)
			{
				Logger.Warn("cannt get html doc by ole! {0}", e.Message);

				return new IHTMLDocument2[] { };
			}
		}


		private List<ImageRakerThumbnailListViewItem> items;
		private Dictionary<string, IHTMLElement> imageDic;		// 중복 추가 방지

	}
}
