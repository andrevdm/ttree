using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

using NGenerics.DataStructures.Trees;

using TTree;
using System.Reflection;
using System.Reflection.Emit;


namespace TTreeProfiler
{
	class Program
	{
		static void Main( string[] args )
		{
			Console.WriteLine( "Type of profile" );
			Console.WriteLine( " 1 - Compare collection performance");
			Console.WriteLine( " 2 - Insert loop for external profiler" );

			char c = Console.ReadKey( true ).KeyChar;

			switch( c )
			{
				case '1': CompareCollectionPerformance();
					break;

				case '2': InsertLoop();
					break;

				default:
					Console.WriteLine( "Unknown option: " + c );
					break;
			}
		}

		private static void InsertLoop()
		{
			Console.WriteLine( "Number of iteration? " );
			int max = int.Parse( Console.ReadLine() );

			Console.WriteLine( "Tree min order (max = min + 3)?" );
			int orderMin = int.Parse( Console.ReadLine() );

			TTreeRoot<string> root = new TTreeRoot<string>( orderMin, orderMin + 3 );

			for( int i = 0; i < max; ++i )
			{
				root.Insert( Guid.NewGuid().ToString() );
			}
		}

		private static void CompareCollectionPerformance()
		{
			Console.WriteLine( "Run each test for seconds?" );
			int seconds = int.Parse( Console.ReadLine() );

			int max = 10000000;
			List<Guid> values = Gen( max );
			Console.WriteLine();
			Console.WriteLine( "Values created, profiling" );

			var profiler = new Profiler();

			//profiler.AddCombine( "add", 20 );
			//profiler.AddCombine( "del", 20 );
			//profiler.AddCombine( "search", 60 );
			profiler.AddCombine( "add", 25 );
			profiler.AddCombine( "search", 75 );

			int[][] ttreeOrders = new int[][] { new[] { 40, 43 }, new[] { 97, 100 }, new[] { 17, 20 }, new[] { 7, 10 }, new[] { 497, 500 }, new[] { 2000, 2003 }, };
			List<TTreeRoot<string>> trees = new List<TTreeRoot<string>>();

			for( int tpos = 0; tpos < ttreeOrders.Length; ++tpos )
			{
				int pos = tpos;
				var order = ttreeOrders[ pos ];
				string name = string.Format( "{0}-{1}", order[ 0 ], order[ 1 ] );

				trees.Add( new TTreeRoot<string>( order[ 0 ], order[ 1 ] ) );
				profiler.Add( "add", "T-Tree(" + name + ")", i => Time( values, seconds, i.Desc + " - " + i.Group, s => trees[ pos ].Insert( s ), values.Count, i ) );
				profiler.Add( "search", "T-Tree(" + name + ")", i => Time( values, seconds, i.Desc + " - " + i.Group, s => trees[ pos ].Search( s ), trees[ pos ].Count - 1, i ) );
			}


			var list = new List<string>();
			profiler.Add( "add", "List<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => list.Add( s ), values.Count, i ) );
			profiler.Add( "search", "List<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => list.IndexOf( s ), list.Count - 1, i ) );

			//var dict = new Dictionary<string, string>();
			//profiler.Add( "add", "Dictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => dict.Add( s, s ), values.Count, i ) );
			//profiler.Add( "search", "Dictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { string x = dict[ s ]; }, dict.Count - 1, i ) );

			var sortedList = new SortedList<string, string>();
			profiler.Add( "add", "SortedList<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => sortedList.Add( s, s ), values.Count, i ) );
			profiler.Add( "search", "SortedList<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { sortedList.IndexOfKey( s ); }, sortedList.Count - 1, i ) );

			//var sortedDictionary = new SortedDictionary<string, string>();
			//profiler.Add( "add", "SortedDictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => sortedDictionary.Add( s, s ), values.Count, i ) );
			//profiler.Add( "search", "SortedDictionary<>", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { string x = sortedDictionary[ s ]; }, sortedDictionary.Count - 1, i ) );

			var arrayList = new ArrayList();
			profiler.Add( "add", "ArrayList", i => Time( values, seconds, i.Desc + " - " + i.Group, s => arrayList.Add( s ), values.Count, i ) );
			profiler.Add( "search", "ArrayList", i => Time( values, seconds, i.Desc + " - " + i.Group, s => arrayList.IndexOf( s ), arrayList.Count - 1, i ) );

			var array = new string[ max ];
			profiler.Add( "add", "array[]", i => Time( values, seconds, i.Desc + " - " + i.Group, s => array[ i.Count ] = s, values.Count, i ) );
			profiler.Add( "search", "array[]", i => Time( values, seconds, i.Desc + " - " + i.Group, s => { var x = (from a in array where a == s select a).First(); }, array.Length - 1, i ) );

			var binTree = new BinarySearchTree<string, string>();
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
					Console.WriteLine( "{0:F2}%", ((double)i / (double)max) * 100 );

				items.Add( Guid.NewGuid() );
			}

			return items;
		}

		private struct StringStruct : IComparable<StringStruct>
		{
			public string value { get; set; }

			public int CompareTo( StringStruct obj )
			{
				return value.CompareTo( obj.value );
			}
		}

		private class StringComparer : IComparer<string>
		{
			public int Compare( string x, string y )
			{
				return x.CompareTo( y );
			}
		}
	}


}
