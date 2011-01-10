using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ImageRakerToolbar
{
	class Message
	{
		public static void Info(string format, params object[] args)
		{
			ShowMessageBox(MessageBoxButtons.OK, MessageBoxIcon.Asterisk, format, args);
		}

		public static void Warn(string format, params object[] args)
		{
			ShowMessageBox(MessageBoxButtons.OK, MessageBoxIcon.Exclamation, format, args);
		}

		public static void Error(string format, params object[] args)
		{
			ShowMessageBox(MessageBoxButtons.OK, MessageBoxIcon.Hand, format, args);
		}

		public static void Fatal(string format, params object[] args)
		{
			ShowMessageBox(MessageBoxButtons.OK, MessageBoxIcon.Hand, format, args);
		}

		private static void ShowMessageBox(MessageBoxButtons button, MessageBoxIcon icon, string format, params object[] args)
		{
			string message = string.Format(format, args);

			MessageBox.Show(message, "이미지 레이커", button, icon);
		}
	}
}
