using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace BattlePrompt
{
	public class Messenger
	{
		public Subject<MessageEvent> Message = new Subject<MessageEvent>();
		public Func<string> Input { get; set; }

		public void AddMessage( bool wait, string format, params object[] args )
		{
			Message.OnNext( new MessageEvent( wait, format, args ) );
		}
		public void AddLine( bool wait, string format, params object[] args )
		{
			Message.OnNext( new MessageEvent( wait, format + Environment.NewLine, args ) );
		}
		public void AddLine()
		{
			Message.OnNext( new MessageEvent( false, Environment.NewLine ) );
		}
		public T Select<T>( IEnumerable<T> contents, Func<T, string> toName )
		{
			StringBuilder builder = new StringBuilder();
			for( int i = 0; i < contents.Count(); i++ )
			{
				builder.AppendFormat( "[{0}]{1}", i + 1, toName( contents.ElementAt( i ) ) );
				if( i + 1 < contents.Count() )
					builder.Append( ", " );
			}

			AddLine( false, builder.ToString() );

			int selection = 0;
			while( selection > 0 && selection < contents.Count() + 1 )
			{
				int.TryParse( Input(), out selection );
			}
			return contents.ElementAt( selection - 1 );
		}
	}

	public class MessageEvent
	{
		public string Message { get; set; }
		public bool Wait { get; set; }

		public MessageEvent( bool wait, string format, params object[] args )
		{
			this.Message = string.Format( format, args );
			this.Wait = wait;
		}
	}
}
