using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TTree;

namespace TTreeTest
{
	[TestClass]
	public class BaicTests
	{
		[TestMethod]
		public void InsertIntoRootWithSpace()
		{
			var tree = new Tree<int>( 10, 13 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			tree.Insert( 3 );

			var data = new int[ 13 ];
			tree.CopyArray( data, 0 );

			Assert.AreEqual( 5, tree.Count, "Invalid count" );
			Assert.AreEqual( 3, data[ 0 ], "Invalid value at index 0" );
			Assert.AreEqual( 5, data[ 1 ], "Invalid value at index 1" );
			Assert.AreEqual( 7, data[ 2 ], "Invalid value at index 2" );
			Assert.AreEqual( 10, data[ 3 ], "Invalid value at index 3" );
			Assert.AreEqual( 15, data[ 4 ], "Invalid value at index 4" );
		}
		
		public TestContext TestContext { get; set; }
	}
}
