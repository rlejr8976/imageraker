using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SHDocVw;
using Microsoft.Win32;

namespace ImageRakerToolbar
{
	[ComVisible(true)]
	[Guid("47A4820F-FFB5-404e-B858-051ABDC72962")]
	[ClassInterface(ClassInterfaceType.None)]
	public class ShowToolbarBHO : IObjectWithSite2
	{
		private InternetExplorer explorer;

		/// <summary>
		/// Called, when the BHO is instantiated and when it is destroyed.
		/// </summary>
		/// <param name="site"></param>
		public void SetSite(Object site)
		{
			if (site != null)
			{
				explorer = (InternetExplorer)site;

				string showString = Config.Instance.GetConfig("ShowToolbar");
				bool show = true;
					
				if(bool.TryParse(showString, out show) == false)
				{
					Logger.Log("Cannot parse 'ShowToolbar' config value. value: {0}", showString);
				}

				ShowBrowserBar(show);
			}
		}

		public void GetSite(ref Guid guid, out Object ppvSite)
		{
			IntPtr punk = Marshal.GetIUnknownForObject(explorer);
			ppvSite = new object();
			IntPtr ppvSiteIntPtr = Marshal.GetIUnknownForObject(ppvSite);
			int hr = Marshal.QueryInterface(punk, ref guid, out ppvSiteIntPtr);
			Marshal.Release(punk);
		}

		private void ShowBrowserBar(bool bShow)
		{
			Guid guid = typeof(Toolbar).GUID;
			object pvaClsid = (object)(guid.ToString("B"));
			object pvarShow = (object)bShow;
			object pvarSize = null;
			if (bShow) /* hide Browser bar before showing to prevent erroneous behavior of IE*/
			{
				object pvarShowFalse = (object)false;
				explorer.ShowBrowserBar(ref pvaClsid, ref pvarShowFalse, ref pvarSize);
			}
			explorer.ShowBrowserBar(ref pvaClsid, ref pvarShow, ref pvarSize);
		}

		private const string BHOKeyName = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

		/// <summary>
		/// Called, when IE browser starts.
		/// </summary>
		/// <param name="t"></param>
		[ComRegisterFunction]
		public static void RegisterBHO(Type t)
		{
			//System.Windows.Forms.MessageBox.Show ("Register BHO");

			RegistryKey key = Registry.LocalMachine.OpenSubKey(BHOKeyName, true);
			if (key == null)
			{
				key = Registry.LocalMachine.CreateSubKey(BHOKeyName);
			}
			string guidString = t.GUID.ToString("B");
			RegistryKey bhoKey = key.OpenSubKey(guidString, true);

			if (bhoKey == null)
			{
				bhoKey = key.CreateSubKey(guidString);
			}
			// NoExplorer:dword = 1 prevents the BHO to be loaded by Explorer

			string _name = "NoExplorer";
			object _value = (object)1;
			bhoKey.SetValue(_name, _value);
			key.Close();
			bhoKey.Close();
		}

		/// <param name="t"></param>
		[ComUnregisterFunction]
		public static void UnregisterBHO(Type t)
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey(BHOKeyName, true);
			string guidString = t.GUID.ToString("B");
			if (key != null)
			{
				key.DeleteSubKey(guidString, false);
			}
		}

	}
}
