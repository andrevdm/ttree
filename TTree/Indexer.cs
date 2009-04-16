using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTree
{
	/// <summary>
	/// Helper class for creating indexers
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Indexer<T>
	{
		private readonly Func<int, T> m_funcIndex;
		private readonly Func<int> m_funcCount;

		public Indexer( Func<int, T> funcIndex, Func<int> funcCount )
		{
			#region param checks
			if( funcIndex == null )
				throw new ArgumentNullException( "funcIndex" );

			if( funcCount == null )
				throw new ArgumentNullException( "funcCount" );
			#endregion

			m_funcIndex = funcIndex;
			m_funcCount = funcCount;
		}

		public int Count { get { return m_funcCount(); } }
		public T this[ int index ] { get { return m_funcIndex( index ); } }
	}
}
