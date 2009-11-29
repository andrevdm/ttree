using System;
using System.Collections.Generic;
using System.Collections;

namespace TTree
{
    /// <summary>
    /// Default TTree. 
    /// See TTree<TItem, TNode> for more information
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class TTree<TItem> : TTree<TItem, SimpleTTreeNode<TItem>>
        where TItem : IComparable
    {
        public TTree( int minimum, int maximum )
            : base( minimum, maximum )
        {
        }

        public override SimpleTTreeNode<TItem> CreateNode( int minimum, int maximum )
        {
            return new SimpleTTreeNode<TItem>( minimum, maximum, this );
        }
    }

    /// <summary>
    /// T-Tree implementation.
    /// 
    /// “A T-tree is a balanced index tree data structure optimized for cases where both the 
    /// index and the actual data are fully kept in memory, just as a B-tree is an index structure 
    /// optimized for storage on block oriented external storage devices like hard disks. T-trees 
    /// seek to gain the performance benefits of in-memory tree structures such as AVL trees while 
    /// avoiding the large storage space overhead which is common to them.” 
    /// (from http://en.wikipedia.org/wiki/T-tree)
    ///
    /// Google for “A Study of Index Structures for Main Memory Database Management Systems” 
    /// for a comprehensive discussion of T-Trees
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TNode"></typeparam>
    public abstract class TTree<TItem, TNode> : ICollection<TItem>
        where TItem : IComparable
        where TNode : TTreeNode<TItem, TNode>
    {
        protected TTree( int minimum, int maximum )
        {
            RootNode = CreateNode( minimum, maximum );
        }

        public void CopyItems( TItem[] destinationArray, int index )
        {
            RootNode.CopyTo( destinationArray, index );
        }

        public abstract TNode CreateNode( int minimum, int maximum );

        public TItem Search( TItem item )
        {
            return RootNode.Search( item );
        }

        public TItem Search<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
        {
            return RootNode.Search( item, comparer );
        }

        public SearchResult<TItem, TNode> SearchFor( TItem item )
        {
            return RootNode.SearchFor( item );
        }

        public SearchResult<TItem, TNode> SearchFor<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
        {
            return RootNode.SearchFor( item, comparer );
        }

        public string ToDot()
        {
            return RootNode.ToDot();
        }

        public string ToDot( Func<TItem, string> toString )
        {
            return RootNode.ToDot( toString );
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add( TItem item )
        {
            RootNode.AddItem( item );
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Clear()
        {
            RootNode.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains( TItem item )
        {
            return (RootNode.SearchFor( item ) != null);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <paramref name="TItem"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo( TItem[] array, int arrayIndex )
        {
            RootNode.CopyTo( array, arrayIndex );
        }

        public bool Remove( TItem item )
        {
            return RootNode.Remove( item );
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return RootNode.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RootNode.GetEnumerator();
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count { get { return RootNode.Count; } }

        public int Height { get { return RootNode.Height; } }

        public TNode RootNode { get; internal set; }

        /*
		#region ITreeNode delegating to actual root node
		public bool AddItem( TItem item )
		{
			return RootNode.AddItem( item );
		}

		public TItem Search<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
		{
			return RootNode.Search<TSearch>( item, comparer );
		}

		public TItem Search( TItem item )
		{
			return RootNode.Search( item );
		}

		public SearchResult<TItem> SearchFor( TItem item )
		{
			return RootNode.SearchFor( item );
		}

		public SearchResult<TItem> SearchFor<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
		{
			return RootNode.SearchFor( item, comparer );
		}

		public void CopyItems( TItem[] destinationArray, int index )
		{
			RootNode.CopyItems( destinationArray, index );
		}

		public bool Remove( TItem item )
		{
			return RootNode.Remove( item );
		}

		public void Add( TItem item )
		{
			RootNode.Add( item );
		}

		public void Clear()
		{
			RootNode.Clear();
		}

		public bool Contains( TItem item )
		{
			return RootNode.Contains( item );
		}

		public void CopyTo( TItem[] array, int arrayIndex )
		{
			RootNode.CopyTo( array, arrayIndex );
		}

		public string ToDot( Func<TItem, string> toString )
		{
			return RootNode.ToDot( toString );
		}

		public string ToDot()
		{
			return RootNode.ToDot();
		}

		public TTreeNode<TItem> Left
		{
			get { return RootNode.Left; }
			internal set { RootNode.Left = value; }
		}

		public TTreeNode<TItem> Right
		{
			get { return RootNode.Right; }
			internal set { RootNode.Right = value; }
		}

		public TTreeNode<TItem> Parent
		{
			get { return RootNode.Parent; }
			internal set { RootNode.Parent = value; }
		}

		public int Count { get { return RootNode.Count; } }
		public int ItemCount { get { return RootNode.ItemCount; } }
		public int Height { get { return RootNode.ItemCount; } }
		public bool IsLeaf { get { return RootNode.IsLeaf; } }
		public bool IsHalfLeaf { get { return RootNode.IsHalfLeaf; } }
		public bool IsInternal { get { return RootNode.IsInternal; } }
		public int MaxItems { get { return RootNode.MaxItems; } }
		public TTree<TItem> Root { get { return RootNode.Root; } }
		public int BalanceFactor { get { return RootNode.BalanceFactor; } }
		public bool IsReadOnly { get { return RootNode.IsReadOnly; } }
		#endregion

		#region IEnumerable delegating to root node
		public IEnumerator<TItem> GetEnumerator()
		{
			return RootNode.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return RootNode.GetEnumerator();
		}
		#endregion
        */
    }
}
