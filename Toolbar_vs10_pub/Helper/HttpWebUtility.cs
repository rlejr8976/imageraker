using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace ImageRakerToolbar
{
	public class HttpWebUtility : IDisposable
	{
		public HttpWebUtility()
		{
		}

		public HttpWebUtility(int httpTimeOut, bool retryOnTimeOut, int retryCount)
		{
			this.retryOnTimeOut = retryOnTimeOut;
			this.retryCount = retryCount;
		}

		//~HttpWebUtility()
		//{
		//    Close();
		//}

		public void Dispose()
		{
			Close();

			//Finalize();
			//System.GC.SuppressFinalize(this);
		}

		public void OpenUrl(string url, string refererUrl)
		{
			OpenUrl(url, refererUrl, null);
		}

		public void Login(string url, string idParam, string idValue, string passParam, string passValue)
		{
			string postData = string.Format("{0}={1}&{2}={3}", idParam, idValue, passParam, passValue);

			OpenUrl(url, null, postData);
		}

		public void SaveContentToFile(string url, string filePath, string refererUrl)
		{
			const int BufferSize = 10 * 1024;
			Stream receiveStream = null;

			OpenUrl(url, refererUrl);

			receiveStream = GetReceiveStream();

			byte[] buffer = new byte[BufferSize];
			int bytesToRead = BufferSize, bytesRead = 0;

			try
			{
				using (FileStream sw = new FileStream(filePath, FileMode.Create))
				{
					while (bytesToRead > 0)
					{
						int n = receiveStream.Read(buffer, 0, bytesToRead);

						bytesToRead = n;
						bytesRead += n;

						sw.Write(buffer, 0, n);
					}
				}
			}
			catch (Exception)
			{
				// 전송중 중지된 파일은 삭제함
				File.Delete(filePath);

				throw;
			}

			//if (receiveStream != null)
			//    receiveStream.Close();
		}

		public void Close()
		{
			if (readStream != null)
			{
				readStream.Close();
				readStream = null;
			}

			if (receiveStream != null)
			{
				receiveStream.Close();
				receiveStream = null;
			}

			if (response != null)
			{
				response.Close();
				response = null;
			}
		}

		public void InitCookie()
		{
			cookieContainer = new CookieContainer();
		}

		public CookieContainer GetCookie()
		{
			return cookieContainer;
		}

		public void SetCookie(CookieContainer cookie)
		{
			cookieContainer = cookie;
		}

		public void AddCookie(string name, string value)
		{
			cookieContainer.Add(new Cookie(name, value));
		}

		public void AddCookie(string name, string value, string path, string domain)
		{
			cookieContainer.Add(new Cookie(name, value, path, domain));
		}

		public Stream GetReceiveStream()
		{
			return receiveStream;
		}

		public string GetHtmlContent()
		{
			readStream = new StreamReader(receiveStream, Encoding.Default);

			return readStream.ReadToEnd();
		}

		public void OpenUrl(string url, string refererUrl, string postData)
		{
			// 시작전에 반드시 연결을 끊어준다.
			Close();

			bool retry = false;
			int currentRetryCount = 0;

			do
			{
				try
				{
					request = (HttpWebRequest)WebRequest.Create(url);
					request.Timeout = httpTimeOut;
					request.CookieContainer = cookieContainer;
					request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2)";
					request.Referer = refererUrl;

					if (postData != null && postData.Length != 0)
					{
						request.Method = "POST";

						ASCIIEncoding encoding = new ASCIIEncoding();
						byte[] byte1 = encoding.GetBytes(postData);

						// Set the content type of the data being posted.
						request.ContentType = "application/x-www-form-urlencoded";

						// Set the content length of the string being posted.
						request.ContentLength = byte1.Length;

						Stream newStream = request.GetRequestStream();
						newStream.Write(byte1, 0, byte1.Length);
					}

					response = (HttpWebResponse)request.GetResponse();
					receiveStream = response.GetResponseStream();
					cookieContainer.Add(response.Cookies);
				}
				catch (WebException e)
				{
					Close();

					if (retryOnTimeOut && e.Status == WebExceptionStatus.Timeout && currentRetryCount < retryCount)
					{
						retry = true;
						currentRetryCount++;
					}
					else
					{
						throw;
					}
				}
			} while (retry);
		}

		private bool retryOnTimeOut = false;
		private int retryCount = 1;
		private int httpTimeOut = 30 * 1000;

		private HttpWebRequest request;
		private HttpWebResponse response;
		private Stream receiveStream;
		private StreamReader readStream;
		private CookieContainer cookieContainer = new CookieContainer();
	}
}
