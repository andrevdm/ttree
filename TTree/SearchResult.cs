using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTree
{
	public class SearchResult<T>
		where T : IComparable
	{
		public SearchResult( T value, TTreeNode<T> node, int index )
		{
			Value = value;
			Node = node;
			Index = index;
		}

		public T Value { get; private set; }
		public TTreeNode<T> Node { get; private set; }
		public int Index { get; private set; }
	}
}
