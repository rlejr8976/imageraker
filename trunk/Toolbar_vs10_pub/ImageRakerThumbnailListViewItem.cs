using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace ImageRakerToolbar
{
	public class ImageRakerThumbnailListViewItem : ThumbnailListViewItem
	{
		public ImageRakerThumbnailListViewItem(string name, IHTMLElementRender render, Bitmap bm, int w, int h, string imgsrc, string refererUrl, int order)
			: base(name, bm)
		{
			int dotPos = name.LastIndexOf('.');

			if (dotPos != -1)
			{
				ext = name.Substring(dotPos + 1).ToLower();
			}
			else
			{
				ext = "";
			}

			this.render = render;
			this.width = w;
			this.height = h;
			this.imgsrc = imgsrc;
			this.refererUrl = refererUrl;
			this.order = order;
		}

		public IHTMLElementRender Render
		{
			get { return render; }
		}

		public string FileExt
		{
			get { return ext; }
		}

		public string ImageSource
		{
			get { return imgsrc; }
		}

		public string RefererUrl
		{
			get { return refererUrl; }
		}

		public int Order
		{
			get { return order; }
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		private IHTMLElementRender render;
		private int width;
		private int height;
		private string ext;
		private string imgsrc;
		private string refererUrl;
		private int order;
	}

	public class ImageRakerThumnbnailListViewItemComparer : IComparer
	{
		public enum CompareMethod
		{
			ByOriginal,
			ByName,
			ByExt,
			BySize
		}

		public enum SortOrder
		{
			Ascending,
			Descending
		}

		public ImageRakerThumnbnailListViewItemComparer()
		{
			this.compareMethod = CompareMethod.ByOriginal;
			this.sortOrder = SortOrder.Ascending;
		}

		public void SetCompareMethod(CompareMethod compareMethod)
		{
			this.compareMethod = compareMethod;
		}

		public void SetSortOrder(SortOrder sortOrder)
		{
			this.sortOrder = sortOrder;
		}

		public CompareMethod GetCompareMethod()
		{
			return compareMethod;
		}

		public SortOrder GetSortOrder()
		{
			return sortOrder;
		}

		public int Compare(object x, object y)
		{
			ImageRakerThumbnailListViewItem a2;
			ImageRakerThumbnailListViewItem b2;

			GetItems(x, y, out a2, out b2);

			switch (compareMethod)
			{
				case CompareMethod.ByOriginal:
					return IntCompare(a2.Order, b2.Order);

				case CompareMethod.ByName:
					return string.Compare(a2.Name, b2.Name);

				case CompareMethod.ByExt:
					return string.Compare(a2.FileExt, b2.FileExt);

				case CompareMethod.BySize:
					{
						int sizea = (a2.Width + a2.Height) / 2;
						int sizeb = (b2.Width + b2.Height) / 2;

						return IntCompare(sizea, sizeb);
					}

				default:
					Logger.Error("unknown compare method: {0}", compareMethod.ToString());
					return -1;
			}
		}

		private int IntCompare(int a, int b)
		{
			if (a < b)
				return -1;
			else if (a > b)
				return 1;
			else
				return 0;
		}

		private void GetItems(object x, object y, out ImageRakerThumbnailListViewItem a, out ImageRakerThumbnailListViewItem b)
		{
			ListViewItem a1 = x as ListViewItem;
			ListViewItem b1 = y as ListViewItem;

			ImageRakerThumbnailListViewItem a2 = a1.Tag as ImageRakerThumbnailListViewItem;
			ImageRakerThumbnailListViewItem b2 = b1.Tag as ImageRakerThumbnailListViewItem;

			a = a2;
			b = b2;

			switch (sortOrder)
			{
				case SortOrder.Ascending:
					a = a2;
					b = b2;
					break;

				case SortOrder.Descending:
					a = b2;
					b = a2;
					break;
			}
		}

		private CompareMethod compareMethod;
		private SortOrder sortOrder;
	}
}
