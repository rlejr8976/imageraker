using System;
using System.Collections.Generic;
using System.Text;

namespace ImageRakerToolbar
{
	public class ImageSelecter
	{
		public ImageSelecter()
			: this(Config.Instance.GetConfigInt("MinSize"), Config.Instance.GetConfigInt("MinSize"))
		{
		}

		public ImageSelecter(int minWidth, int minHeight)
		{
			this.minSelectWidth = minWidth;
			this.minSelectHeight = minHeight;

			minAvgWidth = 100;
			minAvgHeight = 100;
		}

		/// <summary>
		/// 실질적으로 candidateItems중에서 선택한다.
		/// </summary>
		/// <param name="originalItems">페이지의 모든 이미지 아이템</param>
		/// <param name="candidateItems">화면(썸네일 리스트)에 표시중인 아이템</param>
		/// <returns>url, referer pair</returns>
		public Dictionary<string, string> AutoSelect(ImageRakerThumbnailListViewItem[] fullItems, 
			ImageRakerThumbnailListViewItem[] candidateItems, bool ignoreSmallImage)
		{
			Logger.Log("start auto select...");

			Dictionary<string, string> selected = new Dictionary<string, string>();	// selected urls

			if (fullItems.Length == 0)
			{
				Logger.Log("auto select skipped. image items empty.");
				return selected;
			}

			// 최소 크기 이상 이미지들의 평균 w, h를 알아낸다.
			int avgW = 0;
			int avgH = 0;
			int avgCount = 0;

			foreach (ImageRakerThumbnailListViewItem item in fullItems)
			{
				int w = item.Width;
				int h = item.Height;

				if(w < minAvgWidth || h < minAvgHeight)
				{
					continue;
				}

				avgW += w;
				avgH += h;
				avgCount++;
			}

			// div by zero 방지
			if (avgCount == 0)
				avgCount = 1;

			avgW /= avgCount;
			avgH /= avgCount;

			Logger.Log("average width: {0}, height: {1}", avgW, avgH);

			// start auto select
//			double multiplier = 1.37;
			double multiplier = 0.45;
			double multiplierStep = -0.1;
			const double MinMultiplier = 0.2;

			int numOfChecked = 0;

			Logger.Log("auto select start");

			do
			{
				if (multiplier < MinMultiplier)
				{
					Logger.Log("    auto select, canceled: {0}", MinMultiplier);

					break;
				}

				Logger.Log("    auto select, multiplier: {0}", multiplier);

				int minW = (int)(avgW * multiplier);
				int minH = (int)(avgH * multiplier);

				Logger.Log("    multiplier applied width: {0}, height: {1}", minW, minH);

				// 우선 평균 크기가 넘는 이미지들을 선택한다. (크기 선택)
				SortedList<int, int> checkedList = new SortedList<int, int>();

				foreach (ImageRakerThumbnailListViewItem item in candidateItems)
				{
					if (minW < item.Width && minH < item.Height
						&& IsAcceptableSize(item.Width, item.Height, ignoreSmallImage))	// 최소 크기보단 커야 한다.
					{
						if (!selected.ContainsKey(item.ImageSource))
						{
							selected.Add(item.ImageSource, item.RefererUrl);
						}

						checkedList.Add(item.Order, 0);
						numOfChecked++;
					}
				}

				// 그후, 이미지가 html상에 타나는 위치를 파악해서 선택된 이미지의 처음, 마지막 이미지 사이의 모든 이미지를 선택한다. (위치 선택)
				if (checkedList.Count >= 2)
				{
					int first = checkedList.Keys[0];
					int last = checkedList.Keys[checkedList.Count - 1];

					Logger.DLog("    select items by html position. first selected item order: {0}, {1}, second: {2}, {3}",
						first, fullItems[first].Name, last, fullItems[last].Name);

					foreach (ImageRakerThumbnailListViewItem item in candidateItems)
					{
						if (first < item.Order && item.Order < last
							&& IsAcceptableSize(item.Width, item.Height, ignoreSmallImage))	// 최소 크기보단 커야 한다.
						{
							if (!selected.ContainsKey(item.ImageSource))
							{
								selected.Add(item.ImageSource, item.RefererUrl);
							}

							numOfChecked++;
						}
					}
				}

				Logger.Log("    num of checked (without block urls): {0}", numOfChecked);

				multiplier += multiplierStep;
			} while (numOfChecked == 0);	// 하나도 선택되지 않았으면 계속 해.

			// 최종적으로 자동선택에서 제외되야할 블럭url에 대해 작업
			// 하지만 블럭 리스트 때문에 아이템이 하나도 선택되지 않더라도 상관 없다.
			if (Config.Instance.GetConfigBool("UseBlockUrls"))
			{
				Dictionary<string, long> blocks = Config.Instance.GetBlockUrls();

				// checked items에 대해서만.
				foreach (ImageRakerThumbnailListViewItem item in candidateItems)
				{
					if (blocks.ContainsKey(item.ImageSource))
					{
						Logger.DLog("    auto select blocked: {0}", item.ImageSource);

						selected.Remove(item.ImageSource);
					}
				}
			}

			return selected;
		}

		private bool IsAcceptableSize(int w, int h, bool ignoreSmallImage)
		{
			if ((ignoreSmallImage && minSelectWidth < w && minSelectHeight < h)
				|| (!ignoreSmallImage))
			{
				return true;
			}

			return false;
		}

		// 평균 구할때 최소 크기
		private int minAvgWidth;
		private int minAvgHeight;

		// 선택시 최소 크기
		private int minSelectWidth;
		private int minSelectHeight;

	}
}
