using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace TTree
{
    /// <summary>
    /// Default TTreeNode used with the default TTree.
    /// See TTreeNode<TItem, TNode> for more information
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class SimpleTTreeNode<TItem> : TTreeNode<TItem, SimpleTTreeNode<TItem>>
        where TItem : IComparable
    {
        public SimpleTTreeNode( int minimum, int maximum, TTree<TItem,SimpleTTreeNode<TItem>> root )
            : base( minimum, maximum, root )
        {
        }
    }

    /// <summary>
    /// Node of a TTree.
    /// When working with a TTree you should always use the TTree class and not the
    /// nodes directly
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TNode"></typeparam>
    public abstract class TTreeNode<TItem, TNode> : IEnumerable<TItem>
		where TItem : IComparable
        where TNode : TTreeNode<TItem, TNode>
	{
		private readonly TItem[] m_data;
		private readonly int m_minimum;
		private int m_height = 0;
		private readonly TTree<TItem,TNode> m_tree;

		protected TTreeNode( int minimum, int maximum, TTree<TItem,TNode> tree )
		{
			#region param checks
			if( minimum < 1 )
				throw new ArgumentOutOfRangeException( "minimum", "Expecting a minimum of at least 1." );

			if( maximum < minimum )
				throw new ArgumentOutOfRangeException( "maximum", "Maximum value must be greater than the minimum. " );

			if( tree == null )
				throw new ArgumentNullException( "root" );
			#endregion

			ItemCount = 0;
			m_data = new TItem[ maximum ];
			m_minimum = minimum;
			m_tree = tree;
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was added or false if it already existed and was not </returns>
		public bool AddItem( TItem item )
		{
			bool isBoundingNode;
			int comparedToFirst = 0;

			//Is this the bounding node for the new item? If the node is empty it is considered to be the bounding node.
			if( ItemCount == 0 )
			{
				isBoundingNode = true;
			}
			else
			{
				//Compare the item to be inserted to the first item in the data
				comparedToFirst = item.CompareTo( m_data[ 0 ] );
				isBoundingNode = ((comparedToFirst >= 0) && (item.CompareTo( m_data[ ItemCount - 1 ] ) <= 0));
			}

			if( isBoundingNode )
			{
				//Is there space in this node?
				if( ItemCount < m_data.Length )
				{
					//This is the bounding node, add the new item
                    return InsertInCurrentNode( item, ShiftType.NoShift );
				}
				else
				{
					//Copy the old minimum. This current item will be inserted into this node
					TItem oldMinimum = m_data[ 0 ];

                    if ( !InsertInCurrentNode( item, ShiftType.FullShiftToLeft ) )
						return false;

					//Add the old minimum
					if( Left == null )
					{
						//There is no left child, so create it
						Left = CreateChild( oldMinimum );
                        UpdateHeight(HeightUpdateType.CurrentLevelOnly);
                        Rebalance( BalanceType.StopAfterFirstRotate );
						return true;
					}
					else
					{
						//Add the old minimum to the left child
						return Left.AddItem( oldMinimum );
					}
				}
			}
			else
			{
				//If the item is less than the minimum and there is a left node, follow it
				if( (Left != null) && (comparedToFirst < 0) )
				{
					return Left.AddItem( item );
				}

				//If the item is less than the maximum and there is a right node, follow it
				if( (Right != null) && (comparedToFirst > 0) )
				{
					return Right.AddItem( item );
				}

				//If we are here then, there is no bounding node for this value.
				//Is there place in this node
				if( ItemCount < m_data.Length )
				{
					//There is place in this node so add the new value. However since this value
					// must be the new minimum or maximum (otherwise it would have found a bounding
					// node) dont call InsertInCurrentNode which would do a binary search to find
					// an insert location. Rather check for min/max and add it here.
					if( comparedToFirst > 0 )
					{
						//The item is greater than the minimum, therfore it must be the new maximum.
						// Add it to the end of the array
						m_data[ ItemCount ] = item;
					}
					else
					{
						//The item is the new miminum. Shift the array up and insert it.
						Array.Copy( m_data, 0, m_data, 1, ItemCount );
						m_data[ 0 ] = item;
					}

					ItemCount++;
				}
				else
				{
					TTreeNode<TItem,TNode> newChild = CreateChild( item );

					//Add it as the the left or the right child
					if( comparedToFirst < 0 )
					{
                        Left = (TNode)newChild;
					}
					else
					{
                        Right = (TNode)newChild;
					}

					UpdateHeight( HeightUpdateType.UpdateAllUpwards );
                    Rebalance( BalanceType.StopAfterFirstRotate );
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
		/// <param name="shiftType">if set to <c>true</c> [full shift to left].</param>
		/// <returns></returns>
        private bool InsertInCurrentNode( TItem item, ShiftType shiftType )
		{
            bool fullShiftToLeft = (shiftType == ShiftType.FullShiftToLeft);
            Debug.Assert( fullShiftToLeft ? (ItemCount == m_data.Length) : (ItemCount < m_data.Length), "If doing a shift left, the node should have been full, otherwise it should not be called if the node is full" );

			//TODO profile and see if there is any advantage to checking the min and max values here and setting closest=0 or closest=Count. Remember to check that the item was not already added

			//Find the position with the closest value to the item being added.
			int closest = Array.BinarySearch<TItem>( m_data, 0, ItemCount, item );

			//If closest is positive then the item already exists at this level, so
			// no need to add it again
			if( closest >= 0 )
				return false;

			//Negate the result, which gives info about where the closest match is
			closest = ~closest;

			//If closest is greater than the count then there is no item in the array than the item being added,
			// so add it to the end of the array. Otherwise the negated value is the position to add the item to
			if( closest > ItemCount )
				closest = ItemCount;

            if ( !fullShiftToLeft )
			{
				//Shift the items up by one place to make space for the new item. This also works when adding
				// an item to the end of the array.
				Array.Copy( m_data, closest, m_data, closest + 1, ItemCount - closest );

				m_data[ closest ] = item;

				//An item has been added.
				ItemCount++;
			}
			else
			{
				//Shift the items before the insertion point to the left.
				Array.Copy( m_data, 1, m_data, 0, closest - 1 );
				m_data[ closest - 1 ] = item;
			}

			return true;
		}

        //private void Rebalance( bool stopAfterFirstRotate, bool stopAfterEvenBalanceFound )
        private void Rebalance( BalanceType balanceType )
		{
            bool stopAfterFirstRotate = ((balanceType & BalanceType.StopAfterFirstRotate) != 0);
            bool stopAfterEvenBalanceFound = ((balanceType & BalanceType.StopAfterEvenBalanceFound) != 0);

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
				Parent.Rebalance( BalanceType.StopAfterFirstRotate );
			}
		}

		private void RotateRL()
		{
			//Check if a T-Tree sliding rotate must be done first.
			if( IsHalfLeaf && Right.IsHalfLeaf && Right.Left.IsLeaf )
			{
				var nodeRight = Right;
				var nodeRightsLeft = Right.Left;

				int maxLen = m_data.Length;
				int delta = maxLen - nodeRightsLeft.ItemCount;
				Array.Copy( nodeRight.m_data, 0, nodeRightsLeft.m_data, nodeRightsLeft.ItemCount, delta );
				Array.Copy( nodeRight.m_data, delta, nodeRight.m_data, 0, nodeRightsLeft.ItemCount );

				nodeRight.ItemCount = nodeRightsLeft.ItemCount;
				nodeRightsLeft.ItemCount = maxLen;
			}

			Right.RotateLL();
			RotateRR();
		}

		private void RotateLR()
		{
			//Check if a T-Tree sliding rotate must be done first.
			if( IsHalfLeaf && Left.IsHalfLeaf && Left.Right.IsLeaf )
			{
				var nodeLeft = Left;
				var nodeLeftsRight = Left.Right;

				int maxLen = m_data.Length;
				int delta = maxLen - nodeLeftsRight.ItemCount;
				Array.Copy( nodeLeftsRight.m_data, 0, nodeLeftsRight.m_data, delta, nodeLeftsRight.ItemCount );
				Array.Copy( nodeLeft.m_data, nodeLeftsRight.ItemCount, nodeLeftsRight.m_data, 0, delta );

				nodeLeft.ItemCount = nodeLeftsRight.ItemCount;
				nodeLeftsRight.ItemCount = maxLen;
			}

			Left.RotateRR();
			RotateLL();
		}

		private void RotateRR()
		{
			TTreeNode<TItem, TNode> nodeRight = Right;
			TTreeNode<TItem, TNode> nodeRightsLeft = nodeRight.Left;

			if( Parent != null )
			{
				if( Parent.Left == this )
                    Parent.Left = (TNode)nodeRight;
				else
                    Parent.Right = (TNode)nodeRight;
			}

			nodeRight.Parent = Parent;
            nodeRight.Left = (TNode)this;
            Parent = (TNode)nodeRight;
            Right = (TNode)nodeRightsLeft;

			if( nodeRightsLeft != null )
			{
                nodeRightsLeft.Parent = (TNode)this;
			}

			if( nodeRight.Parent == null )
			{
				Tree.RootNode = (TNode)nodeRight;
			}

            UpdateHeight(HeightUpdateType.UpdateAllUpwards);
		}

		private void RotateLL()
		{
			TTreeNode<TItem, TNode> left = Left;
            TTreeNode<TItem, TNode> leftsRight = left.Right;

			if( Parent != null )
			{
				if( Parent.Left == this )
                    Parent.Left = (TNode)left;
				else
                    Parent.Right = (TNode)left;
			}

			left.Parent = Parent;
            left.Right = (TNode)this;
            Parent = (TNode)left;
            Left = (TNode)leftsRight;

			if( leftsRight != null )
			{
                leftsRight.Parent = (TNode)this;
			}

			if( left.Parent == null )
			{
				Tree.RootNode = (TNode)left;
			}

            UpdateHeight(HeightUpdateType.UpdateAllUpwards);
		}

		public bool Remove( TItem item )
		{
			SearchResult<TItem,TNode> searchResult = SearchFor( item );

			if( searchResult == null )
			{
				return false;
			}

			TTreeNode<TItem,TNode> rebalanceFrom = searchResult.Node;

			//If the remove will not cause an underflow then, delete the value and stop
			if( searchResult.Node.ItemCount > searchResult.Node.m_minimum )
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
					var child = searchResult.Node.Left ?? searchResult.Node.Right;

					//If all the child items can fit into this node
					if( searchResult.Node.ItemCount + child.ItemCount <= MaxItems )
					{
						//TODO consider not looping - the child is sorted, insert all the items at once, either at the begining or at the end (left/right)
						for( int i = 0; i < child.ItemCount; ++i )
						{
                            searchResult.Node.InsertInCurrentNode( child.m_data[i], ShiftType.NoShift );
						}

						//Remove the child
						searchResult.Node.Left = searchResult.Node.Right = null;
						searchResult.Node.m_height = 0;
					}
				}
				else //Is leaf
				{
					if( searchResult.Node.ItemCount != 0 )
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

							//The current node has been deleted, so rebalance from its parent
							rebalanceFrom = rebalanceFrom.Parent;
						}
					}
				}
			}

            rebalanceFrom.Rebalance( BalanceType.StopAfterEvenBalanceFound );
			return true;
		}

		private TItem CutGreatestLowerBound()
		{
			if( Right == null )
			{
				ItemCount--;

				if( ItemCount == 0 )
				{
					//If there is a left node then the parent should now point to it.
					if( Parent.Right == this )
						Parent.Right = Left;
					else
						Parent.Left = Left;

					if( Left != null )
						Left.Parent = Parent;

                    UpdateHeight( HeightUpdateType.UpdateAllUpwards );
				}

				return m_data[ ItemCount ];
			}
			else
			{
				return Right.CutGreatestLowerBound();
			}
		}

		private void DeleteFoundValue( SearchResult<TItem, TNode> searchResult )
		{
			Array.Copy( searchResult.Node.m_data, searchResult.Index + 1, searchResult.Node.m_data, searchResult.Index, searchResult.Node.m_data.Length - 1 - searchResult.Index );
			searchResult.Node.ItemCount--;
		}

        public void Clear()
        {
            ItemCount = 0;
            Left = Right = null;
            m_height = 0;
        }

		/// <summary>
		/// Search for an item using a custom comparison function
		/// </summary>
		/// <typeparam name="TSearch">The type of the search.</typeparam>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		public TItem Search<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
		{
			var result = SearchFor( item, comparer );
			return (result == null) ? default( TItem ) : result.Value;
		}

		public SearchResult<TItem, TNode> SearchFor<TSearch>( TSearch item, Func<TSearch, TItem, int> comparer )
		{
			if( ItemCount == 0 )
				return null;

			int compare = comparer( item, m_data[ 0 ] );

			if( compare == 0 )
				return new SearchResult<TItem, TNode>( m_data[ 0 ], this, 0 );

			if( compare < 0 )
			{
				if( Left != null )
					return Left.SearchFor( item, comparer );
				else
					return null;
			}

			compare = comparer( item, m_data[ ItemCount - 1 ] );

			if( compare == 0 )
				return new SearchResult<TItem, TNode>( m_data[ ItemCount - 1 ], this, ItemCount - 1 );

			if( compare > 0 )
			{
				if( Right != null )
					return Right.SearchFor( item, comparer );
				else
					return null;
			}

			int closest = BinarySearch<TSearch>( m_data, 0, ItemCount, item, comparer );

			if( closest >= 0 )
				return new SearchResult<TItem, TNode>( m_data[ closest ], this, closest );

			return null;
		}

		/// <summary>
		/// Searches for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public TItem Search( TItem item )
		{
			var result = SearchFor( item );
			return (result == null) ? default( TItem ) : result.Value;
		}

		public SearchResult<TItem, TNode> SearchFor( TItem item )
		{
			//This code is not shared with the other Search() method to keep things as fast as possible

			if( ItemCount == 0 )
				return null;

			int compare = item.CompareTo( m_data[ 0 ] );

			if( compare == 0 )
				return new SearchResult<TItem, TNode>( m_data[ 0 ], this, 0 );

			if( compare < 0 )
			{
				if( Left != null )
					return Left.SearchFor( item );
				else
					return null;
			}

			compare = item.CompareTo( m_data[ ItemCount - 1 ] );

			if( compare == 0 )
				return new SearchResult<TItem, TNode>( m_data[ ItemCount - 1 ], this, ItemCount - 1 );

			if( compare > 0 )
			{
				if( Right != null )
					return Right.SearchFor( item );
				else
					return null;
			}

			int closest = Array.BinarySearch<TItem>( m_data, 0, ItemCount, item );

			if( closest >= 0 )
				return new SearchResult<TItem, TNode>( m_data[ closest ], this, closest );

			return null;
		}

		/// <summary>
		/// Copies the items from the current node only
		/// </summary>
		/// <param name="destinationArray">The destination array.</param>
		/// <param name="index">The index.</param>
		public void CopyItems( TItem[] destinationArray, int index )
		{
			m_data.CopyTo( destinationArray, index );
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
		/// Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo( TItem[] array, int arrayIndex )
		{
			foreach( TItem item in this )
			{
				array[ arrayIndex++ ] = item;
			}
		}

		public string ToDot()
		{
			return ToDot( i => i.ToString() );
		}

		public string ToDot( Func<TItem, string> toString )
		{
			var dot = new StringBuilder();

			dot.AppendLine( "digraph structs {{" );
			dot.AppendLine( "	node [shape=record, fontsize=9, fontname=Ariel];" );
			ToDot( dot, toString );
			dot.AppendLine( "}}" );

			return dot.ToString();
		}

		private void ToDot( StringBuilder dot, Func<TItem, string> toString )
		{
			dot.AppendFormat( "	struct{0} [shape=record, label=\"{{ {{ ", GetHashCode().ToString( "X" ) );

			for( int i = 0; i < m_data.Length; ++i )
			{
				if( i > 0 )
				{
					dot.Append( "| " );
				}

				if( i < ItemCount )
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
		private TNode CreateChild( TItem item )
		{
			//Create a new child node
			TNode newChild = Tree.CreateNode( m_minimum, m_data.Length );
			newChild.m_data[ 0 ] = item;
			newChild.ItemCount = 1;
			newChild.Parent = (TNode)this;

			return newChild;
		}

        private void UpdateHeight(HeightUpdateType updateType)
        {
            if( ItemCount == 0 )
            {
                m_height = -1;
            }
            else
            {
                int lheight = (Left != null) ? (Left.Height) : -1;
                int rheight = (Right != null) ? (Right.Height) : -1;

                m_height = 1 + Math.Max( lheight, rheight );
            }

            if( (updateType == HeightUpdateType.UpdateAllUpwards) && (Parent != null) )
            {
                Parent.UpdateHeight( updateType );
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
		private int BinarySearch<TSearch>( TItem[] array, int index, int length, TSearch value, Func<TSearch, TItem, int> comparer )
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
				if( ItemCount == 0 )
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

		#region IEnumerable<T>
		public IEnumerator<TItem> GetEnumerator()
		{
			if( Left != null )
			{
				foreach( var item in Left )
					yield return item;
			}

			for( int i = 0; i < ItemCount; ++i )
				yield return m_data[ i ];

			if( Right != null )
			{
				foreach( var item in Right )
					yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		public int Count
		{
			get
			{
				int count = ItemCount;

				if( Right != null )
					count += Right.Count;

				if( Left != null )
					count += Left.Count;

				return count;
			}
		}

		public int Height { get { return m_height; } }
		public int ItemCount { get; protected set; }
		public int MaxItems { get { return m_data.Length; } }
		public TNode Left { get; internal set; }
        public TNode Right { get; internal set; }
        public TNode Parent { get; internal set; }
		public TTree<TItem,TNode> Tree { get { return m_tree; } }
		public bool IsLeaf { get { return (Left == null) && (Right == null); } }
		public bool IsHalfLeaf { get { return !IsLeaf && ((Left == null) || (Right == null)); } }
		public bool IsInternal { get { return (Left != null) && (Right != null); } }
	}
}