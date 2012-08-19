using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePrompt
{
	public class EventNode
	{
		IEnumerable<EventNode> Behavior { get; set; }

		public EventNode( IEnumerable<EventNode> behavior )
		{
			this.Behavior = behavior;
		}
		public EventNode( IEnumerable<EventNode> behavior, IEnumerable<EventNode> before, IEnumerable<EventNode> after )
		{
			this.Behavior = before.Concat( behavior ).Concat( after );
		}

		public IEnumerable<EventNode> Subscribe()
		{
			foreach( var item in Behavior )
			{
				yield return item;
				if( item != null )
				{
					foreach( var item2 in item.Subscribe() )
					{
						yield return item2;
					}
				}
			}
		}

		public static EventNode None
		{
			get { return new EventNode( NoneIE ); }
		}
		public static IEnumerable<EventNode> NoneIE
		{
			get { yield break; }
		}
	}

	public static class EventNodeHelper
	{
		public static EventNode ToEventNode( this IEnumerable<EventNode> behavior )
		{
			return new EventNode( behavior );
		}
	}

}
