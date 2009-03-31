using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTreeProfiler
{
	public class Profiler
	{
		private List<Item> m_items = new List<Item>();
		private Dictionary<string, int> m_combine = new Dictionary<string, int>();

		public void Add( string group, string desc, Action<Item> method )
		{
			m_items.Add( new Item { Group = group, Desc = desc, Count = 0, Method = method } );
		}

		public void AddCombine( string group, int percent )
		{
			m_combine[ group ] = percent;
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

			ProcessCombinedResult();
		}

		private void ProcessCombinedResult()
		{
			var descs = (from i in m_items select i.Desc).Distinct();
			var combined = new Dictionary<string, double>();

			if( !BuildCombinedResult( descs, combined ) )
				return;

			PrintCombinedResults( combined );
		}

		private void PrintCombinedResults( Dictionary<string, double> combined )
		{
			var sortedCombined = (from c in combined orderby c.Value descending select new { c.Key, c.Value });
			long maxDescLen = (from i in combined.Keys select i.Length).Max();
			double maxResultPercent = (from i in combined.Values select i).Max();

			Console.WriteLine( "----------" );
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine( "Combined" );
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine( "----------" );

			foreach( var combinedResult in sortedCombined )
			{
				double percent = combinedResult.Value / maxResultPercent;

				string bar = DrawBar( percent, 110 );

				Console.Write( "{0," + maxDescLen + "}", combinedResult.Key );

				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write( " {0} ", bar );
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.Write( " {0,7:F2}%", percent * 100 );
				Console.WriteLine();
			}
		}

		private bool BuildCombinedResult( IEnumerable<string> descs, Dictionary<string, double> combined )
		{
			foreach( var desc in descs )
			{
				foreach( KeyValuePair<string, int> kv in m_combine )
				{
					Item item = (from i in m_items where i.Group == kv.Key where i.Desc == desc select i).First();

					if( item == null )
					{
						Console.WriteLine( "{0}.{1} missing combined results will not be displayed", item.Group, item.Desc );
						return false;
					}

					if( !combined.ContainsKey( item.Desc ) )
						combined[ item.Desc ] = 0;

					combined[ item.Desc ] += (item.Percent * 100) * ((double)kv.Value / 100);
				}
			}

			return true;
		}

		private void PrintResults( string grp )
		{
			long maxCount = (from i in m_items where i.Group == grp select i.Count).Max();
			long maxDescLen = (from i in m_items where i.Group == grp select i.Desc.Length).Max();
			long maxCountLen = (from i in m_items where i.Group == grp select i.Count.ToString().Length).Max();

			var results = (from i in m_items where i.Group == grp orderby i.Count descending select i);

			Console.WriteLine();
			Console.WriteLine( new string( '-', grp.Length + 1 ) );
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine( grp.ToUpper() );
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine( new string( '-', grp.Length + 1 ) );
			foreach( var result in results )
			{
				double percent = result.Count / (double)maxCount;
				result.Percent = percent;

				string bar = DrawBar( percent, 110 );

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

		private string DrawBar( double percent, int len )
		{
			var bar = new StringBuilder();
			int chars = (int)(percent * (double)len);

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
