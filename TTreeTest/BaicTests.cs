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
			var tree = new Tree<int>( 5, 5 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			tree.Insert( 3 );

			CheckTree( tree, 0, new[] { 3, 5, 7, 10, 15 }, false, false, null );
		}
		
		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsNotBounding()
		{
			var tree = new Tree<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 5 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree( tree, 0, new[] { 5, 7, 10, 15 }, false, false, null );

			tree.Insert( 2 );
			CheckTree( tree, 1, new[] { 5, 7, 10, 15 }, true, false, null );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree );
		}

		[TestMethod]
		public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsBounding()
		{
			var tree = new Tree<int>( 4, 4 );

			tree.Insert( 10 );
			tree.Insert( 2 );
			tree.Insert( 15 );
			tree.Insert( 7 );
			CheckTree( tree, 0, new[] { 2, 7, 10, 15 }, false, false, null );

			tree.Insert( 8 );
			CheckTree( tree, 1, new[] { 7, 8, 10, 15 }, true, false, null );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree );
		}

		[TestMethod]
		public void Rotate_LL()
		{
			var tree = new Tree<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree( tree, 0, new[] { 5 }, false, false, null );

			tree.Insert( 3 );
			CheckTree( tree, 1, new[] { 5 }, true, false, null );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree );

			tree.Insert( 2 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 3 }, true, true, null );
			CheckTree( tree.Left, 0, new[] { 2 }, false, false, tree );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree );
		}

		[TestMethod]
		public void Rotate_RR()
		{
			var tree = new Tree<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree( tree, 0, new[] { 3 }, false, false, null );

			tree.Insert( 5 );
			CheckTree( tree, 1, new[] { 3 }, false, true, null );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree );

			tree.Insert( 7 );
			tree = tree.Root;
			CheckTree( tree, 1, new[] { 5 }, true, true, null );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree );
			CheckTree( tree.Right, 0, new[] { 7 }, false, false, tree );
		}

		[TestMethod]
		public void Rotate_LR()
		{
			var tree = new Tree<int>( 1, 1 );

			tree.Insert( 5 );
			CheckTree( tree, 0, new[] { 5 }, false, false, null );

			tree.Insert( 3 );
			CheckTree( tree, 1, new[] { 5 }, true, false, null );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree );

			tree.Insert( 4 );
			CheckTree( tree, 1, new[] { 4 }, true, true, null );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree );
		}

		[TestMethod]
		public void Rotate_RL()
		{
			var tree = new Tree<int>( 1, 1 );

			tree.Insert( 3 );
			CheckTree( tree, 0, new[] { 3 }, false, false, null );

			tree.Insert( 5 );
			CheckTree( tree, 1, new[] { 3 }, false, true, null );
			CheckTree( tree.Right, 0, new[] { 3 }, false, false, tree );

			tree.Insert( 4 );
			CheckTree( tree, 1, new[] { 4 }, true, true, null );
			CheckTree( tree.Left, 0, new[] { 3 }, false, false, tree );
			CheckTree( tree.Right, 0, new[] { 5 }, false, false, tree );
		}

		private void CheckTree<T>(
			Tree<T> tree,
			int height,
			T[] data,
			bool hasLeft,
			bool hasRight,
			Tree<T> parent )
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
		}

		public TestContext TestContext { get; set; }
	}
}
