using System;

namespace TTree
{
	public class SearchResult<TItem,TNode>
		where TItem : IComparable
        where TNode : TTreeNode<TItem, TNode>
	{
        public SearchResult( TItem value, TTreeNode<TItem,TNode> node, int index )
		{
			Value = value;
			Node = node;
			Index = index;
		}

        public TItem Value { get; private set; }
		public TTreeNode<TItem,TNode> Node { get; private set; }
		public int Index { get; private set; }
	}
}
