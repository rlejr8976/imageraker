using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace ImageRakerToolbar
{
	public class FilePathMaker
	{
		public static string AppendString = "_by_ImageRaker";
		public static string DefaultImageExtension = ".jpg";

		// enums
		public enum FileNameMakingMethod
		{
			PartialUrl,						// 중복시 덮어쓰기
			FullUrl,						// 중복시 덮어쓰기
			FileName,						// 중복시 통보
			FileNameDuplicateNotAllowed,	// 사라진 옵션
			FileNameOrUrl					// 최초 중복시 재시도, 이후 덮어쓰기 (이름으로 시도하고 중복이면 url로 저장)
		}

		public FilePathMaker(string saveFolder)
		{
			this.saveFolder = saveFolder;
		}

		public static FileNameMakingMethod GetDefaultFileNameMakingMethod()
		{
			return FileNameMakingMethod.PartialUrl;
		}

		public static FileNameMakingMethod GetFileNameMakingMethodFromConfig()
		{
			FileNameMakingMethod method = GetDefaultFileNameMakingMethod();		// default

			string methodStr = Config.Instance.GetConfig("FileNameMakingMethod");

			if(methodStr == FileNameMakingMethod.PartialUrl.ToString())
			{
				method = FileNameMakingMethod.PartialUrl;
			}
			else if (methodStr == FileNameMakingMethod.FullUrl.ToString())
			{
				method = FileNameMakingMethod.FullUrl;
			}
			else if (methodStr == FileNameMakingMethod.FileName.ToString())
			{
				method = FileNameMakingMethod.FileName;
			}
			else if (methodStr == FileNameMakingMethod.FileNameDuplicateNotAllowed.ToString())
			{
				// use default
			}
			else if(methodStr == FileNameMakingMethod.FileNameOrUrl.ToString())
			{
				method = FileNameMakingMethod.FileNameOrUrl;
			}
			else
			{
				Logger.Warn("Cannot recognize file name making method config value {0}. default value used.", methodStr);
			}

			return method;
		}

		public string MakeFilePath(string url, FileNameMakingMethod method)
		{
			string path = null;

			switch (method)
			{
				default:
					Logger.Warn("File name making method is not valid: {0}", method);

					goto case FileNameMakingMethod.FileNameOrUrl;

				case FileNameMakingMethod.PartialUrl:
					{
						// 마지막 디렉토리 + 파일이름
						// ex) www.hello.com/test.jpg
						// ex) www.my.com/1/test.jpg

						// find last /
						int last = url.LastIndexOf('/');
						int secondlast = url.Substring(0, last).LastIndexOf('/');

						if(secondlast == -1)
						{
							secondlast = last;
						}

						string partialurl = url.Substring(secondlast + 1);
						string filename = HttpUtility.UrlEncode(partialurl);

						Logger.DLog("partial url: {0}, filename: {1} from full url: {2}", partialurl, filename, url);

						path = GetFilePath(filename);
					}
					break;

				case FileNameMakingMethod.FullUrl:
					{
						string filename = HttpUtility.UrlEncode(url);

						path = GetFilePath(filename);
					}
					break;

				case FileNameMakingMethod.FileName:
					{
						string last = url.Substring(url.LastIndexOf('/') + 1);
						string filename = HttpUtility.UrlEncode(last);

						path = GetFilePath(filename);
					}
					break;

				case FileNameMakingMethod.FileNameDuplicateNotAllowed:
					/*
					{
						string last = url.Substring(url.LastIndexOf('/') + 1);
						string filename = HttpUtility.UrlEncode(last);

						// 중복 파일 검사
						path = GetFilePath(filename);
						string alterPath = null;

						if (File.Exists(path))
						{
							Logger.Info("File path {0} already exists. try alter path.", path);

							int count = 1;

							do
							{
								// make alter path
								string nameonly;
								string ext;

								ExtractFileNameAndExt(filename, out nameonly, out ext);

								alterPath = string.Format("{0}({1}){2}", nameonly, count++, ext);
								alterPath = GetFilePath(alterPath);
							}
							while (File.Exists(alterPath));

							path = alterPath;
						}
					}
					break;
					 */
					// 사라진 옵션이니 FileNameOrUrl로 대체

				case FileNameMakingMethod.FileNameOrUrl:
					{
						// by filename first
						path = MakeFilePath(url, FileNameMakingMethod.FileName);

						// check dup
						if (File.Exists(path))
						{
							Logger.DLog("file already exists, try url");

							// by url
							path = MakeFilePath(url, FileNameMakingMethod.FullUrl);
						}
					}
					break;
			}

			return path;
		}

		/// <summary>
		/// testfile.jpg를 testfile, .jpg로 분리한다.
		/// </summary>
		/// <param name="filename">ex: 'testfile.jpg'</param>
		/// <param name="nameonly">ex: 'testfile'</param>
		/// <param name="ext">ex: '.jpg'</param>
		/// <returns></returns>
		private bool ExtractFileNameAndExt(string filename, out string nameonly, out string ext)
		{
			if (filename.LastIndexOf('.') == -1)
			{
				nameonly = filename;
				ext = "";

				return false;
			}

			nameonly = filename.Substring(0, filename.LastIndexOf('.'));
			//ext = filename.Substring(filename.LastIndexOf('.') + 1);
			ext = filename.Substring(filename.LastIndexOf('.'));	// .을 포함한다.

			return true;
		}

		private string GetFilePath(string filename)
		{
			// 우선 확장자가 붙어있지 않은 경우 확장자를 붙이고, appendstring을 붙인다.

			// make path
			filename = AppendImageExtension(filename);

			// append '_by_ImageRaker' to filename
			string nameonly;
			string ext;

			ExtractFileNameAndExt(filename, out nameonly, out ext);

			filename = string.Format("{0}{1}{2}", nameonly, AppendString, ext);

			// cut excceeded string
			string path = saveFolder + "\\" + filename;
			path = path.Substring(0, Math.Min(path.Length, MaxPath));

			return path;
		}

		/// <summary>
		/// 파일이름이 이미지 파일인지 검사
		/// </summary>
		/// <param name="filename">파일 이름</param>
		/// <returns>이미지 파일 경로일 경우 true</returns>
		private bool ContainsImageExtension(string filename)
		{
			string[] ImageExtensions = {".jpg", ".jpeg", ".bmp", ".png", ".tga", ".gif",	/*well-known format*/
											  ".pcx", ".psd", ".tiff"};

			string lowPath = filename.ToLower();

			foreach (string ie in ImageExtensions)
			{
				if (lowPath.EndsWith(ie))
					return true;
			}

			return false;
		}

		/// <summary>
		/// 이미지 확장자가 붙지 않은 경우 붙여준다.
		/// </summary>
		/// <param name="filename">파일 경로</param>
		/// <returns>이미지 확장자가 붙은 경로</returns>
		private string AppendImageExtension(string filename)
		{
			if (!ContainsImageExtension(filename))
			{
				filename += DefaultImageExtension;
			}

			return filename;
		}

		private const int MaxPath = 255;

		private string saveFolder;
	}
}
