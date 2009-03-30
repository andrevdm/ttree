using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTreeProfiler
{
	public class Item
	{
		public string Group { get; set; }
		public string Desc { get; set; }
		public long Count { get; set; }
		public TimeSpan Elapsed { get; set; }
		public Action<Item> Method { get; set; }
	}
}
