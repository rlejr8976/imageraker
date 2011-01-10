using System;
using System.Collections.Generic;
using System.Text;

namespace ImageRakerToolbar
{
	class ExceptionTester
	{
		public static ExceptionTester Instance
		{
			get { return instance; }
		}

		private void RegisterExceptions()
		{
			//////////////////////////////////////////////////////////////////////////
			// 주석으로 켜고 끄자.
			Disable();
			//////////////////////////////////////////////////////////////////////////


			// 릴리즈에서는 반드시 disable
#if !DEBUG
			Disable();
#endif

			// register whole test exceptions here!
			if (enable == false)
			{
				return;
			}

#if DEBUG
			//RegisterException("random_mainformload", 0.1);
			//RegisterException("random_save1", 0.1);
			//RegisterException("random_save2", 0.1);
			//RegisterException("random_save3", 0.1);

			RegisterException("exception", 1);

			RegisterException("showupdatemessage", 0.8);
			RegisterException("checkforupdatethread", 0.7);
			RegisterException("setconfig", 0.3);
			RegisterException("loadblockurls", 0.3);
			RegisterException("loadconfig", 0.01);
			RegisterException("savecontenttofile", 0.2);
			RegisterException("makethumbnail", 0.01);
			RegisterException("htmlelemdrawing", 0.01);
			RegisterException("openimagerakerform", 0.01);
			RegisterException("reportthread", 0.4);

			RegisterException("gethtmlbyole", 0.1);
			RegisterException("showabout", 0.5);
			RegisterException("aboutformctor", 0.5);
			RegisterException("updatethumbnaillistview", 0.1);
#endif
		}

		public void Enable()
		{
			enable = true;

			Logger.Warn("ExceptionTester ENABLED!");
		}

		public void Disable()
		{
			enable = false;

			Logger.Warn("ExceptionTester DISABLED!");
		}

		public void RegisterException(string name, double prob)
		{
			Logger.DLog("register exception - name: {0}, prob: {1}", name, prob);

			registeredExceptions.Add(name, prob);
		}

		public bool Throw(string name)
		{
			return Throw(name, new Exception(string.Format("ExceptionTester - name: {0}", name)));
		}

		public bool Throw(string name, Exception ex)
		{
			if(enable == false)
			{
//				Logger.DLog("exception tester disabled!");
				return false;
			}

			double prob = 0;

			if(registeredExceptions.TryGetValue(name, out prob))
			{
				Random r = new Random();

				double rv = r.NextDouble();

				if(0 <= prob && prob <= 1 && rv <= prob)
				{
					Logger.DLog("throwing test exception - name: {0}, prob: {1}", name, prob);

					// begin throw!
					IncrementThrowCount(name);

					throw ex;
					//throw new Exception(string.Format("ExceptionTester - name: {0}, prob: {1}", name, prob));
				}
				else
				{
					//Logger.DLog("not in range or invalid prob! - test exception name: {0}, prob: {1}, rv: {2}", name, prob, rv);
				}
			}
			else
			{
				Logger.DLog("cannot found test exception named: {0}", name);
			}

			return false;
		}

		private void IncrementThrowCount(string name)
		{
			if(!threwExceptions.ContainsKey(name))
			{
				threwExceptions.Add(name, 0);
			}

			threwExceptions[name]++;
		}

		public void Dump()
		{
			if (enable == false)
			{
				return;
			}

			Logger.DLog("---DUMP EXCEPTION TESTER---");

			foreach(KeyValuePair<string, double> kvp in registeredExceptions)
			{
				double count = 0;

				threwExceptions.TryGetValue(kvp.Key, out count);

				if(count == 0)
				{
					Logger.DLog("    name: {0}, prob: {1}, NOT THROWN YET!", kvp.Key, kvp.Value);
				}
				else
				{
					Logger.DLog("    name: {0}, prob: {1}, threw count: {2}", kvp.Key, kvp.Value, count);
				}
			}

			Logger.DLog("---DUMP EXCEPTION TESTER END.---");
		}

		private ExceptionTester()
		{
			RegisterExceptions();
		}

		private bool enable = true;
		private Dictionary<string, double> registeredExceptions = new Dictionary<string, double>();
		private Dictionary<string, double> threwExceptions = new Dictionary<string,double>();

		private static ExceptionTester instance = new ExceptionTester();
	}
}
