//	This file is a part of the Command Prompt Explorer Bar project.
// 
//	Copyright Pavel Zolnikov, 2002
//
//			declarations of some COM interfaces and structues

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageRakerToolbar
{
	// NOT WORKS!!!!!!
	public class ShowInactive
	{
		private const int SW_SHOWNOACTIVATE = 4;
		private const int HWND_TOPMOST = -1;
		private const uint SWP_NOACTIVATE = 0x0010;

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		static extern bool SetWindowPos(
			 int hWnd,           // window handle
			 int hWndInsertAfter,    // placement-order handle
			 int X,          // horizontal position
			 int Y,          // vertical position
			 int cx,         // width
			 int cy,         // height
			 uint uFlags);       // window positioning flags

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static void ShowInactiveTopmost(Form frm)
		{
			ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
			SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST,
			frm.Left, frm.Top, frm.Width, frm.Height,
			SWP_NOACTIVATE);
		}

		//protected override bool ShowWithoutActivation
		//{
		//    get { return true; }
		//}

	}

	abstract class ExplorerGUIDs
	{
		public static readonly Guid IID_IWebBrowserApp = new Guid("{0002DF05-0000-0000-C000-000000000046}");
		public static readonly Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
	}

	public class UxTheme
	{
		[DllImport("UxTheme.dll")]
		public static extern IntPtr DrawThemeParentBackground(IntPtr hwnd, IntPtr hdc, ref Rectangle prc);
	}

	[Flags]
	public enum DBIM : uint
	{
		MINSIZE = 0x0001,
		MAXSIZE = 0x0002,
		INTEGRAL = 0x0004,
		ACTUAL = 0x0008,
		TITLE = 0x0010,
		MODEFLAGS = 0x0020,
		BKCOLOR = 0x0040
	}

	[Flags]
	public enum DBIMF : uint
	{
		NORMAL = 0x0000,
		FIXED = 0x0001,
		FIXEDBMP = 0x0004,
		VARIABLEHEIGHT = 0x0008,
		UNDELETEABLE = 0x0010,
		DEBOSSED = 0x0020,
		BKCOLOR = 0x0040,
		USECHEVRON = 0x0080,
		BREAK = 0x0100,
		ADDTOFRONT = 0x0200,
		TOPALIGN = 0x0400
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DESKBANDINFO
	{
		public DBIM dwMask;
		public Point ptMinSize;
		public Point ptMaxSize;
		public Point ptIntegral;
		public Point ptActual;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
		public String wszTitle;
		public DBIMF dwModeFlags;
		public Int32 crBkgnd;
	};

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
	public interface IObjectWithSite2
	{
		void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkSite);
		void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out Object ppvSite);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000114-0000-0000-C000-000000000046")]
	public interface IOleWindow
	{
		void GetWindow(out System.IntPtr phwnd);
		void ContextSensitiveHelp([In] bool fEnterMode);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8")]
	public interface IDockingWindow
	{
		void GetWindow(out System.IntPtr phwnd);
		void ContextSensitiveHelp([In] bool fEnterMode);

		void ShowDW([In] bool fShow);
		void CloseDW([In] UInt32 dwReserved);
		void ResizeBorderDW(
			IntPtr prcBorder,
			[In, MarshalAs(UnmanagedType.IUnknown)] Object punkToolbarSite,
			bool fReserved);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
	public interface IDeskBand
	{
		void GetWindow(out System.IntPtr phwnd);
		void ContextSensitiveHelp([In] bool fEnterMode);

		void ShowDW([In] bool fShow);
		void CloseDW([In] UInt32 dwReserved);

		void ResizeBorderDW(
			IntPtr prcBorder,
			[In, MarshalAs(UnmanagedType.IUnknown)] Object punkToolbarSite,
			bool fReserved);

		void GetBandInfo(
			UInt32 dwBandID,
			UInt32 dwViewMode,
			ref DESKBANDINFO pdbi);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000010c-0000-0000-C000-000000000046")]
	public interface IPersist
	{
		void GetClassID(out Guid pClassID);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	public interface IPersistStream
	{
		void GetClassID(out Guid pClassID);

		void IsDirty();

		void Load([In, MarshalAs(UnmanagedType.Interface)] Object pStm);

		void Save([In, MarshalAs(UnmanagedType.Interface)] Object pStm,
		   [In] bool fClearDirty);

		void GetSizeMax([Out] out UInt64 pcbSize);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
	public interface _IServiceProvider
	{
		void QueryService(
			ref Guid guid,
			ref Guid riid,
			[MarshalAs(UnmanagedType.Interface)] out Object Obj);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("68284faa-6a48-11d0-8c78-00c04fd918b4")]
	public interface IInputObject
	{
		void UIActivateIO(Int32 fActivate, ref MSG msg);

		[PreserveSig]
		//[return:MarshalAs(UnmanagedType.Error)]
		Int32 HasFocusIO();

		[PreserveSig]
		Int32 TranslateAcceleratorIO(ref MSG msg);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f1db8392-7331-11d0-8c99-00a0c92dbfe8")]
	public interface IInputObjectSite
	{
		[PreserveSig]
		Int32 OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] Object punkObj, Int32 fSetFocus);
	}

	public struct POINT
	{
		public Int32 x;
		public Int32 y;
	}

	public struct MSG
	{
		public IntPtr hwnd;
		public UInt32 message;
		public UInt32 wParam;
		public Int32 lParam;
		public UInt32 time;
		public POINT pt;
	}

	// for enumerating browser object
	[ComImport, Guid("00000100-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumUnknown
	{
		[PreserveSig]
		int Next(
			[In, MarshalAs(UnmanagedType.U4)] int celt,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object rgelt,
			[Out, MarshalAs(UnmanagedType.U4)] out int pceltFetched
		);

		[PreserveSig]
		int Skip(
			[In, MarshalAs(UnmanagedType.U4)] int celt
		);

		void Reset();
		void Clone(out IEnumUnknown ppenum);
	}

	[Flags()]
	public enum tagOLECONTF
	{
		OLECONTF_EMBEDDINGS = 1,
		OLECONTF_LINKS = 2,
		OLECONTF_OTHERS = 4,
		OLECONTF_ONLYUSER = 8,
		OLECONTF_ONLYIFRUNNING = 16,
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000011B-0000-0000-C000-000000000046")]
	public interface IOleContainer
	{
		[PreserveSig]
		int ParseDisplayName(
			[In, MarshalAs(UnmanagedType.Interface)] object pbc,
			[In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName,
			[Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
			[Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut
		);

		[PreserveSig]
		int EnumObjects(
			[In, MarshalAs(UnmanagedType.U4)] tagOLECONTF grfFlags,
			out IEnumUnknown ppenum
		);

		[PreserveSig]
		int LockContainer(bool fLock);
	}

	[Guid("3050f669-98b5-11cf-bb82-00aa00bdce0b"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	ComVisible(true),
	ComImport]
	public interface IHTMLElementRender
	{
		void DrawToDC([In] IntPtr hDC);
		void SetDocumentPrinter([In, MarshalAs(UnmanagedType.BStr)] string bstrPrinterName, [In] IntPtr hDC);
	};

}