using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TTree
{
	public class Tree<T> //visitor, IEnumerable etc
		where T : IComparable
	{
		protected readonly T[] m_data;
		protected readonly int m_minimum;

		public Tree( int minimum, int maximum )
		{
			#region param checks
			if( minimum < 1 )
				throw new ArgumentOutOfRangeException( "order", "Expecting an order of at least 1." );

			if( maximum < minimum )
				throw new ArgumentOutOfRangeException( "maximum", "Maximum value must be greater than the minimum. " );
			#endregion

			m_data = new T[ maximum ];
			Count = 0;
			m_minimum = minimum;
		}

		public bool Insert( T item )
		{
			//Is this the bounding node for the new item? If the node is empty it is considered to be the bounding node.
			bool isBoundingNode = (Count == 0) || ((item.CompareTo( m_data[ 0 ] ) >= 0) && (item.CompareTo( m_data[ Count - 1 ] ) <= 0));

			if( isBoundingNode )
			{
				//This is the bounding node, add the new item
				return InsertInCurrentNode( item );
			}
			else
			{
				//If the item is less than the minimum and there is a left node, follow it
				if( (Left != null) && (item.CompareTo( m_data[ 0 ] ) < 0) )
					return Left.Insert( item );

				//If the item is less than the maximum and there is a right node, follow it
				if( (Right != null) && (item.CompareTo( m_data[ 0 ] ) > 0) )
					return Right.Insert( item );


				//If we are here then, there is no bounding node for this value.
				Debug.Assert( IsLeaf || IsHalfLeaf, "Something is very wrong if this node is not a leaf or half-leaf." );

				//Is this node full?
				if( Count < m_minimum )
				{
					//There is place in this node so add the new value. However since this value
					// must be the new minimum or maximum (otherwise it would have found a bounding
					// node) dont call InsertInCurrentNode which would do a binary search to find
					// an insert location. Rather check for min/max and add it here.
					if( item.CompareTo( m_data[ 0 ] ) > 0 )
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
					//TODO overflow
					throw new NotImplementedException();
				}
			}

			return true;
		}

		private bool InsertInCurrentNode( T item )
		{
			Debug.Assert( Count < m_data.Length, "This method must never be called on a full node" );

			//TODO profile and see if there is any advantage to checking the min and max values here and setting closest=0 or closest=Count. Remember to check that the item was not already added

			//Find the position with the closest value to the item being added.
			int closest = Array.BinarySearch<T>( m_data, 0, Count, item );

			//If closest is positive then the item already exists at this level, so
			// no need to add it again
			if( closest >= 0 )
				return true;

			//Is there space in this node?
			if( Count < m_minimum )
			{
				//Negate the result, which gives info about where the closest match is
				closest = ~closest;

				//If closest is greater than the count then there is no item in the array than the item being added,
				// so add it to the end of the array. Otherwise the negated value is the position to add the item to
				if( closest > Count )
					closest = Count;

				//Shift the items up by one place to make space for the new item. This also works when adding
				// an item to the end of the array.
				Array.Copy( m_data, closest, m_data, closest + 1, Count - closest );
				m_data[ closest ] = item;

				//An item has been added.
				Count++;
			}
			else
			{
				throw new NotImplementedException();
			}

			return true;
		}

		public void Delete( T item )
		{
			throw new NotImplementedException();
		}

		public T Search( T item )
		{
			//Array.BinarySearch<T>( m_data, 

			throw new NotImplementedException();
		}

		public T Search( Func<T, bool> match )
		{
			throw new NotImplementedException();
		}

		public void CopyArray( T[] destinationArray, int index )
		{
			m_data.CopyTo( destinationArray, index );
		}

		public int Count { get; protected set; }
		public Tree<T> Left { get; set; }
		public Tree<T> Right { get; set; }
		public Tree<T> Parent { get; set; }
		public bool IsLeaf { get { return (Left == null) && (Right == null); } }
		public bool IsHalfLeaf { get { return !IsLeaf && ((Left == null) || (Right == null)); } }
	}
}
