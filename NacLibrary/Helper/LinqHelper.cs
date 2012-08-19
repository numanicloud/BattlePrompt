using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 一部neueccさんから借りた
namespace Nac.Helpers
{
	public class CompareSelector<T, TKey> : IEqualityComparer<T>
	{
		private Func<T, TKey> selector;

		public CompareSelector( Func<T, TKey> selector )
		{
			this.selector = selector;
		}

		public bool Equals( T x, T y )
		{
			return selector( x ).Equals( selector( y ) );
		}
		public int GetHashCode( T obj )
		{
			return selector( obj ).GetHashCode();
		}
	}

	public static class LinqHelper
	{
		public static void Foreach<T>( this IEnumerable<T> source, Action<T> action )
		{
			foreach( var item in source )
				action( item );
		}
		public static IEnumerable<T> Distinct<T, TKey>( this IEnumerable<T> source, Func<T, TKey> selector )
		{
			return source.Distinct( new CompareSelector<T, TKey>( selector ) );
		}
		public static IEnumerable<T> Except<T, TKey>( this IEnumerable<T> source, IEnumerable<T> second, Func<T, TKey> selector )
		{
			return source.Except( second, new CompareSelector<T, TKey>( selector ) );
		}
		public static IEnumerable<T> DeleteNull<T>( this IEnumerable<T> source )
		{
			return source.Where( x => x != null );
		}
		public static T GetRandom<T>( this IEnumerable<T> collection, Random machine = null )
		{
			if( collection.Count() == 0 ) return default( T );
			if( machine == null ) machine = new Random();
			return collection.ElementAt( machine.Next( collection.Count() ) );
		}
		public static IEnumerable<T> GetRandom<T>( this IEnumerable<T> source, int num, Random machine = null )
		{
			if( source.Count() == 0 ) return Enumerable.Empty<T>();
			if( machine == null ) machine = new Random();

			var s = new List<T>( source );
			var result = new List<T>();
			for( int i = 0; i < num; i++ )
			{
				var x = s.GetRandom( machine );
				s.Remove( x );
				result.Add( x );
			}

			return result;
		}
	}
}
