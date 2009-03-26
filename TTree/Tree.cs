using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public void Insert( T item )
		{
			//Find the position with the closest value to the item being added.
			int closest = Array.BinarySearch<T>( m_data, 0, Count, item );

			//If closest is posative then the item already exists at this level, so
			// no need to add it again
			if( closest >= 0 )
				return;

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
		public Tree<T> Parent { get; set; }
		public Tree<T> Left { get; set; }
		public Tree<T> Right { get; set; }
	}
}
