using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace ImageRakerToolbar
{
	[RunInstaller(true)]
	public partial class ImageRakerInstaller : Installer
	{
		public ImageRakerInstaller()
		{
			InitializeComponent();
		}

		public ImageRakerInstaller(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		/// <summary>
		/// Installation
		/// </summary>
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);

			RegistrationServices regsrv = new RegistrationServices();
			
			if (!regsrv.RegisterAssembly(this.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
			{
				throw new InstallException("Failed To Register for COM");
			}
		}

		/// <summary>
		/// Deinstallation
		/// </summary>
		/// <param name="savedState"></param>
		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			base.Uninstall(savedState);

			Assembly asm = Assembly.GetExecutingAssembly();

			string fullName = asm.GetModules()[0].FullyQualifiedName;
			string dataFolder = Toolbar.AppFolder;

			//try
			//{
			//    Directory.Delete(dataFolder, true);
			//}
			//catch (Exception)
			//{
			//    throw new InstallException("Failed to delete folder");
			//}

			//try
			//{
			//    Registry.LocalMachine.DeleteSubKeyTree(IEToolbarEngine.AppKey);
			//    Registry.CurrentUser.DeleteSubKeyTree(IEToolbarEngine.AppKey);
			//}
			//catch (Exception)
			//{
			//}

			RegistrationServices regsrv = new RegistrationServices();

			if (!regsrv.UnregisterAssembly(this.GetType().Assembly))
			{
				throw new InstallException("Failed To Unregister for COM");
			}

		}
	}
}
