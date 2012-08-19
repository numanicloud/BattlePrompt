using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePrompt
{
	public static class CUIMessage
	{
		public static int Indent = 0;
		public static void WriteColor( ConsoleColor color, string format, params object[] args )
		{
			var temp = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write( string.Empty.PadLeft( Indent ) + format, args );
			Console.ForegroundColor = temp;
		}
		public static void Interact( string format, params object[] args )
		{
			Console.Write( string.Empty.PadLeft( Indent ) + format, args );
			while( true )
			{
				var key = Console.ReadKey( true );
				if( key.Key == ConsoleKey.Enter ) break;
			}
		}
		public static void InteractColor( ConsoleColor color, string format, params object[] args )
		{
			WriteColor( color, string.Empty.PadLeft( Indent ) + format, args );
			while( true )
			{
				var key = Console.ReadKey( false );
				if( key.Key == ConsoleKey.Enter ) break;
			}
		}
		public static void DrawLine( ConsoleColor color = ConsoleColor.Gray )
		{
			int num = Console.WindowWidth-1;
			WriteColor( color, string.Empty.PadLeft( num, '-' ) + Environment.NewLine );
		}

		public static T Select<T>( IEnumerable<T> contents, Func<T, string> toName )
		{
			StringBuilder builder = new StringBuilder();
			for( int i = 0; i < contents.Count(); i++ )
			{
				builder.AppendFormat( "[{0}]{1}", i+1, toName( contents.ElementAt( i ) ) );
				if( i + 1 < contents.Count() )
					builder.Append( ", " );
			}

			Console.WriteLine( builder );

			while( true )
			{
				Console.Write( " > " );
				int selection;

				if( int.TryParse( Console.ReadLine(), out selection ) 
					&& selection > 0
					&& selection < contents.Count() + 1 )
				{
					return contents.ElementAt( selection - 1 );
				}
			}
		}
		public static IEnumerable<T> Select<T>( IEnumerable<T> source, int num, Func<T, string> toName )
		{
			var result = new List<T>();
			for( int i = 0; i < num; i++ )
			{
				var c = source.Except( result );
				if( c.Count() == 0 ) break;
				result.Add( Select( c, toName ) );
			}
			return result;
		}
	}
}
