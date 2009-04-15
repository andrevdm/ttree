using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace TTree
{
	/// <summary>
	/// T-Tree implementation.
	/// Google "A Study of Index Structures for Main Memory Database Management Systems"
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TTreeNode<T> : ITreeNode<T> //visitor, IEnumerable etc
		where T : IComparable
	{
		protected readonly T[] m_data;
		protected readonly int m_minimum;
		protected int m_height = 0;
		protected readonly TTreeRoot<T> m_root;

		public TTreeNode( int minimum, int maximum, TTreeRoot<T> root )
		{
			#region param checks
			if( minimum < 1 )
				throw new ArgumentOutOfRangeException( "order", "Expecting an order of at least 1." );

			if( maximum < minimum )
				throw new ArgumentOutOfRangeException( "maximum", "Maximum value must be greater than the minimum. " );

			if( root == null )
				throw new ArgumentNullException( "root" );
			#endregion

			Count = 0;
			m_data = new T[ maximum ];
			m_minimum = minimum;
			m_root = root;
		}

		/// <summary>
		/// Inserts the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was added or false if it already existed and was not </returns>
		public bool Insert( T item )
		{
			bool isBoundingNode;
			int comparedToFirst = 0;

			//Is this the bounding node for the new item? If the node is empty it is considered to be the bounding node.
			if( Count == 0 )
			{
				isBoundingNode = true;
			}
			else
			{
				//Compare the item to be inserted to the first item in the data
				comparedToFirst = item.CompareTo( m_data[ 0 ] );
				isBoundingNode = ((comparedToFirst >= 0) && (item.CompareTo( m_data[ Count - 1 ] ) <= 0));
			}

			if( isBoundingNode )
			{
				//Is there space in this node?
				if( Count < m_data.Length )
				{
					//This is the bounding node, add the new item
					return InsertInCurrentNode( item, false );
				}
				else
				{
					//Copy the old minimum. This current item will be inserted into this node
					T oldMinimum = m_data[ 0 ];

					if( !InsertInCurrentNode( item, true ) )
						return false;

					//Add the old minimum
					if( Left == null )
					{
						//There is no left child, so create it
						Left = CreateChild( oldMinimum );
						UpdateHeight( false );
						Rebalance( true, false );
						return true;
					}
					else
					{
						//Add the old minimum to the left child
						return Left.Insert( oldMinimum );
					}
				}
			}
			else
			{
				//If the item is less than the minimum and there is a left node, follow it
				if( (Left != null) && (comparedToFirst < 0) )
				{
					return Left.Insert( item );
				}

				//If the item is less than the maximum and there is a right node, follow it
				if( (Right != null) && (comparedToFirst > 0) )
				{
					return Right.Insert( item );
				}

				//If we are here then, there is no bounding node for this value.
				//Is there place in this node
				if( Count < m_data.Length )
				{
					//There is place in this node so add the new value. However since this value
					// must be the new minimum or maximum (otherwise it would have found a bounding
					// node) dont call InsertInCurrentNode which would do a binary search to find
					// an insert location. Rather check for min/max and add it here.
					if( comparedToFirst > 0 )
					{
						//The item is greater than the minimum, therfore it must be the new maximum.
						// Add it to the end of the array
						m_data[ Count ] = item;
					}
					else
					{
						//The item is the new miminum. Shift the array up and insert it.
						Array.Copy( m_data, 0, m_data, 1, Count );
						m_data[ 0 ] = item;
					}

					Count++;
				}
				else
				{
					TTreeNode<T> newChild = CreateChild( item );

					//Add it as the the left or the right child
					if( comparedToFirst < 0 )
					{
						Left = newChild;
					}
					else
					{
						Right = newChild;
					}

					UpdateHeight( true );
					Rebalance( true, false );
					return true;
				}
			}

			return true;
		}

		/// <summary>
		/// Inserts an item into the current node. This method can be called in two instances
		/// 1 - When this node is not full
		/// 2 - When this node is full and the first item has been copied so that it can be
		/// used as the new insert value. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="fullShiftToLeft">if set to <c>true</c> [full shift to left].</param>
		/// <returns></returns>
		private bool InsertInCurrentNode( T item, bool fullShiftToLeft )
		{
			Debug.Assert( fullShiftToLeft ? (Count == m_data.Length) : (Count < m_data.Length), "If doing a shift left, the node should have been full, otherwise it should not be called if the node is full" );

			//TODO profile and see if there is any advantage to checking the min and max values here and setting closest=0 or closest=Count. Remember to check that the item was not already added

			//Find the position with the closest value to the item being added.
			int closest = Array.BinarySearch<T>( m_data, 0, Count, item );

			//If closest is positive then the item already exists at this level, so
			// no need to add it again
			if( closest >= 0 )
				return false;

			//Negate the result, which gives info about where the closest match is
			closest = ~closest;

			//If closest is greater than the count then there is no item in the array than the item being added,
			// so add it to the end of the array. Otherwise the negated value is the position to add the item to
			if( closest > Count )
				closest = Count;

			if( !fullShiftToLeft )
			{
				//Shift the items up by one place to make space for the new item. This also works when adding
				// an item to the end of the array.
				Array.Copy( m_data, closest, m_data, closest + 1, Count - closest );

				m_data[ closest ] = item;

				//An item has been added.
				Count++;
			}
			else
			{
				//Shift the items before the insertion point to the left.
				Array.Copy( m_data, 1, m_data, 0, closest - 1 );
				m_data[ closest - 1 ] = item;
			}

			return true;
		}

		private void Rebalance( bool stopAfterFirstRotate, bool stopAfterEvenBalanceFound )
		{
			if( BalanceFactor > 1 )
			{
				if( Left.BalanceFactor > 0 )
				{
					RotateLL();

					if( stopAfterFirstRotate )
						return;
				}
				else
				{
					RotateLR();

					if( stopAfterFirstRotate )
						return;
				}
			}
			else if( BalanceFactor < -1 )
			{
				if( Right.BalanceFactor < 0 )
				{
					RotateRR();

					if( stopAfterFirstRotate )
						return;
				}
				else
				{
					RotateRL();

					if( stopAfterFirstRotate )
						return;
				}
			}
			else
			{
				if( stopAfterEvenBalanceFound )
					return;
			}

			if( Parent != null )
			{
				Parent.Rebalance( true, false );
			}
		}

		private void RotateRL()
		{
			//Check if a T-Tree sliding rotate must be done first.
			if( IsHalfLeaf && Right.IsHalfLeaf && Right.Left.IsLeaf )
			{
				var nodeB = Right;
				var nodeC = Right.Left;

				int maxLen = m_data.Length;
				int delta = maxLen - nodeC.Count;
				Array.Copy( nodeB.m_data, 0, nodeC.m_data, nodeC.Count, delta );
				Array.Copy( nodeB.m_data, delta, nodeB.m_data, 0, nodeC.Count );

				nodeB.Count = nodeC.Count;
				nodeC.Count = maxLen;
			}

			Right.RotateLL();
			RotateRR();
		}

		private void RotateLR()
		{
			//Check if a T-Tree sliding rotate must be done first.
			if( IsHalfLeaf && Left.IsHalfLeaf && Left.Right.IsLeaf )
			{
				var nodeB = Left;
				var nodeC = Left.Right;

				int maxLen = m_data.Length;
				int delta = maxLen - nodeC.Count;
				Array.Copy( nodeC.m_data, 0, nodeC.m_data, delta, nodeC.Count );
				Array.Copy( nodeB.m_data, nodeC.Count, nodeC.m_data, 0, delta );

				nodeB.Count = nodeC.Count;
				nodeC.Count = maxLen;
			}

			Left.RotateRR();
			RotateLL();
		}

		private void RotateRR()
		{
			TTreeNode<T> b = Right;
			TTreeNode<T> c = b.Left;

			if( Parent != null )
			{
				if( Parent.Left == this )
					Parent.Left = b;
				else
					Parent.Right = b;
			}

			b.Parent = Parent;
			b.Left = this;
			Parent = b;
			Right = c;

			if( c != null )
			{
				c.Parent = this;
			}

			if( b.Parent == null )
			{
				Root.RootNode = b;
			}

			UpdateHeight( true );
		}

		private void RotateLL()
		{
			TTreeNode<T> left = Left;
			TTreeNode<T> leftsRight = left.Right;

			if( Parent != null )
			{
				if( Parent.Left == this )
					Parent.Left = left;
				else
					Parent.Right = left;
			}

			left.Parent = Parent;
			left.Right = this;
			Parent = left;
			Left = leftsRight;

			if( leftsRight != null )
			{
				leftsRight.Parent = this;
			}

			if( left.Parent == null )
			{
				Root.RootNode = left;
			}

			UpdateHeight( true );
		}

		public bool Delete( T item )
		{
			SearchResult<T> searchResult = SearchFor( item );

			if( searchResult == null )
			{
				return false;
			}

			//If the delete will not cause an underflow then, delete the value and stop
			if( searchResult.Node.Count > searchResult.Node.m_minimum )
			{
				DeleteFoundValue( searchResult );
				return true;
			}

			if( searchResult.Node.IsInternal )
			{
				//Shift the array to the right "delete"
				// This is faster than calling DeleteFoundValue() because now there is no need to shift
				// the array a second time to make space for the greatest lower bound
				if( searchResult.Index > 0 )
				{
					Array.Copy( searchResult.Node.m_data, searchResult.Index - 1, searchResult.Node.m_data, searchResult.Index, searchResult.Node.m_data.Length - 1 - searchResult.Index );
				}

				//Insert the greatest lower bound
				m_data[ 0 ] = searchResult.Node.Left.CutGreatestLowerBound();
			}
			else  //This is a leaf or half-leaf so just delete the value (leaves and half-leaves are permitted to underflow)
			{
				DeleteFoundValue( searchResult );

				//If this is a half leaf and it can be merged with a leaf, then combine
				if( searchResult.Node.IsHalfLeaf )
				{
					var child = (searchResult.Node.Left == null) ? searchResult.Node.Right : searchResult.Node.Left;

					//If all the child items can fit into this node
					if( searchResult.Node.Count + child.Count <= MaxItems )
					{
						//TODO consider not looping - the child is sorted, insert all the items at once, either at the begining or at the end (left/right)
						for( int i = 0; i < child.Count; ++i )
						{
							searchResult.Node.InsertInCurrentNode( child.m_data[ i ], false );
						}

						//Remove the child
						searchResult.Node.Left = searchResult.Node.Right = null;
						searchResult.Node.m_height = 0;
					}
				}
				else //Is leaf
				{
					if( searchResult.Node.Count != 0 )
					{
						//This is a non-empty leaf. Nothing more to do
						return true;
					}
					else
					{
						//The node is empty. So, unles this is the root, remove the node from its parent.
						if( searchResult.Node.Parent != null )
						{
							if( searchResult.Node.Parent.Left == searchResult.Node )
							{
								searchResult.Node.Parent.Left = null;
							}
							else
							{
								searchResult.Node.Parent.Right = null;
							}
						}
					}
				}
			}

			searchResult.Node.Rebalance( false, true );
			return true;
		}

		private T CutGreatestLowerBound()
		{
			if( Right == null )
			{
				Count--;

				if( Count == 0 )
				{
					//If there is a left node then the parent should now point to it.
					if( Parent.Right == this )
						Parent.Right = Left;
					else
						Parent.Left = Left;

					if( Left != null )
						Left.Parent = Parent;

					UpdateHeight( true );
				}

				return m_data[ Count ];
			}
			else
			{
				return Right.CutGreatestLowerBound();
			}
		}

		private void DeleteFoundValue( SearchResult<T> searchResult )
		{
			Array.Copy( searchResult.Node.m_data, searchResult.Index + 1, searchResult.Node.m_data, searchResult.Index, searchResult.Node.m_data.Length - 1 - searchResult.Index );
			searchResult.Node.Count--;
		}

		/// <summary>
		/// Search for an item using a custom comparison function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		public T Search<TSearch>( TSearch item, Func<TSearch, T, int> comparer )
		{
			var result = SearchFor( item, comparer );
			return (result == null) ? default( T ) : result.Value;
		}

		public SearchResult<T> SearchFor<TSearch>( TSearch item, Func<TSearch, T, int> comparer )
		{
			if( Count == 0 )
				return null;

			int compare = comparer( item, m_data[ 0 ] );

			if( compare == 0 )
				return new SearchResult<T>( m_data[ 0 ], this, 0 );

			if( compare < 0 )
			{
				if( Left != null )
					return Left.SearchFor( item, comparer );
				else
					return null;
			}

			compare = comparer( item, m_data[ Count - 1 ] );

			if( compare == 0 )
				return new SearchResult<T>( m_data[ Count - 1 ], this, Count - 1 );

			if( compare > 0 )
			{
				if( Right != null )
					return Right.SearchFor( item, comparer );
				else
					return null;
			}

			int closest = BinarySearch<TSearch>( m_data, 0, Count, item, comparer );

			if( closest >= 0 )
				return new SearchResult<T>( m_data[ closest ], this, closest );

			return null;
		}

		/// <summary>
		/// Searches for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public T Search( T item )
		{
			var result = SearchFor( item );
			return (result == null) ? default( T ) : result.Value;
		}

		public SearchResult<T> SearchFor( T item )
		{
			//This code is not shared with the other Search() method to keep things as fast as possible

			if( Count == 0 )
				return null;

			int compare = item.CompareTo( m_data[ 0 ] );

			if( compare == 0 )
				return new SearchResult<T>( m_data[ 0 ], this, 0 );

			if( compare < 0 )
			{
				if( Left != null )
					return Left.SearchFor( item );
				else
					return null;
			}

			compare = item.CompareTo( m_data[ Count - 1 ] );

			if( compare == 0 )
				return new SearchResult<T>( m_data[ Count - 1 ], this, Count - 1 );

			if( compare > 0 )
			{
				if( Right != null )
					return Right.SearchFor( item );
				else
					return null;
			}

			int closest = Array.BinarySearch<T>( m_data, 0, Count, item );

			if( closest >= 0 )
				return new SearchResult<T>( m_data[ closest ], this, closest );

			return null;
		}

		public void CopyItems( T[] destinationArray, int index )
		{
			m_data.CopyTo( destinationArray, index );
		}

		public string ToDot()
		{
			return ToDot( i => i.ToString() );
		}

		public string ToDot( Func<T, string> toString )
		{
			var dot = new StringBuilder();

			dot.AppendLine( "digraph structs {{" );
			dot.AppendLine( "	node [shape=record, fontsize=9, fontname=Ariel];" );
			ToDot( dot, toString );
			dot.AppendLine( "}}" );

			return dot.ToString();
		}

		private void ToDot( StringBuilder dot, Func<T, string> toString )
		{
			dot.AppendFormat( "	struct{0} [shape=record, label=\"{{ {{ ", GetHashCode().ToString( "X" ) );

			for( int i = 0; i < m_data.Length; ++i )
			{
				if( i > 0 )
				{
					dot.Append( "| " );
				}

				if( i < Count )
				{
					dot.AppendFormat( "{0}", toString( m_data[ i ] ) );
				}
			}

			dot.AppendFormat( " }} | {{ <left> . | <m> h={0}, bf={1} | <right> . }} }}\"];", Height, BalanceFactor ).AppendLine();

			if( Left != null )
				Left.ToDot( dot, toString );

			if( Right != null )
				Right.ToDot( dot, toString );

			if( Left != null )
				dot.AppendFormat( "	\"struct{0}\":left -> struct{1};", GetHashCode().ToString( "X" ), Left.GetHashCode().ToString( "X" ) ).AppendLine();

			if( Right != null )
				dot.AppendFormat( "	\"struct{0}\":right -> struct{1};", GetHashCode().ToString( "X" ), Right.GetHashCode().ToString( "X" ) ).AppendLine();

			if( Parent != null )
				dot.AppendFormat( "	struct{0} -> \"struct{1}\":m [style=dotted, color=saddlebrown, arrowsize=0.4];", GetHashCode().ToString( "X" ), Parent.GetHashCode().ToString( "X" ) ).AppendLine();
		}

		/// <summary>
		/// Creates a new child node.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		private TTreeNode<T> CreateChild( T item )
		{
			//Create a new child node
			TTreeNode<T> newChild = new TTreeNode<T>( m_minimum, m_data.Length, Root );
			newChild.m_data[ 0 ] = item;
			newChild.Count = 1;
			newChild.Parent = this;

			return newChild;
		}

		private void UpdateHeight( bool updateAllUpwards )
		{
			//int oldHeight = m_height;

			if( Count == 0 )
			{
				m_height = -1;
			}
			else
			{
				int lheight = (Left != null) ? (Left.Height) : -1;
				int rheight = (Right != null) ? (Right.Height) : -1;

				m_height = 1 + Math.Max( lheight, rheight );
			}

			if( updateAllUpwards && (Parent != null) )
			{
				Parent.UpdateHeight( updateAllUpwards );
			}
		}

		/// <summary>
		/// Binary search implementation using a custom compare function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		/// <param name="length">The length.</param>
		/// <param name="value">The value.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		private int BinarySearch<TSearch>( T[] array, int index, int length, TSearch value, Func<TSearch, T, int> comparer )
		{
			int num1 = index;
			int num2 = (index + length) - 1;

			while( num1 <= num2 )
			{
				int num3 = num1 + ((num2 - num1) >> 1);
				int num4 = -comparer( value, array[ num3 ] );

				if( num4 == 0 )
					return num3;

				if( num4 < 0 )
					num1 = num3 + 1;
				else
					num2 = num3 - 1;
			}

			return ~num1;
		}

		public int BalanceFactor
		{
			get
			{
				if( Count == 0 )
				{
					return 0;
				}
				else
				{
					int lheight = (Left != null) ? (Left.Height) : -1;
					int rheight = (Right != null) ? (Right.Height) : -1;

					return lheight - rheight;
				}
			}
		}

		public int Height { get { return m_height; } }
		public int Count { get; protected set; }
		public int MaxItems { get { return m_data.Length; } }
		public TTreeNode<T> Left { get; set; }
		public TTreeNode<T> Right { get; set; }
		public TTreeNode<T> Parent { get; set; }
		public TTreeRoot<T> Root { get { return m_root; } }
		public bool IsLeaf { get { return (Left == null) && (Right == null); } }
		public bool IsHalfLeaf { get { return !IsLeaf && ((Left == null) || (Right == null)); } }
		public bool IsInternal { get { return (Left != null) && (Right != null); } } 
		public T this[ int index ] { get { return m_data[ index ]; } } //TODO change to be Values[]
	}
}