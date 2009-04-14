using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTree
{
	public class TTreeRoot<T> : ITreeNode<T>
		where T : IComparable
	{
		public TTreeRoot( int minimum, int maximum )
		{
			RootNode = new TTreeNode<T>( minimum, maximum, this );
		}

		public TTreeNode<T> RootNode { get; protected internal set; }

		#region ITreeNode delegating to actual root node
		public bool Insert( T item )
		{
			return RootNode.Insert( item );
		}

		public T Search<TSearch>( TSearch item, Func<TSearch, T, int> comparer )
		{
			return RootNode.Search<TSearch>( item, comparer );
		}

		public T Search( T item )
		{
			return RootNode.Search( item );
		}

		public SearchResult<T> SearchFor( T item )
		{
			return RootNode.SearchFor( item );
		}

		public SearchResult<T> SearchFor<TSearch>( TSearch item, Func<TSearch, T, int> comparer )
		{
			return RootNode.SearchFor( item, comparer );
		}

		public void CopyItems( T[] destinationArray, int index )
		{
			RootNode.CopyItems( destinationArray, index );
		}

		public void Delete( T item )
		{
			RootNode.Delete( item );
		}

		public string ToDot( Func<T, string> toString )
		{
			return RootNode.ToDot( toString );
		}

		public string ToDot()
		{
			return RootNode.ToDot();
		}

		public int Count
		{
			get { return RootNode.Count; }
		}

		public int Height
		{
			get { return RootNode.Count; }
		}

		public bool IsLeaf
		{
			get { return RootNode.IsLeaf; }
		}

		public bool IsHalfLeaf
		{
			get { return RootNode.IsHalfLeaf; }
		}

		public int MaxItems
		{
			get { return RootNode.MaxItems; }
		}

		public TTreeRoot<T> Root
		{
			get { return RootNode.Root; }
		}

		public TTreeNode<T> Left
		{
			get{return RootNode.Left; }
			set { RootNode.Left = value; }
		}

		public TTreeNode<T> Right
		{
			get { return RootNode.Right; }
			set { RootNode.Right = value; }
		}

		public TTreeNode<T> Parent
		{
			get { return RootNode.Parent; }
			set { RootNode.Parent = value; }
		}

		public T this[ int index ]
		{
			get { return RootNode[ index ]; }
		}

		public int BalanceFactor
		{
			get { return RootNode.BalanceFactor; }
		}
		#endregion
	}
}
