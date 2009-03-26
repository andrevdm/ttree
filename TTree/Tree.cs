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
			//Is there space in this node?
			if( Count < m_minimum )
			{
				int closest = Array.BinarySearch<T>( m_data, 0, Count, item );

				if( closest < 0 )
				{
					closest = ~closest;

					if( closest > Count )
						closest = Count;
				}

				Array.Copy( m_data, closest, m_data, closest + 1, Count - closest );
				m_data[ closest ] = item;
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
