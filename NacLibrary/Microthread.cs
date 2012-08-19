using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Nac.Helpers;
using System.Threading.Tasks;

namespace Nac
{
	public class Microthread<T>
	{
		IEnumerator<T> thread { get; set; }
		public bool IsEnabled { get { return thread != null; } }
		public T Current { get { return IsEnabled ? thread.Current : default( T ); } }
		public event Action OnFinish;

		public Microthread( IEnumerable<T> thread )
		{
			this.thread = thread.GetEnumerator();
		}
		public Microthread( IEnumerator<T> thread )
		{
			this.thread = thread;
		}
		public T Yield()
		{
			if( IsEnabled && !thread.MoveNext() )
			{
				OnFinish.TryInvoke();
				thread = null;
			}
			return Current;
		}

		public IEnumerator<T> GetEnumerator()
		{
			while( thread.MoveNext() )
			{
				yield return thread.Current;
			}
		}
	}

	public static class MTHelper
	{
		public static Microthread<object> ToMicrothread( this IEnumerable enumerable )
		{
			return new Microthread<object>( enumerable.Cast<object>() );
		}
		public static Microthread<object> ToMicrothread( this IEnumerator enumerator )
		{
			return new Microthread<object>( enumerator as IEnumerator<object> );
		}
		public static Microthread<T> ToMicrothread<T>( this IEnumerable<T> enumerable )
		{
			return new Microthread<T>( enumerable );
		}
		public static Microthread<T> ToMicrothread<T>( this IEnumerator<T> enumerator )
		{
			return new Microthread<T>( enumerator );
		}
		public static IEnumerable Start<T>( Func<T> func )
		{
			var task = Task.Factory.StartNew( func );
			while( !task.IsCompleted )
			{
				yield return null;
			}
		}
		public static IEnumerable Start( Action func )
		{
			var task = Task.Factory.StartNew( func );
			while( !task.IsCompleted )
			{
				yield return null;
			}
		}
	}

}
