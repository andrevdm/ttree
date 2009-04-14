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
			var tree = new TTreeRoot<int>( 5, 5 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			tree.Insert( 3 );

			CheckTree<int>( tree.RootNode, 0, new[] { 3, 5, 7, 10, 15 }, false, false, null, 3 );
		}

		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsNotBounding()
		{
			var tree = new TTreeRoot<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree<int>( tree.RootNode, 0, new[] { 5, 7, 10, 15 }, false, false, null, 5 );

			tree.Insert( 2 );
			CheckTree<int>( tree.RootNode, 1, new[] { 5, 7, 10, 15 }, true, false, null, 5 );
			CheckTree<int>( tree.Left, 0, new[] { 2 }, false, false, tree.RootNode, 5 );
		}

		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsBounding()
		{
			var tree = new TTreeRoot<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 2 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree<int>( tree.RootNode, 0, new[] { 2, 7, 10, 15 }, false, false, null, 2 );

			tree.Insert( 8 );
			CheckTree<int>( tree.RootNode, 1, new[] { 7, 8, 10, 15 }, true, false, null, 7 );
			CheckTree<int>( tree.Left, 0, new[] { 2 }, false, false, tree.RootNode, 7 );
		}

		[TestMethod]
		public void Rotate_LL()
		{
			var tree = new TTreeRoot<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree<int>( tree.RootNode, 0, new[] { 5 }, false, false, null, 5 );

			tree.Insert( 3 );
			CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, false, null, 5 );
			CheckTree<int>( tree.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );

			tree.Insert( 2 );
			CheckTree<int>( tree.RootNode, 1, new[] { 3 }, true, true, null, 3 );
			CheckTree<int>( tree.Left, 0, new[] { 2 }, false, false, tree.RootNode, 3 );
			CheckTree<int>( tree.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );
		}

		[TestMethod]
		public void Rotate_RR()
		{
			var tree = new TTreeRoot<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree<int>( tree.RootNode, 0, new[] { 3 }, false, false, null, 3 );

			tree.Insert( 5 );
			CheckTree<int>( tree.RootNode, 1, new[] { 3 }, false, true, null, 3 );
			CheckTree<int>( tree.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );

			tree.Insert( 7 );
			CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, true, null, 5 );
			CheckTree<int>( tree.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );
			CheckTree<int>( tree.Right, 0, new[] { 7 }, false, false, tree.RootNode, 5 );
		}

		[TestMethod]
		public void Rotate_LR()
		{
			var tree = new TTreeRoot<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree<int>( tree.RootNode, 0, new[] { 5 }, false, false, null, 5 );

			tree.Insert( 3 );
			CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, false, null, 5 );
			CheckTree<int>( tree.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );

			tree.Insert( 4 );
			CheckTree<int>( tree.RootNode, 1, new[] { 4 }, true, true, null, 4 );
			CheckTree<int>( tree.Left, 0, new[] { 3 }, false, false, tree.RootNode, 4 );
			CheckTree<int>( tree.Right, 0, new[] { 5 }, false, false, tree.RootNode, 4 );
		}

		[TestMethod]
		public void Rotate_RL()
		{
			var tree = new TTreeRoot<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree<int>( tree.RootNode, 0, new[] { 3 }, false, false, null, 3 );

			tree.Insert( 5 );
			CheckTree<int>( tree.RootNode, 1, new[] { 3 }, false, true, null, 3 );
			CheckTree<int>( tree.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );

			tree.Insert( 4 );
			CheckTree<int>( tree.RootNode, 1, new[] { 4 }, true, true, null, 4 );
			CheckTree<int>( tree.Left, 0, new[] { 3 }, false, false, tree.RootNode, 4 );
			CheckTree<int>( tree.Right, 0, new[] { 5 }, false, false, tree.RootNode, 4 );
		}

		[TestMethod]
		public void Rotate_LR_SlidingRotate()
		{
			var tree = new TTreeRoot<int>( 3, 3 );

			tree.Insert( 20 );
			tree.Insert( 21 );
			tree.Insert( 22 );
			CheckTree<int>( tree.RootNode, 0, new[] { 20, 21, 22 }, false, false, null, 20 );

			tree.Insert( 10 );
			tree.Insert( 11 );
			tree.Insert( 12 );
			CheckTree<int>( tree.RootNode, 1, new[] { 20, 21, 22 }, true, false, null, 20 );
			CheckTree<int>( tree.Left, 0, new[] { 10, 11, 12 }, false, false, tree.RootNode, 20 );

			tree.Insert( 15 );
			CheckTree<int>( tree.RootNode, 1, new[] { 11, 12, 15 }, true, true, null, 11 );
			CheckTree<int>( tree.Left, 0, new[] { 10 }, false, false, tree.RootNode, 11 );
			CheckTree<int>( tree.Right, 0, new[] { 20, 21, 22 }, false, false, tree.RootNode, 11 );
		}

		[TestMethod]
		public void Rotate_LR_SlidingRotate2()
		{
			var tree = new TTreeRoot<int>( 6, 6 );

			tree.Insert( 201 );
			tree.Insert( 202 );
			tree.Insert( 203 );
			tree.Insert( 204 );
			tree.Insert( 205 );
			tree.Insert( 206 );
			CheckTree<int>( tree.RootNode, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, null, 201 );

			tree.Insert( 101 );
			tree.Insert( 102 );
			tree.Insert( 103 );
			tree.Insert( 104 );
			tree.Insert( 105 );
			tree.Insert( 106 );
			CheckTree<int>( tree.RootNode, 1, new[] { 201, 202, 203, 204, 205, 206 }, true, false, null, 201 );
			CheckTree<int>( tree.Left, 0, new[] { 101, 102, 103, 104, 105, 106 }, false, false, tree.RootNode, 201 );

			tree.Insert( 115 );
			CheckTree<int>( tree.RootNode, 1, new[] { 102, 103, 104, 105, 106, 115 }, true, true, null, 102 );
			CheckTree<int>( tree.Left, 0, new[] { 101 }, false, false, tree.RootNode, 102 );
			CheckTree<int>( tree.Right, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, tree.RootNode, 102 );
		}

		[TestMethod]
		public void Rotate_RL_SlidingRotate()
		{
			var tree = new TTreeRoot<int>( 3, 3 );

			tree.Insert( 30 );
			tree.Insert( 31 );
			tree.Insert( 32 );
			CheckTree<int>( tree.RootNode, 0, new[] { 30, 31, 32 }, false, false, null, 30 );

			tree.Insert( 50 );
			tree.Insert( 51 );
			tree.Insert( 52 );
			CheckTree<int>( tree.RootNode, 1, new[] { 30, 31, 32 }, false, true, null, 30 );
			CheckTree<int>( tree.Right, 0, new[] { 50, 51, 52 }, false, false, tree.RootNode, 30 );

			tree.Insert( 40 );
			CheckTree<int>( tree.RootNode, 1, new[] { 40, 50, 51 }, true, true, null, 40 );
			CheckTree<int>( tree.Left, 0, new[] { 30, 31, 32 }, false, false, tree.RootNode, 40 );
			CheckTree<int>( tree.Right, 0, new[] { 52 }, false, false, tree.RootNode, 40 );
		}

		[TestMethod]
		public void Rotate_RL_SlidingRotate2()
		{
			var tree = new TTreeRoot<int>( 6, 6 );

			tree.Insert( 30 );
			tree.Insert( 31 );
			tree.Insert( 32 );
			tree.Insert( 33 );
			tree.Insert( 34 );
			tree.Insert( 35 );
			CheckTree<int>( tree.RootNode, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, null, 30 );

			tree.Insert( 50 );
			tree.Insert( 51 );
			tree.Insert( 52 );
			tree.Insert( 53 );
			tree.Insert( 54 );
			tree.Insert( 55 );
			CheckTree<int>( tree.RootNode, 1, new[] { 30, 31, 32, 33, 34, 35 }, false, true, null, 30 );
			CheckTree<int>( tree.Right, 0, new[] { 50, 51, 52, 53, 54, 55 }, false, false, tree.RootNode, 30 );

			tree.Insert( 40 );
			CheckTree<int>( tree.RootNode, 1, new[] { 40, 50, 51, 52, 53, 54 }, true, true, null, 40 );
			CheckTree<int>( tree.Left, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, tree.RootNode, 40 );
			CheckTree<int>( tree.Right, 0, new[] { 55 }, false, false, tree.RootNode, 40 );
		}

		[TestMethod]
		public void Search()
		{
			var tree = new TTreeRoot<int>( 1, 1 );

			tree.Insert( 70 );
			tree.Insert( 20 );
			tree.Insert( 10 );
			tree.Insert( 50 );
			tree.Insert( 90 );

			Assert.AreEqual( 90, tree.Search( 90 ) );
			Assert.AreEqual( 70, tree.Search( 70 ) );
			Assert.AreEqual( 50, tree.Search( 50 ) );
			Assert.AreEqual( 20, tree.Search( 20 ) );
			Assert.AreEqual( 10, tree.Search( 10 ) );
		}

		[TestMethod]
		public void CustomSearch()
		{
			var tree = new TTreeRoot<string>( 1, 1 );

			tree.Insert( "70" );
			tree.Insert( "20" );
			tree.Insert( "10" );
			tree.Insert( "50" );
			tree.Insert( "90" );

			Assert.AreEqual( "90", tree.Search( 90, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "70", tree.Search( 70, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "50", tree.Search( 50, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "20", tree.Search( 20, ( x, y ) => x.ToString().CompareTo( y ) ) );
			Assert.AreEqual( "10", tree.Search( 10, ( x, y ) => x.ToString().CompareTo( y ) ) );
		}

		[TestMethod]
		public void CheckThatAllInsertedItemsExist()
		{
			var tree = new TTreeRoot<int>( 20, 23 );

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

		[TestMethod]
		public void CheckHeightAfterMultiLevelInsert()
		{
			var tree = new TTreeRoot<int>( 2, 2 );

			tree.Insert( 10 );
			tree.Insert( 20 );
			tree.Insert( 30 );
			tree.Insert( 40 );
			tree.Insert( 50 );
			tree.Insert( 60 );
			tree.Insert( 70 );
			tree.Insert( 80 );
			tree.Insert( 90 );

			CheckTree<int>( tree.RootNode, 2, new[] { 30, 40 }, true, true, null, 30 );

			CheckTree<int>( tree.RootNode.Left, 0, new[] { 10, 20 }, false, false, tree.RootNode, 30 );
			CheckTree<int>( tree.RootNode.Right, 1, new[] { 70, 80 }, true, true, tree.RootNode, 30 );

			CheckTree<int>( tree.RootNode.Right.Left, 0, new[] { 50, 60 }, false, false, tree.RootNode.Right, 30 );
			CheckTree<int>( tree.RootNode.Right.Right, 0, new[] { 90 }, false, false, tree.RootNode.Right, 30 );
		}

		[TestMethod]
		public void Delete_From_NodeNoUnderflow_RemovesItem()
		{
			var tree = new TTreeRoot<int>( 2, 3 );

			tree.Insert( 10 );
			tree.Insert( 11 );
			tree.Insert( 12 );
			tree.Insert( 4 );
			tree.Insert( 5 );

			tree.Delete( 11 );

			CheckTree<int>( tree.RootNode, 1, new[] { 10, 12 }, true, false, null, 10 );
			CheckTree<int>( tree.RootNode.Left, 0, new[] { 4, 5 }, false, false, tree.RootNode, 10 );
		}

		[TestMethod]
		public void Delete_From_Leaf_RemovesItem()
		{
			var tree = new TTreeRoot<int>( 3, 3 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			
			tree.Delete( 10 );
			
			CheckTree<int>( tree.RootNode, 0, new[] { 5, 15 }, false, false, null, 5 );
		}

		[TestMethod]
		public void Delete_From_HalfLeaf_RemovesItem()
		{
			var tree = new TTreeRoot<int>( 3, 3 );

			tree.Insert( 10 );
			tree.Insert( 11 );
			tree.Insert( 12 );
			tree.Insert( 4 );
			tree.Insert( 5 );

			tree.Delete( 12 );

			CheckTree<int>( tree.RootNode, 1, new[] { 10, 11 }, true, false, null, 10 );
			CheckTree<int>( tree.RootNode.Left, 0, new[] { 4, 5 }, false, false, null, 10 );
		}

		[TestMethod]
		public void Delete_From_InternalNode_BorrowTheGreatestLowerBound()
		{
			Assert.Fail();
		}

		private void CheckTree<T>(
			TTreeNode<T> tree,
			int height,
			T[] data,
			bool hasLeft,
			bool hasRight,
			TTreeNode<T> parent,
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