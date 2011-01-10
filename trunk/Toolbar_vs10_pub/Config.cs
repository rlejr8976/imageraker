using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ImageRakerToolbar
{
	class Config
	{
		public static Config Instance
		{
			get { return instance; }
		}

		public string GetConfig(string name)
		{
			string value;

			if(config.TryGetValue(name, out value) == false)
			{
				Logger.Error("Config named '{0}' doesn't exists, get default value", name);

				string defaultValue = GetDefaultConfig(name);

				if(defaultValue == null || defaultValue.Length == 0)
				{
					Logger.Error("Default config named '{0}' doesn't exists!");
					return null;
				}

				return defaultValue;
			}

			return value;
		}

		public int GetConfigInt(string name)
		{
			string strval = GetConfig(name);
			int i = 0;

			if(int.TryParse(strval, out i) == false)
			{
				Logger.Error("Cannot parse config {0}:{1} to int!", name, strval);
			}
		
			return i;
		}

		public bool GetConfigBool(string name)
		{
			string strval = GetConfig(name);
			bool b = false;

			if (bool.TryParse(strval, out b) == false)
			{
				Logger.Error("Cannot parse config {0}:{1} to bool!", name, strval);
			}

			return b;
		}

		public string GetDefaultConfig(string name)
		{
			string value;

			if(defaultConfig.TryGetValue(name, out value) == false)
			{
				Logger.Error("Default config named '{0}' doesn't exists", name);
				return null;
			}

			return value;
		}

		public bool SetConfig(string name, string value)
		{
			if(!config.ContainsKey(name))
			{
				Logger.Warn("Config named '{0}' doesn't exists. You can only save existing config.", name);
//				Message.Warn("설정 항목 {0}이 존재하지 않습니다. 설정을 저장할 수 없습니다.", name);

				return false;
			}

			config[name] = value;

			// save to xml
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(GetConfigFilePath());

				XmlNodeList nodes = doc.GetElementsByTagName(name);
				XmlElement elem = nodes.Item(0) as XmlElement;
				elem.Attributes.Item(0).Value = value;

				doc.Save(GetConfigFilePath());

				ExceptionTester.Instance.Throw("setconfig");

				Logger.Log("Config '{0} : {1}' saved to config file", name, value);

				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn("cannot save config to xml! name: {0}, value: {1}", name, value);
//				Message.Warn("항목 {0}을 설정할 수 없습니다.", name);

				return false;
			}
		}

		public Dictionary<string, long> GetBlockUrls()
		{
			return blockUrls;
		}

		public void AddBlockUrl(string url)
		{
			if(!blockUrls.ContainsKey(url))
			{
				blockUrls.Add(url, DateTime.Now.ToFileTime());
			}
		}

		public void RemoveBlockUrl(string url)
		{
			blockUrls.Remove(url);
		}

		public void ClearBlockUrls()
		{
			Logger.Log("clearing block urls.");

			blockUrls.Clear();
		}

		public void SaveBlockUrls()
		{
			Logger.Log("saving block urls");

			XmlDocument doc = new XmlDocument();
			doc.Load(GetConfigFilePath());

			XmlNodeList nodes = doc.GetElementsByTagName("Config");
			XmlElement root;

			if (nodes.Count != 0)
			{
				root = nodes.Item(0) as XmlElement;
			}
			else	// doesn't exist
			{
				root = doc.CreateElement("Config");
				doc.AppendChild(root);
			}

			// save current
			int i = 0;

			// sort blockurls by datetime in descending order
			MultiSortedDictionary<long, string> sortedBlockUrls = new MultiSortedDictionary<long, string>(new LongReverseComparer());

			foreach (KeyValuePair<string, long> kvp in blockUrls)
			{
				sortedBlockUrls.Add(kvp.Value, kvp.Key);
			}

			// add from sorted
			foreach(long dt in sortedBlockUrls.Keys)
			{
				foreach (string url in sortedBlockUrls[dt])
				{
					if (i >= maxNumOfBlockUrls)
						break;

					string name = BlockUrlPrefix + i.ToString();

					Logger.DLog("save block url, {0} - {1}", name, url);

					nodes = doc.GetElementsByTagName(name);

					if (nodes.Count != 0)
					{
						XmlElement elem = nodes.Item(0) as XmlElement;
						elem.Attributes.Item(0).Value = url;
					}
					else
					{
						XmlElement elem = doc.CreateElement(name);
						root.AppendChild(elem);

						elem.SetAttribute("Url", url);
						elem.SetAttribute("DateTime", dt.ToString());
					}

					i++;
				}
			}

			// clear old
			for (i = blockUrls.Count; i < maxNumOfBlockUrls; i++)
			{
				string name = BlockUrlPrefix + i.ToString();

				Logger.DLog("remove block url, {0}", name);

				nodes = doc.GetElementsByTagName(name);

				if (nodes.Count != 0)
				{
					XmlElement elem = nodes.Item(0) as XmlElement;
					root.RemoveChild(elem);
				}
			}

			doc.Save(GetConfigFilePath());

			Logger.Log("saving block urls DONE.");
		}

		private void LoadBlockUrls(XmlElement root)
		{
			Logger.Log("loading block urls");

			blockUrls.Clear();

			XmlElement parent = root;

			for (int i = 0; i < maxNumOfBlockUrls; i++)
			{
				string name = BlockUrlPrefix + i.ToString();

				XmlNodeList nodes = parent.GetElementsByTagName(name);

				if (nodes.Count != 0)
				{
					// exist
					XmlElement elem = nodes.Item(0) as XmlElement;

					string url = elem.GetAttribute("Url");
					string dts = elem.GetAttribute("DateTime");

					if(!blockUrls.ContainsKey(url))
					{
						DateTime dt = DateTime.Now;
						long dtl = dt.ToFileTime();

						try 
						{
							ExceptionTester.Instance.Throw("loadblockurls");

							dtl = long.Parse(dts);
							dt = DateTime.FromFileTime(dtl);
						}
						catch (Exception)
						{
							Logger.Warn("cannot parse block url datetime: {0}", dts);
						}

						blockUrls.Add(url, dtl);

						Logger.Log("load block url xml element '{0} - url: {1}, datetime: {2}'", name, url, dt.ToString());
					}
					else
					{
						Logger.Warn("duplicated block url xml element '{0} - url: {1}'", name, url);
					}
				}
				else
				{
					// doesn't exist. but ignore
				}
			}

			Logger.Log("loading block urls DONE.");
		}

		static Config()
		{
			Instance.LoadDefault();
			Instance.Load();
		}

		private void LoadDefault()
		{
			//////////////////////////////////////////////////////////////////////////
			// add whole config value in here!!!
			//////////////////////////////////////////////////////////////////////////

			// basics
			defaultConfig["FirstRun"] = "True";
			defaultConfig["ShowToolbar"] = "True";
			defaultConfig["FormWidth"] = "800";
			defaultConfig["FormHeight"] = "600";
			defaultConfig["SaveFolder"] = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + "ImageRaker";
			defaultConfig["CompareMethod"] = ImageRakerThumnbnailListViewItemComparer.CompareMethod.ByOriginal.ToString();
			defaultConfig["SortOrder"] = ImageRakerThumnbnailListViewItemComparer.SortOrder.Ascending.ToString();
			defaultConfig["FileNameMakingMethod"] = FilePathMaker.GetDefaultFileNameMakingMethod().ToString();
			//defaultConfig["Overwrite"] = "True";	// 일반 저장시 파일이 있어도 덮어쓰기
			defaultConfig["HelpAboutAutoSave"] = "True";

			// min image size
			defaultConfig["MinSize"] = "200";

			// performances
			defaultConfig["NumOfThreads"] = "4";

			// thumbnails
			defaultConfig["ThumbnailSize"] = "180";
			defaultConfig["ThumbnailQuality"] = "High";	// Low or High

			// features
			defaultConfig["ShowAutoSaveButton"] = "True";		// instant save and auto save
			defaultConfig["ExitOnComplete"] = "True";
			defaultConfig["ImproveProgram"] = "True";
			defaultConfig["MarkFailed"] = "True";

			// update
			defaultConfig["CheckForUpdate"] = "True";
			defaultConfig["UpdateLastCheckedDate"] = DateTime.Now.ToShortDateString();

			// block urls
			defaultConfig["UseBlockUrls"] = "True";
			defaultConfig["MaxNumOfBlockUrls"] = "100";

			// filters
			defaultConfig["JpgFilterChecked"] = "True";
			defaultConfig["GifFilterChecked"] = "True";
			defaultConfig["PngFilterChecked"] = "True";
			defaultConfig["BmpFilterChecked"] = "True";
			defaultConfig["EtcFilterChecked"] = "True";
			defaultConfig["ShowSmallsChecked"] = "False";
		}

		private void Load()
		{
			Logger.Info("start loading xml config");

			XmlDocument doc = new XmlDocument();

			string configFilePath = GetConfigFilePath();

			if (File.Exists(configFilePath))
			{
				try
				{
					ExceptionTester.Instance.Throw("loadconfig", new XmlException());

					doc.Load(configFilePath);
				}
				catch(XmlException)
				{
					Logger.Warn("Cannot load config: {0}", configFilePath);
					Message.Warn("설정 파일을 로드할 수 없습니다. 기본 설정을 사용합니다.");
				}
			}
			else
			{
				Logger.Log("Config file path '{0}' doesn't exists", configFilePath);
			}

			// get root element
			XmlNodeList nodes = doc.GetElementsByTagName("Config");
			XmlElement root;

			if (nodes.Count != 0)
			{
				root = nodes.Item(0) as XmlElement;
			}
			else	// doesn't exist
			{
				root = doc.CreateElement("Config");
				doc.AppendChild(root);
			}

			// default config의 key들을 자동으로...
			foreach(string key in defaultConfig.Keys)
			{
				LoadElement(doc, root, key);
			}

			// load max num of block urls
			string s = config["MaxNumOfBlockUrls"];

			if (s != null && int.TryParse(s, out maxNumOfBlockUrls) == false)
			{
				Logger.Warn("cannot parse MaxNumOfBlockUrls, {0}", s);
			}

			// load block urls
			LoadBlockUrls(root);

			doc.Save(configFilePath);

			Logger.Info("xml config loaded.");
		}

		private void LoadElement(XmlDocument doc, XmlElement parent, string name)
		{
			string attrName = "value";
			string defaultValue = GetDefaultConfig(name);

			string value;

			XmlNodeList nodes = parent.GetElementsByTagName(name);

			if (nodes.Count != 0)
			{
				// exist
				XmlElement elem = nodes.Item(0) as XmlElement;
				value = elem.GetAttribute(attrName);
			}
			else
			{
				// doesn't exist
				Logger.Warn("config element {0} doesn't exist. use default value {1}", name, defaultValue);
//				Message.Warn("설정 항목 {0}이 존재하지 않습니다.", name);

				XmlElement elem = doc.CreateElement(name);
				parent.AppendChild(elem);

				elem.SetAttribute(attrName, defaultValue);
				value = defaultValue;
			}

			config[name] = value;

			Logger.Log("load config xml element '{0} : {1}' default: {2}", name, value, defaultValue);
		}

		public string GetConfigFilePath()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + ConfigFileName;

			return path;
		}

		private static Config instance = new Config();

		private const string ConfigFileName = "ImageRakerConfig.xml";

		private Dictionary<string, string> config = new Dictionary<string, string>();
		private Dictionary<string, string> defaultConfig = new Dictionary<string, string>();

		// block urls
		private int maxNumOfBlockUrls = 100;	// default
		private const string BlockUrlPrefix = "BlockUrl_";
		private Dictionary<string, long> blockUrls = new Dictionary<string, long>();	// url, added time (in file time)

	}
}
