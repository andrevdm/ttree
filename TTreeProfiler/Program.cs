using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

using NGenerics.DataStructures.Trees;

using TTree;


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

			int seconds = 2;

			var profiler = new Profiler();

			//var ttree = new Tree<string>( 8, 10 );
			//Time( seconds, "T-Tree", s => ttree.Insert( s ), s => ttree.Search( s ) );

			var list = new List<string>();
			profiler.Add( "add", "List<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => list.Add( s ), values.Count, i ) );
			profiler.Add( "search", "List<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => list.IndexOf( s ), list.Count - 1, i ) );

			var sortedList = new SortedList<string, string>();
			profiler.Add( "add", "SortedList<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => sortedList.Add( s, s ), values.Count, i ) );
			profiler.Add( "search", "SortedList<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { sortedList.IndexOfKey( s ); }, sortedList.Count - 1, i ) );

			var sortedDictionary = new SortedDictionary<string, string>();
			profiler.Add( "add", "SortedDictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => sortedDictionary.Add( s, s ), values.Count, i ) );
			profiler.Add( "search", "SortedDictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { string x = sortedDictionary[ s ]; }, sortedDictionary.Count - 1, i ) );

			var arrayList = new ArrayList();
			profiler.Add( "add", "ArrayList", i => Time( values, seconds, i.Desc + " - " + i.Group, s => arrayList.Add( s ), values.Count, i ) );
			profiler.Add( "search", "ArrayList", i => Time( values, seconds, i.Desc + " - " + i.Group, s => arrayList.IndexOf( s ), arrayList.Count - 1, i ) );

			var array = new string[ max ];
			profiler.Add( "add", "array[]", i => Time( values, seconds, i.Desc + " - " + i.Group, s => array[ i.Count ] = s, values.Count, i ) );
			profiler.Add( "search", "array[]", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { var x = (from a in array where a == s select a).First(); }, array.Length - 1, i ) );

			var binTree = new BinarySearchTree<string,string>();
			profiler.Add( "add", "BinarySearchTree", i => Time( values, seconds, i.Desc + " - " + i.Group, s => binTree.Add( s, s ), values.Count, i ) );
			profiler.Add( "search", "BinarySearchTree", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { string x = binTree[ s ]; }, binTree.Count - 1, i ) );

			var redBlackTree = new RedBlackTree<string, string>();
			profiler.Add( "add", "RedBlackTree", i => Time( values, seconds, i.Desc + " - " + i.Group, s => redBlackTree.Add( s, s ), values.Count, i ) );
			profiler.Add( "search", "RedBlackTree", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { string x = redBlackTree[ s ]; }, redBlackTree.Count - 1, i ) );

			profiler.Profile();
			Console.WriteLine();
			Console.WriteLine( "----" );
			Console.WriteLine();
			profiler.PrintResults(); 
		}

		private static void Time( List<Guid> values, int seconds, string desc, Action<string> method, int modCount, Item item )
		{
			item.Count = 0;

			var stop = new Stopwatch();
			stop.Start();
			while( stop.Elapsed.TotalSeconds < seconds )
			{
				method( values[ (int)item.Count % modCount ].ToString() );

				item.Count++;

				if( item.Count >= values.Count )
				{
					Console.WriteLine( "   All values used, exiting test" );
					break;
				}
			}
			stop.Stop();
			item.Elapsed = stop.Elapsed;

			Console.WriteLine( "{1,12} in {2}, {0}", desc, item.Count.ToString( "#,#" ), stop.Elapsed );
		}

		private static List<Guid> Gen( int max )
		{
			var items = new List<Guid>();

			for( int i = 0; i < max; ++i )
			{
				if( i % 1000000 == 0 )
					Console.WriteLine( "{0:F2}%", ((float)i / (float)max) * 100 );

				items.Add( Guid.NewGuid() );
			}

			return items;
		}
	}
}
