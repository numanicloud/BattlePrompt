using System;
using System.Collections.Generic;
using System.Linq;
using Nac.Geometory;

namespace Nac.Helpers
{
	public static class Helper
	{
		public static void DisposeRange( IEnumerable<IDisposable> array )
		{
			foreach( var item in array )
				item.Dispose();
		}
		public static void DisposeRange( IEnumerator<IDisposable> array )
		{
			while( array.MoveNext() )
				array.Current.Dispose();
		}

		[Obsolete]
		public static int GetEnumLength( this Type enumType )
		{
			return Enum.GetValues( enumType ).Length;
		}
		public static int GetEnumLength<T>()
		{
			return Enum.GetValues( typeof( T ) ).Length;
		}
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues( typeof( T ) ).Cast<T>();
		}

		public static void Swap<Type>( ref Type obj1, ref Type obj2 )
		{
			Type temp = obj1;
			obj1 = obj2;
			obj2 = obj1;
		}

		public static void TryInvoke( this Action action )
		{
			if( action != null )
				action();
		}
		public static void TryInvoke<T1>( this Action<T1> action, T1 arg1 )
		{
			if( action != null )
				action( arg1 );
		}
		public static void TryInvoke<T1, T2>( this Action<T1,T2> action, T1 arg1, T2 arg2 )
		{
			if( action != null )
				action( arg1, arg2 );
		}
		public static void TryInvoke<TResult>( this Func<TResult> function, ref TResult result )
		{
			if( function != null )
				result = function();
		}
		public static void TryInvoke<TResult, T1>( this Func<T1, TResult> function, T1 arg1, ref TResult result )
		{
			if( function != null )
				result = function( arg1 );
		}
		public static void TryInvoke<TResult, T1, T2>( this Func<T1, T2, TResult> function, T1 arg1, T2 arg2, ref TResult result )
		{
			if( function != null )
				result = function( arg1, arg2 );
		}
	}
}
