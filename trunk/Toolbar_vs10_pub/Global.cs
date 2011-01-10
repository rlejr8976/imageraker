using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace ImageRakerToolbar
{
	public class About
	{
		public static readonly string AppName = "ImageRaker";
		public static readonly string Version = "0.8";

		public static string GetBuildDate()
		{
			DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

			return buildDate.ToString();
		}
	};

	public class Pair<T, U>
	{
		public Pair()
		{
		}

		public Pair(T first, U second)
		{
			this.First = first;
			this.Second = second;
		}

		private T first;
		private U second;

		public T First { get { return first; } set { first = value; } }
		public U Second { get { return second; } set { second = value; } }
	};

	// allow duplicate key
	class MultiSortedDictionary<TKey, TValue>
	{
		public MultiSortedDictionary()
		{
			dic = new SortedDictionary<TKey, List<TValue>>();
		}

		public MultiSortedDictionary(IComparer<TKey> comparer)
		{
			dic = new SortedDictionary<TKey, List<TValue>>(comparer);
		}

		public void Add(TKey key, TValue value)
		{
			List<TValue> list;

			if (dic.TryGetValue(key, out list))
			{
				list.Add(value);
			}
			else
			{
				list = new List<TValue>();
				list.Add(value);

				dic.Add(key, list);
			}
		}

		public IEnumerable<TKey> Keys
		{
			get
			{
				return this.dic.Keys;
			}
		}

		public List<TValue> this[TKey key]
		{
			get
			{
				List<TValue> list;

				if (this.dic.TryGetValue(key, out list))
				{
					return list;
				}
				else
				{
					return new List<TValue>();
				}
			}
		}

		private SortedDictionary<TKey, List<TValue>> dic = null;
	}

	class LongReverseComparer : IComparer<long>
	{
		int IComparer<long>.Compare(long x, long y)
		{
			// in reverse order
			if (x < y) return 1;
			if (x > y) return -1;

			return 0;
		}
	}

	//class ReverseComparer<T> : IComparer<T>
	//{
	//    int IComparer<T>.Compare(T x, T y)
	//    {
	//        // in reverse order
	//        if (x < y) return 1;
	//        if (x > y) return -1;

	//        return 0;
	//    }
	//}
}
