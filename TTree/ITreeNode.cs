using System;

namespace TTree
{
	public interface ITreeNode<T> 
		where T : IComparable
	{

		/// <summary>
		/// Inserts the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was added or false if it already existed and was not </returns>
		bool Insert( T item );

		/// <summary>
		/// Search for an item using a custom comparison function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		T Search<TSearch>( TSearch item, Func<TSearch, T, int> comparer );

		SearchResult<T> SearchFor<TSearch>( TSearch item, Func<TSearch, T, int> comparer );
		SearchResult<T> SearchFor( T item );

		/// <summary>
		/// Searches for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		T Search( T item );

		void CopyItems( T[] destinationArray, int index );
		bool Delete( T item );
		string ToDot( Func<T, string> toString );
		string ToDot();

		int Count { get; }
		int Height { get; }
		bool IsLeaf { get; }
		bool IsHalfLeaf { get; }
		bool IsInternal { get; }
		int MaxItems { get; }
		TTreeRoot<T> Root { get; }
		TTreeNode<T> Left { get; set; }
		TTreeNode<T> Right { get; set; }
		TTreeNode<T> Parent { get; set; }
		T this[ int index ] { get; }
		int BalanceFactor { get; }
	}
}
