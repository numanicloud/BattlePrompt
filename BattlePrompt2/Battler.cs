using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using Nac.Helpers;

namespace BattlePrompt
{
	public class Battler
	{
		public string Name { get; set; }
		public ActorScore Score { get; set; }
		public bool IsAlive
		{
			get { return Score.Hp.Value > 0; }
		}

		public Messenger Messenger { get; set; }
		public Battle Battle { get; set; }
		protected Tactics Tactics { get; set; }

		public Battler( Messenger messenger, string name, int maxHp, int hp )
		{
			this.Messenger = messenger;
			this.Name = name;
			this.Score = new ActorScore( maxHp, hp );
		}
		public virtual string Show( string format )
		{
			StringBuilder state = new StringBuilder();
			if( !IsAlive )
			{
				state.Append( "[戦闘不能]" );
			}

			var show = format.Replace( "{N}", Name )
				.Replace( "{h}", Score.Hp.Value.ToString() )
				.Replace( "{H}", Score.MaxHp.Value.ToString() )
				.Replace( "{St}", state.ToString() );

			return show;
		}
		public virtual void AddSkill( Skill skill )
		{
			Score.Skills.Add( skill );
		}
		public virtual IEnumerable Think()
		{
			var skill = Score.Skills.Where( x => x.CanExecute( Battle, this ) ).GetRandom();
			List<Battler> doers = new List<Battler>();
			List<Battler> targets = new List<Battler>();

			if( skill.TargetNum.HasValue )
			{
				for( int i = 0; i < skill.TargetNum; i++ )
				{
					targets.Add( Battle.GetRival( this, true ).GetRandom() );
				}
			}
			else
			{
				targets = Battle.GetRival( this, true ).ToList();
			}

			if( skill.DoerNum.HasValue )
			{
				doers.Add( this );
				for( int i = 0; i < skill.DoerNum - 1; i++ )
				{
					doers.Add( Battle.GetParty( this, true, true ).GetRandom() );
				}
			}
			else
			{
				doers = Battle.GetParty( this, true, true ).ToList();
			}

			Tactics = new Tactics( skill, new[] { this }, targets );
			yield break;
		}
		public virtual IEnumerable<EventNode> OnTurn()
		{
			IEnumerable<EventNode> result = null;

			if( Tactics != null && Tactics.Skill.CanExecute( Battle, this ) )
			{
				if( Tactics.Skill.aliveOnly && Tactics.Skill.TargetNum.HasValue && Tactics.Skill.TargetNum > Tactics.Targets.Count( x => x.IsAlive ) )
				{
					var list = new List<Battler>( Tactics.Targets.Where( x => x.IsAlive ) );
					var rivals = Battle.GetRival( this, Tactics.Skill.aliveOnly ).Except( list );
					Tactics.Targets = list.Concat( rivals.GetRandom( Math.Min( rivals.Count(), Tactics.Skill.TargetNum.Value - list.Count ) ) );
				}
				if( Tactics.Skill.DoerNum.HasValue && Tactics.Skill.DoerNum > Tactics.Doers.Count( x => x.IsAlive ) )
				{
					var list = new List<Battler>( Tactics.Targets.Where( x => x.IsAlive ) );
					var members = Battle.GetParty( this, true, true ).Except( list );
					if( Tactics.Skill.DoerNum - list.Count > members.Count() )
						return Enumerable.Empty<EventNode>();
					else
						Tactics.Doers = list.Concat( members.GetRandom( Math.Min( members.Count(), Tactics.Skill.TargetNum.Value - list.Count ) ) );
				}

				result = Tactics.Skill.Subscribe( Battle, Tactics.Doers, Tactics.Targets );
			}
			else
			{
				result = Enumerable.Empty<EventNode>();
			}

			Tactics = null;
			return result;
		}
	}
}
