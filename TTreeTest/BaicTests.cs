using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TTree;
using System.Collections;

namespace TTreeTest
{
	[TestClass]
	public class BaicTests
	{
		[TestMethod]
		public void InsertIntoRootWithSpace()
		{
			var tree = new TreeNode<int>( 5, 5 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			tree.Insert( 3 );

			CheckTree( tree, 0, new[] { 3, 5, 7, 10, 15 }, false, false, null, 3 );
		}

		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsNotBounding()
		{
			var tree = new TreeNode<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree( tree, 0, new[] { 5, 7, 10, 15 }, false, false, null, 5 );

			tree.Insert( 2 );
			CheckTree( tree, 1, new[] { 5, 7, 10, 15 }, true, false, null, 5 );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree, 5 );
		}

		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsBounding()
		{
			var tree = new TreeNode<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 2 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree( tree, 0, new[] { 2, 7, 10, 15 }, false, false, null, 2 );

			tree.Insert( 8 );
			CheckTree( tree, 1, new[] { 7, 8, 10, 15 }, true, false, null, 7 );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree, 7 );
		}

		[TestMethod]
		public void Rotate_LL()
		{
			var tree = new TreeNode<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree( tree, 0, new[] { 5 }, false, false, null, 5 );

			tree.Insert( 3 );
			CheckTree( tree, 1, new[] { 5 }, true, false, null, 5 );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree, 5 );

			tree.Insert( 2 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 3 }, true, true, null, 3 );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree, 3 );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree, 3 );
		}

		[TestMethod]
		public void Rotate_RR()
		{
			var tree = new TreeNode<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree( tree, 0, new[] { 3 }, false, false, null, 3 );

			tree.Insert( 5 );
			CheckTree( tree, 1, new[] { 3 }, false, true, null, 3 );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree, 3 );

			tree.Insert( 7 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 5 }, true, true, null, 5 );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree, 5 );
			CheckTree( tree.Right, 0, new[] { 7 }, false, false, tree, 5 );
		}

		[TestMethod]
		public void Rotate_LR()
		{
			var tree = new TreeNode<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree( tree, 0, new[] { 5 }, false, false, null, 5 );

			tree.Insert( 3 );
			CheckTree( tree, 1, new[] { 5 }, true, false, null, 5 );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree, 5 );

			tree.Insert( 4 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 4 }, true, true, null, 4 );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree, 4 );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree, 4 );
		}

		[TestMethod]
		public void Rotate_RL()
		{
			var tree = new TreeNode<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree( tree, 0, new[] { 3 }, false, false, null, 3 );

			tree.Insert( 5 );
			CheckTree( tree, 1, new[] { 3 }, false, true, null, 3 );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree, 3 );

			tree.Insert( 4 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 4 }, true, true, null, 4 );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree, 4 );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree, 4 );
		}

		[TestMethod]
		public void Rotate_LR_SlidingRotate()
		{
			var tree = new TreeNode<int>( 3, 3 );

			tree.Insert( 20 );
			tree.Insert( 21 );
			tree.Insert( 22 );
			CheckTree( tree, 0, new[] { 20, 21, 22 }, false, false, null, 20 );

			tree.Insert( 10 );
			tree.Insert( 11 );
			tree.Insert( 12 );
			CheckTree( tree, 1, new[] { 20, 21, 22 }, true, false, null, 20 );
			CheckTree( tree.Left, 0, new[] { 10, 11, 12 }, false, false, tree, 20 );

			tree.Insert( 15 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 11, 12, 15 }, true, true, null, 11 );
			CheckTree( tree.Left, 0, new[] { 10 }, false, false, tree, 11 );
			CheckTree( tree.Right, 0, new[] { 20, 21, 22 }, false, false, tree, 11 );
		}

		[TestMethod]
		public void Rotate_LR_SlidingRotate2()
		{
			var tree = new TreeNode<int>( 6, 6 );

			tree.Insert( 201 );
			tree.Insert( 202 );
			tree.Insert( 203 );
			tree.Insert( 204 );
			tree.Insert( 205 );
			tree.Insert( 206 );
			CheckTree( tree, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, null, 201 );

			tree.Insert( 101 );
			tree.Insert( 102 );
			tree.Insert( 103 );
			tree.Insert( 104 );
			tree.Insert( 105 );
			tree.Insert( 106 );
			CheckTree( tree, 1, new[] { 201, 202, 203, 204, 205, 206 }, true, false, null, 201 );
			CheckTree( tree.Left, 0, new[] { 101, 102, 103, 104, 105, 106 }, false, false, tree, 201 );

			tree.Insert( 115 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 102, 103, 104, 105, 106, 115 }, true, true, null, 102 );
			CheckTree( tree.Left, 0, new[] { 101 }, false, false, tree, 102 );
			CheckTree( tree.Right, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, tree, 102 );
		}

		[TestMethod]
		public void Rotate_RL_SlidingRotate()
		{
			var tree = new TreeNode<int>( 3, 3 );

			tree.Insert( 30 );
			tree.Insert( 31 );
			tree.Insert( 32 );
			CheckTree( tree, 0, new[] { 30, 31, 32 }, false, false, null, 30 );

			tree.Insert( 50 );
			tree.Insert( 51 );
			tree.Insert( 52 );
			CheckTree( tree, 1, new[] { 30, 31, 32 }, false, true, null, 30 );
			CheckTree( tree.Right, 0, new[] { 50, 51, 52 }, false, false, tree, 30 );

			tree.Insert( 40 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 40, 50, 51 }, true, true, null, 40 );
			CheckTree( tree.Left, 0, new[] { 30, 31, 32 }, false, false, tree, 40 );
			CheckTree( tree.Right, 0, new[] { 52 }, false, false, tree, 40 );
		}

		[TestMethod]
		public void Rotate_RL_SlidingRotate2()
		{
			var tree = new TreeNode<int>( 6, 6 );

			tree.Insert( 30 );
			tree.Insert( 31 );
			tree.Insert( 32 );
			tree.Insert( 33 );
			tree.Insert( 34 );
			tree.Insert( 35 );
			CheckTree( tree, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, null, 30 );

			tree.Insert( 50 );
			tree.Insert( 51 );
			tree.Insert( 52 );
			tree.Insert( 53 );
			tree.Insert( 54 );
			tree.Insert( 55 );
			CheckTree( tree, 1, new[] { 30, 31, 32, 33, 34, 35 }, false, true, null, 30 );
			CheckTree( tree.Right, 0, new[] { 50, 51, 52, 53, 54, 55 }, false, false, tree, 30 );

			tree.Insert( 40 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 40, 50, 51, 52, 53, 54 }, true, true, null, 40 );
			CheckTree( tree.Left, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, tree, 40 );
			CheckTree( tree.Right, 0, new[] { 55 }, false, false, tree, 40 );
		}

		[TestMethod]
		public void Search()
		{
			var tree = new TreeNode<int>( 1, 1 );

			tree.Insert( 70 );
			tree.Insert( 20 );
			tree.Insert( 10 );
			tree.Insert( 50 );
			tree.Insert( 90 );

			tree = tree.Root;
			Assert.AreEqual( 90, tree.Search( 90 ) );
			Assert.AreEqual( 70, tree.Search( 70 ) );
			Assert.AreEqual( 50, tree.Search( 50 ) );
			Assert.AreEqual( 20, tree.Search( 20 ) );
			Assert.AreEqual( 10, tree.Search( 10 ) );
		}

		[TestMethod]
		public void CustomSearch()
		{
			var tree = new TreeNode<string>( 1, 1 );

			tree.Insert( "70" );
			tree.Insert( "20" );
			tree.Insert( "10" );
			tree.Insert( "50" );
			tree.Insert( "90" );

			tree = tree.Root;
			Assert.AreEqual( "90", tree.Search( 90, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "70", tree.Search( 70, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "50", tree.Search( 50, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "20", tree.Search( 20, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "10", tree.Search( 10, ( x, y ) => x.ToString().CompareTo( y ) ) );
		}

		[TestMethod]
		public void CheckThatAllInsertedItemsExist()
		{
			var tree = new TreeNode<int>( 20, 23 );

			for( int i = 0; i < 1000; ++i )
			{
				tree.Root.Insert( i );
			}

			tree = tree.Root;

			for( int i = 0; i < 1000; ++i )
			{
				Assert.AreEqual( i, tree.Search( i ) );
			}
		}


		private void CheckTree<T>(
			TreeNode<T> tree,
			int height,
			T[] data,
			bool hasLeft,
			bool hasRight,
			TreeNode<T> parent,
			T root1stItem )
			where T : IComparable
		{
			Assert.AreEqual( data.Length, tree.Count, "Invalid count" );

			T[] values = new T[ tree.MaxItems ];
			tree.CopyItems( values, 0 );

			for( int i = 0; i < data.Length; ++i )
			{
				Assert.AreEqual( data[ i ], values[ i ], "Invalid value item " + i );
			}

			Assert.AreEqual( hasLeft, (tree.Left != null), "Invalid has left value" );
			Assert.AreEqual( hasRight, (tree.Right != null), "Invalid has right value" );
			Assert.AreSame( parent, tree.Parent, "Incorrect parent node" );
			Assert.AreEqual( height, tree.Height, "Invalid height" );
			Assert.AreEqual( root1stItem, tree.Root[ 0 ], "Invalid root item 0, incorrect root node" );
		}

		public TestContext TestContext { get; set; }
	}
}
