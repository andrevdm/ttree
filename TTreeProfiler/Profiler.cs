using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTreeProfiler
{
	public class Profiler
	{
		private List<Item> m_items = new List<Item>();

		public void Add( string group, string desc, Action<Item> method )
		{
			m_items.Add( new Item { Group = group, Desc = desc, Count = 0, Method = method } );
		}

		public void Profile()
		{
			foreach( var item in m_items )
			{
				item.Method( item );
			}
		}

		public void PrintResults()
		{
			var groups = (from i in m_items select i.Group).Distinct();

			foreach( string group in groups )
			{
				PrintResults( group );
			}
		}

		private void PrintResults( string grp )
		{
			long maxCount = (from i in m_items where i.Group == grp select i.Count).Max();
			long maxDescLen = (from i in m_items where i.Group == grp select i.Desc.Length).Max();
			long maxCountLen = (from i in m_items where i.Group == grp select i.Count.ToString().Length).Max();

			var results = (from i in m_items where i.Group == grp orderby i.Count select i);

			Console.WriteLine();
			Console.WriteLine( new string( '-', grp.Length + 1 ) );
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine( grp.ToUpper() );
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine( new string( '-', grp.Length + 1 ) );
			foreach( var result in results )
			{
				float percent = result.Count / (float)maxCount;

				string bar = DrawBar( percent, 90 );

				Console.Write( "{0," + maxDescLen + "}", result.Desc );

				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write( " {0} ", bar );
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.Write( " {0,7:F2}%", percent * 100 );
				Console.Write( ", {0," + (maxCountLen + 3) + "}", result.Count.ToString( "#,#" ) );
				Console.WriteLine();
			}
			Console.WriteLine( new string( '-', grp.Length + 1 ) );

			Console.WriteLine();
			Console.WriteLine();
		}

		private string DrawBar( float percent, int len )
		{
			var bar = new StringBuilder();
			int chars = (int)(percent * (float)len);

			bar.Append( "[" );
			for( int i = 0; i < len; ++i )
			{
				bar.Append( i <= chars ? "*" : " " );
			}
			bar.Append( "]" );

			return bar.ToString();
		}
	}

}
