using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;

namespace ImageRakerToolbar
{
	public class BandObject : UserControl, IObjectWithSite2, IDeskBand, IDockingWindow, IOleWindow, IInputObject
	{
		/// <summary>
		/// Reference to the host explorer.
		/// </summary>
		protected WebBrowserClass webBrowserClass;
		protected IInputObjectSite inputObjectSite;

		protected String title;
		protected Size minSize = new Size(-1, -1);	// 사이즈는 반드시 지정 해야 함.
		protected Size maxSize = new Size(-1, -1);
		protected Size integralSize = new Size(-1, -1);

		protected delegate void SetSiteEventHandler(Object pUnkSite);
		protected event SetSiteEventHandler SetSiteEvent;

		protected delegate void ShowDWEventHandler(bool fShow);
		protected event ShowDWEventHandler ShowDWEvent;

		public BandObject()
		{
		}

		public virtual void GetBandInfo(UInt32 dwBandID, UInt32 dwViewMode, ref DESKBANDINFO dbi)
		{
			dbi.wszTitle = this.title;

			dbi.ptActual.X = this.Size.Width;
			dbi.ptActual.Y = this.Size.Height;

			dbi.ptMaxSize.X = this.maxSize.Width;
			dbi.ptMaxSize.Y = this.maxSize.Height;

			dbi.ptMinSize.X = this.minSize.Width;
			dbi.ptMinSize.Y = this.minSize.Height;

			dbi.ptIntegral.X = this.integralSize.Width;
			dbi.ptIntegral.Y = this.integralSize.Height;

			dbi.dwMask = DBIM.TITLE | DBIM.MINSIZE;
			dbi.dwModeFlags = DBIMF.NORMAL;
		}

		/// <summary>
		/// Called by explorer when band object needs to be showed or hidden.
		/// </summary>
		/// <param name="fShow"></param>
		public virtual void ShowDW(bool fShow)
		{
			if (fShow)
				Show();
			else
				Hide();

			if(ShowDWEvent != null)
				ShowDWEvent(fShow);

			// write to config
			Config.Instance.SetConfig("ShowToolbar", fShow.ToString());
		}

		/// <sumrmary>
		/// Called by explorer when window is about to close.
		/// </summary>
		public virtual void CloseDW(UInt32 dwReserved)
		{
			Dispose(true);
		}

		/// <summary>
		/// Not used.
		/// </summary>
		public virtual void ResizeBorderDW(IntPtr prcBorder, Object punkToolbarSite, bool fReserved) { }

		public virtual void GetWindow(out System.IntPtr phwnd)
		{
			phwnd = Handle;
		}

		public virtual void ContextSensitiveHelp(bool fEnterMode) { }

		public virtual void SetSite(Object pUnkSite)
		{
			if (inputObjectSite != null)
				Marshal.ReleaseComObject(inputObjectSite);

			if (webBrowserClass != null)
			{
				Marshal.ReleaseComObject(webBrowserClass);
				webBrowserClass = null;
			}

			inputObjectSite = (IInputObjectSite)pUnkSite;

			if (inputObjectSite != null)
			{
				//pUnkSite is a pointer to object that implements IOleWindowSite or something  similar
				//we need to get access to the top level object - explorer itself
				//to allows this explorer objects also implement IServiceProvider interface
				//(don't mix it with System.IServiceProvider!)
				//we get this interface and ask it to find WebBrowserApp
				_IServiceProvider sp = inputObjectSite as _IServiceProvider;
				Guid guid = ExplorerGUIDs.IID_IWebBrowserApp;
				Guid riid = ExplorerGUIDs.IID_IUnknown;

				try
				{
					object w;
					sp.QueryService(ref guid, ref riid, out w);

					//once we have interface to the COM object we can create RCW from it
					webBrowserClass = (WebBrowserClass)Marshal.CreateWrapperOfType(w as IWebBrowser, typeof(WebBrowserClass));

					if (SetSiteEvent != null)
						SetSiteEvent(pUnkSite);
				}
				catch (COMException)
				{
					//we anticipate this exception in case our object instantiated 
					//as a Desk Band. There is no web browser service available.
				}
			}
			else
			{
				if (SetSiteEvent != null)
					SetSiteEvent(pUnkSite);
			}
		}

		public virtual void GetSite(ref Guid riid, out object ppvSite)
		{
			ppvSite = inputObjectSite;
		}

		/// <summary>
		/// Called explorer when focus has to be chenged.
		/// </summary>
		public virtual void UIActivateIO(Int32 fActivate, ref MSG Msg)
		{
			if (fActivate != 0)
			{
				Control ctrl = GetNextControl(this, true);//first
				if (ModifierKeys == Keys.Shift)
					ctrl = GetNextControl(ctrl, false);//last

				if (ctrl != null) ctrl.Select();
				this.Focus();
			}
		}

		public virtual Int32 HasFocusIO()
		{
			return this.ContainsFocus ? 0 : 1; //S_OK : S_FALSE;
		}

		/// <summary>
		/// Called by explorer to process keyboard events. Undersatands Tab and F6.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>S_OK if message was processed, S_FALSE otherwise.</returns>
		public virtual Int32 TranslateAcceleratorIO(ref MSG msg)
		{
			if (msg.message == 0x100)//WM_KEYDOWN
				if (msg.wParam == (uint)Keys.Tab || msg.wParam == (uint)Keys.F6)//keys used by explorer to navigate from control to control
					if (SelectNextControl(
							ActiveControl,
							ModifierKeys == Keys.Shift ? false : true,
							true,
							true,
							false)
						)
						return 0;//S_OK

			return 1;//S_FALSE
		}

		/// <summary>
		/// Notifies explorer of focus change.
		/// </summary>
		protected override void OnGotFocus(System.EventArgs e)
		{
			base.OnGotFocus(e);
			inputObjectSite.OnFocusChangeIS(this as IInputObject, 1);
		}
		/// <summary>
		/// Notifies explorer of focus change.
		/// </summary>
		protected override void OnLostFocus(System.EventArgs e)
		{
			base.OnLostFocus(e);
			if (ActiveControl == null)
				inputObjectSite.OnFocusChangeIS(this as IInputObject, 0);
		}

		/// <summary>
		/// Called when derived class is registered as a COM server.
		/// </summary>
		[ComRegisterFunctionAttribute]
		public static void Register(Type t)
		{
			string guid = t.GUID.ToString("B");

			RegistryKey rkClass = Registry.ClassesRoot.CreateSubKey(@"CLSID\" + guid);
			RegistryKey rkCat = rkClass.CreateSubKey("Implemented Categories");

			BandObjectAttribute[] boa = (BandObjectAttribute[])t.GetCustomAttributes(typeof(BandObjectAttribute), false);

			string name = t.Name;
			string help = t.Name;
			BandObjectStyle style = 0;
			if (boa.Length == 1)
			{
				if (boa[0].Name != null)
					name = boa[0].Name;

				if (boa[0].HelpText != null)
					help = boa[0].HelpText;

				style = boa[0].Style;
			}

			rkClass.SetValue(null, name);
			rkClass.SetValue("MenuText", name);
			rkClass.SetValue("HelpText", help);

			if (0 != (style & BandObjectStyle.Vertical))
				rkCat.CreateSubKey("{00021493-0000-0000-C000-000000000046}");

			if (0 != (style & BandObjectStyle.Horizontal))
				rkCat.CreateSubKey("{00021494-0000-0000-C000-000000000046}");

			if (0 != (style & BandObjectStyle.TaskbarToolBar))
				rkCat.CreateSubKey("{00021492-0000-0000-C000-000000000046}");

			if (0 != (style & BandObjectStyle.ExplorerToolbar))
				Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").SetValue(guid, name);
		}

		/// <summary>
		/// Called when derived class is unregistered as a COM server.
		/// </summary>
		[ComUnregisterFunctionAttribute]
		public static void Unregister(Type t)
		{
			string guid = t.GUID.ToString("B");
			BandObjectAttribute[] boa = (BandObjectAttribute[])t.GetCustomAttributes(typeof(BandObjectAttribute), false);

			BandObjectStyle style = 0;
			if (boa.Length == 1) style = boa[0].Style;

			if (0 != (style & BandObjectStyle.ExplorerToolbar))
				Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").DeleteValue(guid, false);

			Registry.ClassesRoot.CreateSubKey(@"CLSID").DeleteSubKeyTree(guid);
		}
	}

	/// <summary>
	/// Represents different styles of a band object.
	/// </summary>
	[Flags]
	[Serializable]
	public enum BandObjectStyle : uint
	{
		Vertical = 1,
		Horizontal = 2,
		ExplorerToolbar = 4,
		TaskbarToolBar = 8
	}

	/// <summary>
	/// Specifies Style of the band object, its Name(displayed in explorer menu) and HelpText(displayed in status bar when menu command selected).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class BandObjectAttribute : System.Attribute
	{
		public BandObjectAttribute() { }

		public BandObjectAttribute(string name, BandObjectStyle style)
		{
			Name = name;
			Style = style;
		}
		public BandObjectStyle Style;
		public string Name;
		public string HelpText;
	}

}
