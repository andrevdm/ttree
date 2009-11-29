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
    public class BasicTests
    {
        [TestMethod]
        public void InsertIntoRootWithSpace()
        {
            var tree = new TTree<int>( 5, 5 );

            tree.Add( 10 );
            tree.Add( 5 );
            tree.Add( 15 );
            tree.Add( 7 );
            tree.Add( 3 );

            CheckTree<int>( tree.RootNode, 0, new[] { 3, 5, 7, 10, 15 }, false, false, null, 3 );
        }

        [TestMethod]
        public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsNotBounding()
        {
            var tree = new TTree<int>( 4, 4 );

            tree.Add( 10 );
            tree.Add( 5 );
            tree.Add( 15 );
            tree.Add( 7 );
            CheckTree<int>( tree.RootNode, 0, new[] { 5, 7, 10, 15 }, false, false, null, 5 );

            tree.Add( 2 );
            CheckTree<int>( tree.RootNode, 1, new[] { 5, 7, 10, 15 }, true, false, null, 5 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 2 }, false, false, tree.RootNode, 5 );
        }

        [TestMethod]
        public void Insert_FullRoot_OverflowLeftToNewNode_LastNodeIsBounding()
        {
            var tree = new TTree<int>( 4, 4 ) 
            {
                10, 
                2, 
                15, 
                7
            };

            CheckTree<int>( tree.RootNode, 0, new[] { 2, 7, 10, 15 }, false, false, null, 2 );

            tree.Add( 8 );
            CheckTree<int>( tree.RootNode, 1, new[] { 7, 8, 10, 15 }, true, false, null, 7 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 2 }, false, false, tree.RootNode, 7 );
        }

        [TestMethod]
        public void Rotate_LL()
        {
            var tree = new TTree<int>( 1, 1 );

            tree.Add( 5 );
            CheckTree<int>( tree.RootNode, 0, new[] { 5 }, false, false, null, 5 );

            tree.Add( 3 );
            CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, false, null, 5 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );

            tree.Add( 2 );
            CheckTree<int>( tree.RootNode, 1, new[] { 3 }, true, true, null, 3 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 2 }, false, false, tree.RootNode, 3 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );
        }

        [TestMethod]
        public void Rotate_RR()
        {
            var tree = new TTree<int>( 1, 1 );

            tree.Add( 3 );
            CheckTree<int>( tree.RootNode, 0, new[] { 3 }, false, false, null, 3 );

            tree.Add( 5 );
            CheckTree<int>( tree.RootNode, 1, new[] { 3 }, false, true, null, 3 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );

            tree.Add( 7 );
            CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, true, null, 5 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 7 }, false, false, tree.RootNode, 5 );
        }

        [TestMethod]
        public void Rotate_LR()
        {
            var tree = new TTree<int>( 1, 1 );

            tree.Add( 5 );
            CheckTree<int>( tree.RootNode, 0, new[] { 5 }, false, false, null, 5 );

            tree.Add( 3 );
            CheckTree<int>( tree.RootNode, 1, new[] { 5 }, true, false, null, 5 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 3 }, false, false, tree.RootNode, 5 );

            tree.Add( 4 );
            CheckTree<int>( tree.RootNode, 1, new[] { 4 }, true, true, null, 4 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 3 }, false, false, tree.RootNode, 4 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 5 }, false, false, tree.RootNode, 4 );
        }

        [TestMethod]
        public void Rotate_RL()
        {
            var tree = new TTree<int>( 1, 1 );

            tree.Add( 3 );
            CheckTree<int>( tree.RootNode, 0, new[] { 3 }, false, false, null, 3 );

            tree.Add( 5 );
            CheckTree<int>( tree.RootNode, 1, new[] { 3 }, false, true, null, 3 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 5 }, false, false, tree.RootNode, 3 );

            tree.Add( 4 );
            CheckTree<int>( tree.RootNode, 1, new[] { 4 }, true, true, null, 4 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 3 }, false, false, tree.RootNode, 4 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 5 }, false, false, tree.RootNode, 4 );
        }

        [TestMethod]
        public void Rotate_LR_SlidingRotate()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 20 );
            tree.Add( 21 );
            tree.Add( 22 );
            CheckTree<int>( tree.RootNode, 0, new[] { 20, 21, 22 }, false, false, null, 20 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            CheckTree<int>( tree.RootNode, 1, new[] { 20, 21, 22 }, true, false, null, 20 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 10, 11, 12 }, false, false, tree.RootNode, 20 );

            tree.Add( 15 );
            CheckTree<int>( tree.RootNode, 1, new[] { 11, 12, 15 }, true, true, null, 11 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 10 }, false, false, tree.RootNode, 11 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 20, 21, 22 }, false, false, tree.RootNode, 11 );
        }

        [TestMethod]
        public void Rotate_LR_SlidingRotate2()
        {
            var tree = new TTree<int>( 6, 6 );

            tree.Add( 201 );
            tree.Add( 202 );
            tree.Add( 203 );
            tree.Add( 204 );
            tree.Add( 205 );
            tree.Add( 206 );
            CheckTree<int>( tree.RootNode, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, null, 201 );

            tree.Add( 101 );
            tree.Add( 102 );
            tree.Add( 103 );
            tree.Add( 104 );
            tree.Add( 105 );
            tree.Add( 106 );
            CheckTree<int>( tree.RootNode, 1, new[] { 201, 202, 203, 204, 205, 206 }, true, false, null, 201 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 101, 102, 103, 104, 105, 106 }, false, false, tree.RootNode, 201 );

            tree.Add( 115 );
            CheckTree<int>( tree.RootNode, 1, new[] { 102, 103, 104, 105, 106, 115 }, true, true, null, 102 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 101 }, false, false, tree.RootNode, 102 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 201, 202, 203, 204, 205, 206 }, false, false, tree.RootNode, 102 );
        }

        [TestMethod]
        public void Rotate_RL_SlidingRotate()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 30 );
            tree.Add( 31 );
            tree.Add( 32 );
            CheckTree<int>( tree.RootNode, 0, new[] { 30, 31, 32 }, false, false, null, 30 );

            tree.Add( 50 );
            tree.Add( 51 );
            tree.Add( 52 );
            CheckTree<int>( tree.RootNode, 1, new[] { 30, 31, 32 }, false, true, null, 30 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 50, 51, 52 }, false, false, tree.RootNode, 30 );

            tree.Add( 40 );
            CheckTree<int>( tree.RootNode, 1, new[] { 40, 50, 51 }, true, true, null, 40 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 30, 31, 32 }, false, false, tree.RootNode, 40 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 52 }, false, false, tree.RootNode, 40 );
        }

        [TestMethod]
        public void Rotate_RL_SlidingRotate2()
        {
            var tree = new TTree<int>( 6, 6 );

            tree.Add( 30 );
            tree.Add( 31 );
            tree.Add( 32 );
            tree.Add( 33 );
            tree.Add( 34 );
            tree.Add( 35 );
            CheckTree<int>( tree.RootNode, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, null, 30 );

            tree.Add( 50 );
            tree.Add( 51 );
            tree.Add( 52 );
            tree.Add( 53 );
            tree.Add( 54 );
            tree.Add( 55 );
            CheckTree<int>( tree.RootNode, 1, new[] { 30, 31, 32, 33, 34, 35 }, false, true, null, 30 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 50, 51, 52, 53, 54, 55 }, false, false, tree.RootNode, 30 );

            tree.Add( 40 );
            CheckTree<int>( tree.RootNode, 1, new[] { 40, 50, 51, 52, 53, 54 }, true, true, null, 40 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 30, 31, 32, 33, 34, 35 }, false, false, tree.RootNode, 40 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 55 }, false, false, tree.RootNode, 40 );
        }

        [TestMethod]
        public void Search()
        {
            var tree = new TTree<int>( 1, 1 );

            tree.Add( 70 );
            tree.Add( 20 );
            tree.Add( 10 );
            tree.Add( 50 );
            tree.Add( 90 );

            Assert.AreEqual( 90, tree.Search( 90 ) );
            Assert.AreEqual( 70, tree.Search( 70 ) );
            Assert.AreEqual( 50, tree.Search( 50 ) );
            Assert.AreEqual( 20, tree.Search( 20 ) );
            Assert.AreEqual( 10, tree.Search( 10 ) );
        }

        [TestMethod]
        public void CustomSearch()
        {
            var tree = new TTree<string>( 1, 1 );

            tree.Add( "70" );
            tree.Add( "20" );
            tree.Add( "10" );
            tree.Add( "50" );
            tree.Add( "90" );

            Assert.AreEqual( "90", tree.Search( 90, ( x, y ) => x.ToString().CompareTo( y ) ) );
            Assert.AreEqual( "70", tree.Search( 70, ( x, y ) => x.ToString().CompareTo( y ) ) );
            Assert.AreEqual( "50", tree.Search( 50, ( x, y ) => x.ToString().CompareTo( y ) ) );
            Assert.AreEqual( "20", tree.Search( 20, ( x, y ) => x.ToString().CompareTo( y ) ) );
            Assert.AreEqual( "10", tree.Search( 10, ( x, y ) => x.ToString().CompareTo( y ) ) );
        }

        [TestMethod]
        public void CheckThatAllInsertedItemsExist()
        {
            var tree = new TTree<int>( 20, 23 );

            for ( int i = 0; i < 1000; ++i )
            {
                tree.Add( i );
            }

            for ( int i = 0; i < 1000; ++i )
            {
                Assert.AreEqual( i, tree.Search( i ) );
            }
        }

        [TestMethod]
        public void CheckHeightAfterMultiLevelInsert()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 10 );
            tree.Add( 20 );
            tree.Add( 30 );
            tree.Add( 40 );
            tree.Add( 50 );
            tree.Add( 60 );
            tree.Add( 70 );
            tree.Add( 80 );
            tree.Add( 90 );

            CheckTree<int>( tree.RootNode, 2, new[] { 30, 40 }, true, true, null, 30 );

            CheckTree<int>( tree.RootNode.Left, 0, new[] { 10, 20 }, false, false, tree.RootNode, 30 );
            CheckTree<int>( tree.RootNode.Right, 1, new[] { 70, 80 }, true, true, tree.RootNode, 30 );

            CheckTree<int>( tree.RootNode.Right.Left, 0, new[] { 50, 60 }, false, false, tree.RootNode.Right, 30 );
            CheckTree<int>( tree.RootNode.Right.Right, 0, new[] { 90 }, false, false, tree.RootNode.Right, 30 );
        }

        [TestMethod]
        public void Delete_From_NodeNoUnderflow_RemovesItem()
        {
            var tree = new TTree<int>( 2, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 4 );
            tree.Add( 5 );

            tree.Remove( 11 );

            CheckTree<int>( tree.RootNode, 1, new[] { 10, 12 }, true, false, null, 10 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 4, 5 }, false, false, tree.RootNode, 10 );
        }

        [TestMethod]
        public void Delete_From_Leaf_RemovesItem()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 5 );
            tree.Add( 15 );

            tree.Remove( 10 );

            CheckTree<int>( tree.RootNode, 0, new[] { 5, 15 }, false, false, null, 5 );
        }

        [TestMethod]
        public void Delete_From_HalfLeaf_Merges()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 5 );

            tree.Remove( 12 );

            CheckTree<int>( tree.RootNode, 0, new[] { 5, 10, 11 }, false, false, null, 5 );
        }

        [TestMethod]
        public void Delete_From_HalfLeaf_RemovesItem()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 4 );
            tree.Add( 5 );

            tree.Remove( 12 );

            CheckTree<int>( tree.RootNode, 1, new[] { 10, 11 }, true, false, null, 10 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 4, 5 }, false, false, tree.RootNode, 10 );
        }

        [TestMethod]
        public void Delete_From_InternalNode_BorrowTheGreatestLowerBound()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 4 );
            tree.Add( 5 );
            tree.Add( 15 );

            tree.Remove( 11 );

            CheckTree<int>( tree.RootNode, 1, new[] { 5, 10, 12 }, true, true, null, 5 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 4 }, false, false, tree.RootNode, 5 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 15 }, false, false, tree.RootNode, 5 );
        }


        [TestMethod]
        public void Delete_From_InternalNode_BorrowTheGreatestLowerBoundCausesHalfLeafToBeEmpty()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );

            tree.Add( 4 );
            tree.Add( 5 );
            tree.Add( 6 );

            tree.Add( 16 );
            tree.Add( 17 );
            tree.Add( 18 );

            tree.Add( 1 );
            tree.Add( 2 );
            tree.Add( 3 );

            tree.Remove( 4 );
            tree.Remove( 5 );
            tree.Remove( 10 );

            CheckTree<int>( tree.RootNode, 1, new[] { 6, 11, 12 }, true, true, null, 6 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 1, 2, 3 }, false, false, tree.RootNode, 6 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 16, 17, 18 }, false, false, tree.RootNode, 6 );
        }

        [TestMethod]
        public void Delete_From_InternalNode_BorrowTheGreatestLowerBound_WhenLeafBecomesEmpty()
        {
            var tree = new TTree<int>( 3, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 4 );
            tree.Add( 15 );

            tree.Remove( 11 );

            CheckTree<int>( tree.RootNode, 1, new[] { 4, 10, 12 }, false, true, null, 4 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 15 }, false, false, tree.RootNode, 4 );
        }

        [TestMethod]
        public void Delete_From_RightLeaf_BecomesEmpty()
        {
            var tree = new TTree<int>( 2, 3 );

            tree.Add( 1 );
            tree.Add( 2 );
            tree.Add( 3 );
            tree.Add( 4 );
            tree.Add( 5 );
            tree.Add( 6 );
            tree.Add( 7 );

            tree.Remove( 7 );

            CheckTree<int>( tree.RootNode, 1, new[] { 4, 5, 6 }, true, false, null, 4 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 1, 2, 3 }, false, false, tree.RootNode, 4 );
        }

        [TestMethod]
        public void Delete_ShouldRebalance()
        {
            var tree = new TTree<int>( 2, 3 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 12 );
            tree.Add( 1 );
            tree.Add( 13 );
            tree.Add( 14 );
            tree.Add( 15 );
            tree.Add( 16 );

            tree.Remove( 1 );

            CheckTree<int>( tree.RootNode, 1, new[] { 13, 14, 15 }, true, true, null, 13 );
            CheckTree<int>( tree.RootNode.Left, 0, new[] { 10, 11, 12 }, false, false, tree.RootNode, 13 );
            CheckTree<int>( tree.RootNode.Right, 0, new[] { 16 }, false, false, tree.RootNode, 13 );
        }

        [TestMethod]
        public void IEnumerable()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 5 );
            tree.Add( 3 );
            tree.Add( 10 );
            tree.Add( 1 );
            tree.Add( 7 );
            tree.Add( 20 );
            tree.Add( 100 );

            var items = new List<int>();

            foreach ( int i in tree )
            {
                items.Add( i );
            }

            Assert.AreEqual( 1, items[0], "Invalid value at 0" );
            Assert.AreEqual( 3, items[1], "Invalid value at 1" );
            Assert.AreEqual( 5, items[2], "Invalid value at 2" );
            Assert.AreEqual( 7, items[3], "Invalid value at 3" );
            Assert.AreEqual( 10, items[4], "Invalid value at 4" );
            Assert.AreEqual( 20, items[5], "Invalid value at 5" );
            Assert.AreEqual( 100, items[6], "Invalid value at 6" );
        }

        [TestMethod]
        public void CopyTo()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 5 );
            tree.Add( 3 );
            tree.Add( 10 );
            tree.Add( 1 );
            tree.Add( 7 );
            tree.Add( 20 );
            tree.Add( 100 );

            var items = new int[7];
            tree.CopyTo( items, 0 );

            Assert.AreEqual( 1, items[0], "Invalid value at 0" );
            Assert.AreEqual( 3, items[1], "Invalid value at 1" );
            Assert.AreEqual( 5, items[2], "Invalid value at 2" );
            Assert.AreEqual( 7, items[3], "Invalid value at 3" );
            Assert.AreEqual( 10, items[4], "Invalid value at 4" );
            Assert.AreEqual( 20, items[5], "Invalid value at 5" );
            Assert.AreEqual( 100, items[6], "Invalid value at 6" );
        }

        [TestMethod]
        public void CopyItems()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 1 );
            tree.Add( 20 );

            var items = new int[20];
            tree.CopyItems( items, 0 );

            Assert.AreEqual(  1, items[0], "Invalid value at 0" );
            Assert.AreEqual( 10, items[1], "Invalid value at 1" );
            Assert.AreEqual( 11, items[2], "Invalid value at 2" );
            Assert.AreEqual( 20, items[3], "Invalid value at 3" );

        }

        [TestMethod]
        public void Clear()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 1 );
            tree.Add( 20 );
            tree.Clear();

            Assert.AreEqual( 0, tree.Count, "Invalid count" );
            Assert.AreEqual( 0, tree.Count, "Invalid item count" );
            Assert.AreEqual( 0, tree.Height, "Invalid height" );
        }

        [TestMethod]
        public void Contains()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 10 );
            tree.Add( 11 );
            tree.Add( 1 );
            tree.Add( 20 );

            Assert.IsTrue( tree.Contains( 1 ), "Should contain 1" );
            Assert.IsTrue( tree.Contains( 10 ), "Should contain 10" );
            Assert.IsTrue( tree.Contains( 11 ), "Should contain 11" );
            Assert.IsTrue( tree.Contains( 20 ), "Should contain 20" );

            Assert.IsFalse( tree.Contains( 5 ), "Should not contain 5" );
            Assert.IsFalse( tree.Contains( 3 ), "Should not contain 3" );
            Assert.IsFalse( tree.Contains( 7 ), "Should not contain 7" );
        }

        [TestMethod]
        public void Count()
        {
            var tree = new TTree<int>( 2, 2 );

            tree.Add( 5 );
            Assert.AreEqual( 1, tree.Count, "Invalid count" );

            tree.Add( 3 );
            Assert.AreEqual( 2, tree.Count, "Invalid count" );

            tree.Add( 10 );
            Assert.AreEqual( 3, tree.Count, "Invalid count" );

            tree.Add( 1 );
            Assert.AreEqual( 4, tree.Count, "Invalid count" );

            tree.Add( 7 );
            Assert.AreEqual( 5, tree.Count, "Invalid count" );

            tree.Add( 20 );
            Assert.AreEqual( 6, tree.Count, "Invalid count" );

            tree.Add( 100 );
            Assert.AreEqual( 7, tree.Count, "Invalid count" );
        }

        private void CheckTree<T>(
            SimpleTTreeNode<T> tree,
            int height,
            T[] data,
            bool hasLeft,
            bool hasRight,
            SimpleTTreeNode<T> parent,
            T root1stItem )
            where T : IComparable
        {
            Assert.AreEqual( data.Length, tree.ItemCount, "Invalid count" );

            T[] values = new T[tree.MaxItems];
            tree.CopyItems( values, 0 );

            for ( int i = 0; i < data.Length; ++i )
            {
                Assert.AreEqual( data[i], values[i], "Invalid value item " + i );
            }

            Assert.AreEqual( hasLeft, (tree.Left != null), "Invalid has left value" );
            Assert.AreEqual( hasRight, (tree.Right != null), "Invalid has right value" );
            Assert.AreSame( parent, tree.Parent, "Incorrect parent node" );
            Assert.AreEqual( height, tree.Height, "Invalid height" );

            var nodeData = new T[tree.MaxItems];
            tree.Tree.RootNode.CopyItems( nodeData, 0 );

            Assert.AreEqual( root1stItem, nodeData[0], "Invalid root item 0, incorrect root node" );
        }

        public TestContext TestContext { get; set; }
    }
}