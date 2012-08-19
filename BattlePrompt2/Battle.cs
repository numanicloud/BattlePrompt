using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Nac.Helpers;
using Nac;

namespace BattlePrompt
{
	public enum BattleState
	{
		Fighting, Win, Lose
	}

	public class Battle
	{
		Messenger messenger { get; set; }
		public List<HeroBattler> Heros = new List<HeroBattler>();
		public List<Battler> Enemies = new List<Battler>();
		public IEnumerable<Battler> Battlers
		{
			get { return Heros.Concat( Enemies ); }
		}

		public Battle( Messenger messenger, IEnumerable<HeroBattler> heros, IEnumerable<Battler> enemies )
		{
			this.messenger = messenger;
			this.Heros.AddRange( heros.ToArray() );
			this.Enemies.AddRange( enemies.ToArray() );
			this.Battlers.Foreach( x => x.Battle = this );
		}

		public IEnumerable<BattleState> Run()
		{
			var state = BattleState.Fighting;
			var mt = BattleMain().ToMicrothread();
			while( state == BattleState.Fighting )
			{
				mt.Yield();
				if( Heros.All( x => !x.IsAlive ) )
				{
					state = BattleState.Lose;
				}
				else if( Enemies.All( x => !x.IsAlive ) )
				{
					state = BattleState.Win;
				}
				yield return state;
			}
		}
		private IEnumerable BattleMain()
		{
			while( true )
			{
				Console.Clear();
				CUIMessage.DrawLine();
				Heros.ForEach( x => Console.WriteLine( x.Show( "[{N}] HP:{h}/{H} {St}" ) ) );
				Console.WriteLine();
				Enemies.ForEach( x => Console.WriteLine( x.Show( "[{N}] HP:{h}/{H} {St}" ) ) );
				CUIMessage.DrawLine();

				foreach( var item in GetThread( GetBattlers( true, true ), x => x.Think() ) ) yield return item;
				foreach( var item in GetThread( GetBattlers( true, true ), x => x.OnTurn() ) ) yield return item;
			}
		}
		private IEnumerable GetThread( IEnumerable<Battler> battlers, Func<Battler, IEnumerable> getMicrothread )
		{
			foreach( var item in battlers )
			{
				foreach( var item2 in getMicrothread( item ) )
				{
					yield return item2;
				}
			}
		}

		public IEnumerable<Battler> GetBattlers( bool heroIsAhead, bool aliveOnly )
		{
			var result = heroIsAhead ? Heros.Concat( Enemies ) : Enemies.Concat( Heros );
			if( aliveOnly )
			{
				result = result.Where( x => x.IsAlive );
			}
			return result;
		}
		public IEnumerable<Battler> GetParty( Battler side, bool containsItself, bool aliveOnly )
		{
			IEnumerable<Battler> result;
			if( Heros.Contains( side ) )
			{
				result = Heros;
			}
			else if( Enemies.Contains( side ) )
			{
				result = Enemies;
			}
			else
			{
				return Enumerable.Empty<Battler>();
			}

			if( !containsItself )
			{
				result = result.Except( new[] { side } );
			}
			if( aliveOnly )
			{
				result = result.Where( x => x.IsAlive );
			}

			return result;
		}
		public IEnumerable<Battler> GetRival( Battler side, bool aliveOnly )
		{
			IEnumerable<Battler> result;
			if( Heros.Contains( side ) )
			{
				result = Enemies;
			}
			else if( Enemies.Contains( side ) )
			{
				result = Heros;
			}
			else
			{
				return Enumerable.Empty<Battler>();
			}

			if( aliveOnly )
			{
				result = result.Where( x => x.IsAlive );
			}

			return result;
		}
	}
}
