using System;
using System.Collections.Generic;

namespace TTree
{
	/// <summary>
	/// Implemented by TTreeNodes and the TTreeRoot
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITTreeNode<T> : ICollection<T> 
		where T : IComparable
	{
		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was added or false if it already existed and was not </returns>
		bool AddItem( T item );

		/// <summary>
		/// Search for an item using a custom comparison function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		T Search<TSearch>( TSearch item, Func<TSearch, T, int> comparer );

		/// <summary>
		/// Search for an item using a custom comparison function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		SearchResult<T> SearchFor<TSearch>( TSearch item, Func<TSearch, T, int> comparer );
		
		/// <summary>
		/// Searches for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		SearchResult<T> SearchFor( T item );

		/// <summary>
		/// Searches for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		T Search( T item );

		/// <summary>
		/// Copies the items from the current node only
		/// </summary>
		/// <param name="destinationArray">The destination array.</param>
		/// <param name="index">The index.</param>
		void CopyItems( T[] destinationArray, int index );

		/// <summary>
		/// Gets number of items in this node
		/// </summary>
		/// <value>The item count.</value>
		int ItemCount { get; }

		string ToDot( Func<T, string> toString );
		string ToDot();

		int Height { get; }
		bool IsLeaf { get; }
		bool IsHalfLeaf { get; }
		bool IsInternal { get; }
		int MaxItems { get; }
		TTreeRoot<T> Root { get; }
		TTreeNode<T> Left { get; }
		TTreeNode<T> Right { get; }
		TTreeNode<T> Parent { get;  }
		int BalanceFactor { get; }
	}
}
