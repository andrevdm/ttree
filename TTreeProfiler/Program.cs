using System;
using System.Collections.Generic;
using System.Text;

using TTree;
using System.Diagnostics;
using System.Collections;

namespace TTreeProfiler
{
	class Program
	{
		static void Main( string[] args )
		{
			int max = 10000000;
			List<Guid> values = Gen( max );
			Console.WriteLine();
			Console.WriteLine( "Values created, profiling" );

			int seconds = 10;

			//TODO Profile( Time(....) ). Adds to collection by type,item (e.g. add, t-tree). Output = percent of max per group

			//var ttree = new Tree<string>( 8, 10 );
			//Time( seconds, "T-Tree", s => ttree.Insert( s ), s => ttree.Search( s ) );

			var list = new List<string>();
			Time( values, seconds, "List - add", s => list.Add( s ), values.Count );
			Time( values, seconds, "List - search", s => list.IndexOf( s ), list.Count - 1 );

			var sortedList = new SortedList<string, string>();
			Time( values, seconds, "SortedList - add", s => sortedList.Add( s, s ), values.Count );
			Time( values, seconds, "SortedList - search", s => { sortedList.IndexOfKey( s ); }, sortedList.Count - 1 );

			var sortedDictionary = new SortedDictionary<string, string>();
			Time( values, seconds, "SortedDictionary - add", s => sortedDictionary.Add( s, s ), values.Count );
			Time( values, seconds, "SortedDictionary - search", s => { string x = sortedDictionary[ s ]; }, sortedDictionary.Count - 1 );

			var arrayList = new ArrayList();
			Time( values, seconds, "ArrayList - add", s => arrayList.IndexOf( s ), values.Count );
			Time( values, seconds, "ArrayList - search", s => arrayList.Add( s ), arrayList.Count - 1 );
		}

		private static void Time( List<Guid> values, int seconds, string desc, Action<string> method, int modCount )
		{
			long count = 0;
			var stop = new Stopwatch();
			stop.Start();
			while( stop.Elapsed.TotalSeconds < seconds )
			{
				method( values[ (int)count % modCount ].ToString() );

				count++;

				if( count >= values.Count )
				{
					Console.WriteLine( "   All values used, exiting test" );
					break;
				}
			}
			stop.Stop();

			Console.WriteLine( "{1,12} in {2}, {0}", desc, count.ToString( "#,#" ), stop.Elapsed );
		}

		private static List<Guid> Gen( int max )
		{
			var items = new List<Guid>();

			for( int i = 0; i < max; ++i )
			{
				if( i % 10000 == 0 )
					Console.WriteLine( i );

				items.Add( Guid.NewGuid() );
			}

			return items;
		}
	}
}
