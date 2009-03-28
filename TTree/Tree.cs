using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TTree
{
	/// <summary>
	/// T-Tree implementation.
	/// Google "A Study of Index Structures for Main Memory Database Management Systems"
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Tree<T> //visitor, IEnumerable etc
		where T : IComparable
	{
		protected readonly T[] m_data;
		protected readonly int m_minimum;
		protected int m_height = 0;

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

		/// <summary>
		/// Inserts the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was added or false if it already existed and was not </returns>
		public bool Insert( T item )
		{
			//Is this the bounding node for the new item? If the node is empty it is considered to be the bounding node.
			bool isBoundingNode = (Count == 0) || ((item.CompareTo( m_data[ 0 ] ) >= 0) && (item.CompareTo( m_data[ Count - 1 ] ) <= 0));

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
						//There is no left child, so create on
						Left = CreateChild( oldMinimum );
						UpdateHeight();
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
				if( (Left != null) && (item.CompareTo( m_data[ 0 ] ) < 0) )
					return Left.Insert( item );

				//If the item is less than the maximum and there is a right node, follow it
				if( (Right != null) && (item.CompareTo( m_data[ 0 ] ) > 0) )
					return Right.Insert( item );


				//If we are here then, there is no bounding node for this value.
				Debug.Assert( IsLeaf || IsHalfLeaf, "Something is very wrong if this node is not a leaf or half-leaf." );

				//Is there place in this node
				if( Count < m_data.Length )
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
					Tree<T> newChild = CreateChild( item );

					//Add it as the the left or the right child
					if( item.CompareTo( m_data[ 0 ] ) < 0 )
					{
						Debug.Assert( Left == null, "The left node should be null" );
						Left = newChild;
					}
					else
					{
						Debug.Assert( Left == null, "The right node should be null" );
						Right = newChild;
					}
					
					UpdateHeight();
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

		//private virtual Rebalance

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

		public void CopyItems( T[] destinationArray, int index )
		{
			m_data.CopyTo( destinationArray, index );
		}

		public string ToDot()
		{
			return ToDot( i => i.ToString() );
		}

		public string ToDot( Func<T,string> toString )
		{
			var dot = new StringBuilder();

			dot.AppendLine( "digraph structs {{" );
			dot.AppendLine( "	node [shape=record, fontsize=9, fontname=Ariel];" );
			ToDot( dot, toString );
			dot.AppendLine( "}}" );

			return dot.ToString();
		}

		private void ToDot( StringBuilder dot, Func<T,string> toString )
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

			dot.AppendFormat( " }} | {{ <left> . | <m> h={0} | <right> . }} }}\"];", Height ).AppendLine();

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
		private Tree<T> CreateChild( T item )
		{
			//Create a new child node
			Tree<T> newChild = new Tree<T>( m_minimum, m_data.Length );
			newChild.m_data[ 0 ] = item;
			newChild.Count = 1;
			newChild.Parent = this;

			newChild.UpdateHeight();

			return newChild;
		}

		private void UpdateHeight()
		{
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
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height { get { return m_height; } }
		public int Count { get; protected set; }
		public int MaxItems { get { return m_data.Length; } }
		public Tree<T> Left { get; set; }
		public Tree<T> Right { get; set; }
		public Tree<T> Parent { get; set; }
		public bool IsLeaf { get { return (Left == null) && (Right == null); } }
		public bool IsHalfLeaf { get { return !IsLeaf && ((Left == null) || (Right == null)); } }
	}	
}
