using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ImageRakerToolbar
{
	public partial class ThumbnailListView : ListView
	{
		private BackgroundWorker thumbnailLoadWorker = new BackgroundWorker();

		public event EventHandler OnLoadComplete;

		public int ThumbnailSize
		{
			get { return thumbnailSize; }
			set { thumbnailSize = value; }
		}

		public Color ThumbBorderColor
		{
			get { return thumbBorderColor; }
			set { thumbBorderColor = value; }
		}

		public bool IsLoading
		{
			get { return thumbnailLoadWorker.IsBusy; }
		}

		public ThumbnailListView()
		{
			InitializeComponent();

			components.Add(thumbnailLoadWorker);

			thumbnailLoadWorker.WorkerSupportsCancellation = true;
			thumbnailLoadWorker.DoWork += new DoWorkEventHandler(thumbnailLoadWorker_DoWork);
			thumbnailLoadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(thumbnailLoadWorker_RunWorkerCompleted);
		}

		public void LoadItems(ThumbnailListViewItem[] items)
		{
			if ((thumbnailLoadWorker != null) && (thumbnailLoadWorker.IsBusy))
				thumbnailLoadWorker.CancelAsync();

			ImageList il = new ImageList();
			il.ImageSize = new Size(thumbnailSize, thumbnailSize);
			il.ColorDepth = ColorDepth.Depth32Bit;
			il.TransparentColor = Color.White;

			LargeImageList = il;

			BeginUpdate();

			Items.Clear();
			LargeImageList.Images.Clear();

			AddDefaultThumbnail();

			foreach (ThumbnailListViewItem item in items)
			{
				ListViewItem lvItem = new ListViewItem(item.Name);
				lvItem.ImageIndex = 0;
				lvItem.Tag = item;

				Items.Add(lvItem);
			}

			EndUpdate();

			if (thumbnailLoadWorker != null)
			{
				// 참고:
				// backgroundworker가 cancel되도록 되어있지만 실질적으로 runworkercompleted가 호출되기 전까지는 
				// 아무것도 할 수 없는 상태(pending)이다. CancellationPending된 상태라서 이 상태에서는 
				// backgroundworker는 동작하지 않는다. (completed가 호출되기 전 까지)
				if (!thumbnailLoadWorker.CancellationPending)
					thumbnailLoadWorker.RunWorkerAsync(items);
			}
		}

		public static Image GetThumbnail(Bitmap bmp, int thumbWidth, int thumbHeight, Color penColor)
		{
			int imgWidth = thumbWidth;
			int imgHeight = thumbHeight;

			if (bmp.Width < imgWidth && bmp.Height < imgHeight)
			{
				imgWidth = bmp.Width;
				imgHeight = bmp.Height;
			}

			Bitmap retBmp = new Bitmap(thumbWidth, thumbHeight);
			//Bitmap retBmp = new Bitmap(thumbWidth, thumbHeight, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);

			Graphics grp = Graphics.FromImage(retBmp);

			int tnWidth = imgWidth, tnHeight = imgHeight;

			if (bmp.Width > bmp.Height)
				tnHeight = (int)(((float)bmp.Height / (float)bmp.Width) * tnWidth);
			else if (bmp.Width < bmp.Height)
				tnWidth = (int)(((float)bmp.Width / (float)bmp.Height) * tnHeight);

			int iLeft = (thumbWidth / 2) - (tnWidth / 2);
			int iTop = (thumbHeight / 2) - (tnHeight / 2);

			grp.PixelOffsetMode = PixelOffsetMode.None;
			//grp.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grp.InterpolationMode = InterpolationMode.High;

			grp.DrawImage(bmp, iLeft, iTop, tnWidth, tnHeight);

			Pen pn = new Pen(penColor, 1); //Color.Wheat
			grp.DrawRectangle(pn, 0, 0, retBmp.Width - 1, retBmp.Height - 1);

			return retBmp;
		}

		private Image GetThumbnail(Bitmap bmp)
		{
			return GetThumbnail(bmp, thumbnailSize, thumbnailSize, thumbBorderColor);
		}

		private void AddDefaultThumbnail()
		{
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(LargeImageList.ImageSize.Width, LargeImageList.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			Graphics grp = Graphics.FromImage(bmp);
			Brush brs = new SolidBrush(Color.White);
			Rectangle rect = new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1);
			grp.FillRectangle(brs, rect);
			Pen pn = new Pen(Color.Wheat, 1);

			grp.DrawRectangle(pn, 0, 0, bmp.Width - 1, bmp.Height - 1);
			LargeImageList.Images.Add(bmp);
		}

		private delegate void AddThumbnailDelegate(ThumbnailListViewItem item);
		private void AddThumbnail(ThumbnailListViewItem item)
		{
			if (Disposing) return;

			if (this.InvokeRequired)
			{
				AddThumbnailDelegate d = new AddThumbnailDelegate(AddThumbnail);
				this.Invoke(d, new object[] { item });
			}
			else
			{
				Image image = GetThumbnail(item.Bitmap);

				LargeImageList.Images.Add(image); //Images[i].repl  

				int imageIndex = LargeImageList.Images.Count - 1;
				int itemIndex = 0;

				// 이 태그에 맞는 아이템을 찾자
				for (int i = 0; i < Items.Count; i++ )
				{
					if (Items[i].Tag == item)
					{
						itemIndex = i;
					}
				}

				Items[itemIndex].ImageIndex = imageIndex;

				//Logger.Log("item {0}, name {1}, image {2}", itemIndex, item.Name, imageIndex);
			}
		}

		private void thumbnailLoadWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			if (thumbnailLoadWorker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}

			ThumbnailListViewItem[] items = (ThumbnailListViewItem[])e.Argument;

			foreach (ThumbnailListViewItem item in items)
			{
				if (thumbnailLoadWorker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}

				AddThumbnail(item);
			}
		}

		private void thumbnailLoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (OnLoadComplete != null)
				OnLoadComplete(this, new EventArgs());
		}

		private int thumbnailSize = 95;
		private Color thumbBorderColor = Color.Wheat;

	}

	public class ThumbnailListViewItem
	{
		public ThumbnailListViewItem(string name, Bitmap bm)
		{
			this.name = name;
			this.bm = bm;
		}

		public string Name
		{
			get { return name; }
		}

		public Bitmap Bitmap
		{
			set { bm = value; }
			get { return bm; }
		}

		private string name;
		private Bitmap bm;
	}

}
