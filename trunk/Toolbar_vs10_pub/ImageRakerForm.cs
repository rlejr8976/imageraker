using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace ImageRakerToolbar
{
	public partial class ImageRakerForm : Form
	{
		public ImageRakerForm(Toolbar toolbar, IHTMLDocument2 rootDoc, bool instantMode)
		{
			InitializeComponent();

#if DEBUG
			this.Text = "ImageRaker DEBUG";
#endif

			thumbnailListView.ListViewItemSorter = comparer;

			this.toolbar = toolbar;
			this.rootDoc = rootDoc;
			this.isThumbnailListViewUpdatable = false;
			this.instantMode = instantMode;
			this.saveCount = 0;
		}

		private void ImageRakerForm_Load(object sender, EventArgs e)
		{
			Logger.Log("main form loaded");

			ExceptionTester.Instance.Throw("random_mainformload");

			// set default ui
			LoadSettingFromConfig();

			// thumbnail size
			thumbnailListView.ThumbnailSize = Config.Instance.GetConfigInt("ThumbnailSize");

			Logger.Log("thumbnail size set to: {0}", thumbnailListView.ThumbnailSize);

			MakeThumbnailListViewImages(rootDoc);

			// 이제 썸네일 리스트뷰 업데이트가 가능하다.
			this.isThumbnailListViewUpdatable = true;

			UpdateThumbnailListView();

			ValidateButtons();

			if(instantMode)
			{
				Logger.Log("main form opened in instant mode. click save button.");

				saveButton.PerformClick();
			}
			else
			{
				CheckForUpdate();
			}
		}

		private void CheckForUpdate()
		{
			updateChecker.CheckForUpdate(this, false, false);
		}

		private void ImageRakerForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			GC.Collect();

			Logger.Log("Imageraker form closed");
		}

		private void ValidateButtons()
		{
			bool enable = true;

			if (thumbnailListView.Items.Count == 0)
			{
				enable = false;
			}

			selectAllButton.Enabled = enable;
			deselectAllButton.Enabled = enable;
			autoSelectButton.Enabled = enable;

			// savebutton은 선택된 아이템에 따라서 또 달라지므로 아래에서 추가로 판단.
			saveButton.Enabled = enable;

			// 혹시 아무것도 체크되지 않았을 경우를 위해. validate 후에 해야한다.
			thumbnailListView_ItemChecked(null, null);
		}

		private void LoadSettingFromConfig()
		{
			// form size
			int width = Config.Instance.GetConfigInt("FormWidth");
			int height = Config.Instance.GetConfigInt("FormHeight");

			Size = new Size(width, height);

			// save folder
			saveFolderTextBox.Text = Config.Instance.GetConfig("SaveFolder");

			// compare method
			string byOriginal = ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByOriginal.ToString();
			string byName = ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByName.ToString();
			string byExt = ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByExt.ToString();
			string bySize = ImageRakerThumnbnailListViewItemComparer.CompareMethod.BySize.ToString();

			string compareMethod = Config.Instance.GetConfig("CompareMethod");

			if(compareMethod == byOriginal)
			{
				byOriginalRadioButton.Checked = true;
			}
			else if(compareMethod == byName)
			{
				byNameRadioButton.Checked = true;
			}
			else if(compareMethod == byExt)
			{
				byExtRadioButton.Checked = true;
			}
			else if(compareMethod == bySize)
			{
				bySizeRadioButton.Checked = true;
			}
			else
			{
				Logger.Warn("Cannot recognize compare method config value {0}. default value used.", compareMethod);
				byOriginalRadioButton.Checked = true;
			}

			// sort order
			string ascOrder = ImageRakerThumnbnailListViewItemComparer.SortOrder.Ascending.ToString();
			string descOrder = ImageRakerThumnbnailListViewItemComparer.SortOrder.Descending.ToString();

			string sortOrder = Config.Instance.GetConfig("SortOrder");

			if(sortOrder == ascOrder)
			{
				ascOrderRadioButton.Checked = true;
			}
			else if(sortOrder == descOrder)
			{
				descOrderRadioButton.Checked = true;
			}
			else
			{
				Logger.Warn("Cannot recognize sort order config value {0}. default value used.", sortOrder);
				ascOrderRadioButton.Checked = true;
			}

			// filters
			string jpg = Config.Instance.GetConfig("JpgFilterChecked");
			string gif = Config.Instance.GetConfig("GifFilterChecked");
			string png = Config.Instance.GetConfig("PngFilterChecked");
			string bmp = Config.Instance.GetConfig("BmpFilterChecked");
			string etc = Config.Instance.GetConfig("EtcFilterChecked");
			string showSmalls = Config.Instance.GetConfig("ShowSmallsChecked");

			if(jpg == true.ToString())
			{
				jpgCheckBox.Checked = true;
			}

			if (gif == true.ToString())
			{
				gifCheckBox.Checked = true;
			}

			if (png == true.ToString())
			{
				pngCheckBox.Checked = true;
			}

			if (bmp == true.ToString())
			{
				bmpCheckBox.Checked = true;
			}

			if (etc == true.ToString())
			{
				etcCheckBox.Checked = true;
			}

			if(showSmalls == true.ToString())
			{
				showSmallsCheckBox.Checked = true;
			}
		}

		private bool GetPrimaryScreenSize(out int screenW, out int screenH)
		{
			Screen[] screens = Screen.AllScreens;
			int upperBound = screens.GetUpperBound(0);
			//Rectangle screenRect = new Rectangle();
			Rectangle? screenRect = null;

			for (int index = 0; index <= upperBound; index++)
			{
				if (screens[index].Primary)
				{
					screenRect = screens[index].Bounds;
				}
			}

			if (screenRect == null)
			{
				Logger.Error("cannot retreive primary screen rect!");

				screenW = 1000;
				screenH = 1000;

				return false;
			}

			screenW = screenRect.Value.Width;
			screenH = screenRect.Value.Height;

			Logger.Info("primary screen size: {0} * {1}", screenW, screenH);

			return true;
		}

		private void MakeThumbnailListViewImages(IHTMLDocument2 rootDoc)
		{
			Logger.Log("Begin making thumbnail list view images...");

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			// set default screen value
			int screenW = 0;
			int screenH = 0;

			GetPrimaryScreenSize(out screenW, out screenH);

			// get thumbnail quality
			string thumbQuality = Config.Instance.GetConfig("ThumbnailQuality");
			InterpolationMode im = InterpolationMode.Low;

			switch(thumbQuality)
			{
				case "High":
					im = InterpolationMode.High;
					break;

				case "Low":
					im = InterpolationMode.Low;
					break;
			}

			ImgSrcExtractor extractor = new ImgSrcExtractor();
			extractor.Extract(rootDoc);
			ImageRakerThumbnailListViewItem[] items = extractor.GetItems();

			// initialize
			imageItems.Clear();

			foreach(ImageRakerThumbnailListViewItem item in items)
			{
				try
				{
					ExceptionTester.Instance.Throw("makethumbnail");
					int orgW = item.Width;
					int orgH = item.Height;

					int adjW = Math.Min(orgW, screenW);
					int adjH = Math.Min(orgH, screenH);

					if (orgW == 0 || orgH == 0)
					{
//						Logger.DLog("	skip image filename {0}. width or height is zero. {1}*{2}", item.Name, orgW, orgH);
						continue;
					}

					// make small bitmap.
					// 비율에 맞게 조절해야 함.
					int toSize = thumbnailListView.ThumbnailSize;
					float scale = 1;

					if (adjW > toSize)
					{
						scale = (float)toSize / adjW;
					}

					if (adjH > toSize)
					{
						scale = (float)toSize / adjH;
					}

					int thumbW = (int)(adjW * scale);
					int thumbH = (int)(adjH * scale);

					if (thumbW == 0 || thumbH == 0)
					{
//						Logger.DLog("	skip image filename {0}. thumbnail width or height is zero. {1}*{2}", item.Name, thumbW, thumbH);
						continue;
					}

					Bitmap thumbBitmap = new Bitmap(thumbW, thumbH);

					// make original bitmap
					using (Bitmap originalBitmap = new Bitmap(adjW, adjH))
					{
						try
						{
							using (Graphics g = Graphics.FromImage(originalBitmap))
							{
								ExceptionTester.Instance.Throw("htmlelemdrawing");

								item.Render.DrawToDC(g.GetHdc());
							}
						}
						catch (Exception ex)
						{
							Logger.Warn("unknown error in html element drawing!");
						}

						using (Graphics smallG = Graphics.FromImage(thumbBitmap))
						{
							smallG.PixelOffsetMode = PixelOffsetMode.None;
							//smallG.InterpolationMode = InterpolationMode.High;
							smallG.InterpolationMode = im;

							smallG.DrawImage(originalBitmap, 0, 0, thumbW, thumbH);
						}
					}

					// set thumbnail
					item.Bitmap = thumbBitmap;

					// and add to list
					imageItems.Add(item);
				}
				catch (Exception ex)
				{
					Logger.Warn("unknown error in MakeThumbnailListViewImages!");
				}
			}

			// 별 관련은 없어보인다.
			GC.Collect();

			stopWatch.Stop();

			UsageReporter.Instance.FormLoadTime = (int)stopWatch.ElapsedMilliseconds;

			Logger.Log("Making thumbnail list view images done. elapsed: {0}", stopWatch.ElapsedMilliseconds);
		}
			 

		private void saveButton_Click(object sender, EventArgs e)
		{
			string saveFolder = Config.Instance.GetConfig("SaveFolder");

			/*
			 * save by gdi+
			 * 모니터 해상도 크기 이상의 이미지는 저장되지 않는다.
			 */

			/*
			Logger.Log("save images by gdi+");
			Stopwatch sw = new Stopwatch();
			sw.Start();

			FilePathMaker filePathMaker = new FilePathMaker(saveFolder);

			foreach(ListViewItem item in thumbnailListView.CheckedItems)
			{
				ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;
				//string src = myitem.ImageSource;
				//string referer = myitem.RefererUrl;

				//urls.Add(new UrlPair(src, referer));

				using (Bitmap originalBitmap = new Bitmap(myitem.Width, myitem.Height))
				{
					using (Graphics g = Graphics.FromImage(originalBitmap))
					{
						myitem.Render.DrawToDC(g.GetHdc());
					}

					Logger.DLog("    save to file: {0} * {1}, url: {2}", myitem.Width, myitem.Height, myitem.ImageSource);

					string filepath = filePathMaker.MakeFilePath(myitem.ImageSource, fileNameMakingMethod);

					originalBitmap.Save(filepath);
				}
			}

			sw.Stop();
			Logger.Log("save images by gdi+ DONE - elapsed: {0}", sw.ElapsedMilliseconds);

			return;
			 */

			// 자동선택됐지만 선택 해제된 아이템들을 블럭. 다시 선택된 애들은 블럭 리스트에서 삭제.
			// 최초 저장시만 사용한다. 두번째 저장부터는 실패한 것일 수 있음.
			if(Config.Instance.GetConfigBool("UseBlockUrls") && saveCount == 0)
			{
				Dictionary<string, long> blockUrls = Config.Instance.GetBlockUrls();
				bool blockUrlsChanged = false;

				foreach (ImageRakerThumbnailListViewItem autoitem in originallyAutoSelectedImageItems)
				{
					string autourl = autoitem.ImageSource;

					foreach (ListViewItem item in thumbnailListView.Items)
					{
						ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;

						if (myitem.ImageSource == autourl)
						{
							if (item.Checked == false)
							{
								if (!blockUrls.ContainsKey(autourl))
								{
									Logger.DLog("auto selected but unchecked, not added in blocks yet: {0}, ADD TO BLOCK URLS", autourl);

									Config.Instance.AddBlockUrl(autourl);
									blockUrlsChanged = true;
								}
							}
							else
							{
								if (blockUrls.ContainsKey(autourl))
								{
									Logger.DLog("originally auto selected, blocked, but checked again: {0}, REMOVE FROM BLOCK URLS", autourl);

									Config.Instance.RemoveBlockUrl(autourl);
									blockUrlsChanged = true;
								}
							}
						}
					}
				}

				if (blockUrlsChanged)
				{
					Config.Instance.SaveBlockUrls();
				}
			}

			ExceptionTester.Instance.Throw("random_save1");

			// extract urls to save
			List<UrlPair> urls = new List<UrlPair>();

			foreach (ListViewItem item in thumbnailListView.CheckedItems)
			{
				ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;
				string src = myitem.ImageSource;
				string referer = myitem.RefererUrl;

				urls.Add(new UrlPair(src, referer));
			}

			// determine selected file name making method
			//FilePathMaker.FileNameMakingMethod fileNameMakingMethod = FilePathMaker.FileNameMakingMethod.FullUrl;
			FilePathMaker.FileNameMakingMethod fileNameMakingMethod = FilePathMaker.GetFileNameMakingMethodFromConfig();
			bool exitOnComplete = Config.Instance.GetConfigBool("ExitOnComplete");
//			bool overwrite = Config.Instance.GetConfigBool("Overwrite");

			UsageReporter.Instance.SaveCountInSession = saveCount++;

			ExceptionTester.Instance.Throw("random_save2");

			ImageRaker.SaveType saveType = ImageRaker.SaveType.ByForm;

			if(instantMode)
			{
				saveType = ImageRaker.SaveType.ByInstant;
			}

			ImageRakerDownloadForm form = new ImageRakerDownloadForm(saveFolder, urls,
																	 fileNameMakingMethod, exitOnComplete, saveType);

			DialogResult result = form.ShowDialog();

			ExceptionTester.Instance.Throw("random_save3");

			if (result != DialogResult.Abort)
			{
				bool failed = false;

				// 실패한 이미지 체크 기능
				if (Config.Instance.GetConfigBool("MarkFailed") == true)
				{
					// reset checked first
					deselectAllButton.PerformClick();

					// check on failed urls
					Logger.Log("check on failed urls...");

					foreach (UrlPair url in urls)
					{
						if(url.Result == UrlPair.SaveResult.Failed
							|| url.Result == UrlPair.SaveResult.Duplicated)
						{
							foreach (ListViewItem item in thumbnailListView.Items)
							{
								ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;

								if (myitem.ImageSource == url.Url)
								{
									failed = true;
									item.Checked = true;
								}
							}
						}
					}
				}

				// exit when exit on complete enabled.
				if (!failed && (exitOnComplete && (result == DialogResult.OK || result == DialogResult.Cancel)))
				{
					// exit the program
					this.Close();
				}
			}
			else
			{
				Logger.Warn("cannot start downloader! reason: {0}", form.ExceptionMessage);
				Message.Warn("이미지 레이커 다운로더를 실행할 수 없습니다.\n" + form.ExceptionMessage);

				//MessageBox.Show("이미지 레이커 다운로더를 실행할 수 없습니다.\n" + form.ExceptionMessage, 
				//    About.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void selectAllButton_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in thumbnailListView.Items)
			{
				item.Checked = true;
			}
		}

		private void deselectAllButton_Click(object sender, EventArgs e)
		{
			foreach(ListViewItem item in thumbnailListView.Items)
			{
				item.Checked = false;
			}
		}

		private void autoSelectButton_Click(object sender, EventArgs e)
		{
			// get displaying items
			List<ImageRakerThumbnailListViewItem> displayingItems = new List<ImageRakerThumbnailListViewItem>();

			foreach (ListViewItem item in thumbnailListView.Items)
			{
				ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;
				displayingItems.Add(myitem);
			}

			bool ignoreSmall = !showSmallsCheckBox.Checked;

			ImageSelecter selecter = new ImageSelecter();
			Dictionary<string, string> selected = selecter.AutoSelect(imageItems.ToArray(), displayingItems.ToArray(), ignoreSmall);

			// select items
			originallyAutoSelectedImageItems.Clear();

			foreach (ListViewItem item in thumbnailListView.Items)
			{
				ImageRakerThumbnailListViewItem myitem = item.Tag as ImageRakerThumbnailListViewItem;

				if(selected.ContainsKey(myitem.ImageSource))
				{
					item.Checked = true;

					originallyAutoSelectedImageItems.Add(myitem);
				}
				else
				{
					item.Checked = false;
				}
			}
		}

		private void SortThumbnailListView(ImageRakerThumnbnailListViewItemComparer.CompareMethod method, bool update)
		{
			if (update)
			{
				comparer.SetCompareMethod(method);
				thumbnailListView.Sort();

				string compareMethod = comparer.GetCompareMethod().ToString();
				Config.Instance.SetConfig("CompareMethod", compareMethod);
			}
		}

		private void byOriginalRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			SortThumbnailListView(ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByOriginal, byOriginalRadioButton.Checked);
		}

		private void byNameRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			SortThumbnailListView(ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByName, byNameRadioButton.Checked);
		}

		private void byExtRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			SortThumbnailListView(ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByExt, byExtRadioButton.Checked);
		}

		private void bySizeRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			SortThumbnailListView(ImageRakerThumnbnailListViewItemComparer.CompareMethod.BySize, bySizeRadioButton.Checked);
		}

		private void UpdateThumbnailListViewSortOrder(ImageRakerThumnbnailListViewItemComparer.SortOrder order, bool update)
		{
			if (update)
			{
				comparer.SetSortOrder(order);
				thumbnailListView.Sort();

				string sortOrder = comparer.GetSortOrder().ToString();
				Config.Instance.SetConfig("SortOrder", sortOrder);
			}
		}

		private void ascOrderRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			UpdateThumbnailListViewSortOrder(ImageRakerThumnbnailListViewItemComparer.SortOrder.Ascending, ascOrderRadioButton.Checked);
		}

		private void descOrderRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			UpdateThumbnailListViewSortOrder(ImageRakerThumnbnailListViewItemComparer.SortOrder.Descending, descOrderRadioButton.Checked);
		}

		private void browseFolderButton_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath = saveFolderTextBox.Text;

			if (DialogResult.OK == folderBrowserDialog.ShowDialog())
			{
				saveFolderTextBox.Text = folderBrowserDialog.SelectedPath;
				Config.Instance.SetConfig("SaveFolder", saveFolderTextBox.Text);
			}
		}

		private void thumbnailListView_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			bool enable = true;

			try
			{
				if (thumbnailListView.CheckedItems.Count == 0)
				{
					enable = false;
				}
			}
			catch (Exception)
			{
			}

			saveButton.Enabled = enable;

			UpdateNumOfSelectedLabel();
		}

		private void UpdateNumOfSelectedLabel()
		{
			int numSelected = thumbnailListView.CheckedItems.Count;

//			Logger.DLog("{0} image selected", numSelected);
 
			numOfSelectedLabel.Text = string.Format("{0}개 선택됨", numSelected);
		}

		private void ImageRakerForm_ResizeEnd(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("FormWidth", this.Size.Width.ToString());
			Config.Instance.SetConfig("FormHeight", this.Size.Height.ToString());
		}

		private void optionButton_Click(object sender, EventArgs e)
		{
			OptionForm of = new OptionForm();

			of.ShowDialog();

			toolbar.UpdateOtherSaveButtonState();
		}	

		private void UpdateThumbnailListView()
		{
			if (!isThumbnailListViewUpdatable)
			{
				Logger.Warn("Cannot update thumbnail list view.");

				return;
			}

			try
			{
				Logger.Log("Updating thumnbnail list view...");

				thumbnailListView.Clear();

				ImageRakerThumbnailListViewItem[] items = GetFilteredThumbnailListViewItems();

				ExceptionTester.Instance.Throw("updatethumbnaillistview");

				thumbnailListView.LoadItems(items);

				ValidateButtons();

				// auto select
				autoSelectButton.PerformClick();
			}
			catch (Exception ex)
			{
				Logger.Warn("cannot update thumbnail list view - {0}", ex.Message);
				Message.Warn("이미지를 정상적으로 표시할 수 없습니다.");

//				UsageReporter.Instance.ShowAlertMessage("이미지를 정상적으로 표시할 수 없습니다.");
			}
		}

		private ImageRakerThumbnailListViewItem[] GetFilteredThumbnailListViewItems()
		{
			List<ImageRakerThumbnailListViewItem> items = new List<ImageRakerThumbnailListViewItem>();

			Logger.Log("Get filtered thumbnail list view items.");

			int minW = Config.Instance.GetConfigInt("MinSize");
			int minH = Config.Instance.GetConfigInt("MinSize");

			foreach (ImageRakerThumbnailListViewItem item in imageItems)
			{
				//Logger.Log("	name: {0}, ext: {1}", item.Name, item.Ext);

				if ((jpgCheckBox.Checked && item.FileExt == "jpg") ||
					(gifCheckBox.Checked && item.FileExt == "gif") ||
					(pngCheckBox.Checked && item.FileExt == "png") ||
					(bmpCheckBox.Checked && item.FileExt == "bmp") ||
					(etcCheckBox.Checked &&	!(item.FileExt == "jpg" || item.FileExt == "gif" || item.FileExt == "png" || item.FileExt == "bmp")))
				{
					Logger.DLog("	name: {0}, ext: {1}, size: {2}*{3} added to items", item.Name, item.FileExt, item.Width, item.Height);

					if (showSmallsCheckBox.Checked
						|| (!showSmallsCheckBox.Checked && (item.Width >= minW && item.Height >= minH)))
					{
						items.Add(item);
					}
				}

				//else if (etcCheckBox.Checked && 
				//    !(item.FileExt == "jpg" || item.FileExt == "gif" || item.FileExt == "png" || item.FileExt == "bmp"))
				//{
				//    Logger.DLog("	name: {0}, ext: {1}, size: {2}*{3} added to items by allowing ETC", item.Name, item.FileExt, item.Width, item.Height);

				//    items.Add(item);
				//}
			}

			return items.ToArray();
		}

		private void jpgCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("JpgFilterChecked", jpgCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private void gifCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("GifFilterChecked", gifCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private void pngCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("PngFilterChecked", pngCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private void bmpCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("BmpFilterChecked", bmpCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private void etcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("EtcFilterChecked", etcCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private void showSmallsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Config.Instance.SetConfig("ShowSmallsChecked", showSmallsCheckBox.Checked.ToString());

			UpdateThumbnailListView();
		}

		private IHTMLDocument2 rootDoc = null;
		private Toolbar toolbar;
		private bool instantMode;
		private int saveCount = 0;

		private List<ImageRakerThumbnailListViewItem> imageItems = new List<ImageRakerThumbnailListViewItem>();
		private List<ImageRakerThumbnailListViewItem> originallyAutoSelectedImageItems = new List<ImageRakerThumbnailListViewItem>();

		private ImageRakerThumnbnailListViewItemComparer comparer = new ImageRakerThumnbnailListViewItemComparer();
		private bool isThumbnailListViewUpdatable;	// 폼 로드시 filter checkbox에 의해 update가 여러번 되지 않도록 하는 플래그.

		private UpdateChecker updateChecker = new UpdateChecker();


	}

}