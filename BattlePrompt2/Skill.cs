using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePrompt
{
	public delegate IEnumerable<EventNode> Execution( SkillParamaters paramaters );

	public enum SkillRange
	{
		Single, Plural, All
	}

	public class Skill
	{
		public string Name { get; set; }
		public SkillRange DoerRange { get; set; }
		public SkillRange TargetRange { get; set; }
		public int? DoerNum { get; set; }
		public int? TargetNum { get; set; }
		public bool aliveOnly { get; set; }
		public Func<Battle, Battler, bool> GetCanExecute { get; set; }
		Execution getEvent { get; set; }

		public Skill( string name, SkillRange range, Execution getEvent )
		{
			this.Name = name;
			this.TargetRange = range;
			this.getEvent = getEvent;
		}
		public Skill( string name, int? doerNum, int? targetNum, Execution getEvent, bool aliveOnly = true )
		{
			this.Name = name;
			this.DoerNum = doerNum;
			this.TargetNum = targetNum;
			this.getEvent = getEvent;
			this.aliveOnly = aliveOnly;
		}
		public IEnumerable<EventNode> Subscribe( Battle battle, Battler doer, Battler target )
		{
			return Subscribe( battle, new[] { doer }, new[] { target } );
		}
		public IEnumerable<EventNode> Subscribe( Battle battle, Battler doer, IEnumerable<Battler> targets )
		{
			return Subscribe( battle, new[] { doer }, targets );
		}
		public IEnumerable<EventNode> Subscribe( Battle battle, IEnumerable<Battler> doers, Battler target )
		{
			return Subscribe( battle, doers, new[] { target } );
		}
		public IEnumerable<EventNode> Subscribe( Battle battle, IEnumerable<Battler> doers, IEnumerable<Battler> targets )
		{
			foreach( var item in getEvent( new SkillParamaters( battle, doers, targets ) ).ToEventNode().Subscribe() )
			{
				yield return item;
			}
		}

		public bool CanExecute( Battle battle, Battler doer )
		{
			if( DoerNum.HasValue && battle.GetParty( doer, true, true ).Count() < DoerNum ) return false;
			if( GetCanExecute != null && !GetCanExecute( battle, doer ) ) return false;
			return true;
		}
	}

	public class SkillParamaters
	{
		public Battle Battle { get; set; }
		public IEnumerable<Battler> Doers { get; set; }
		public IEnumerable<Battler> Targets { get; set; }
		public Battler Doer
		{
			get { return Doers.First(); }
		}
		public Battler Target
		{
			get { return Targets.First(); }
		}

		public SkillParamaters( Battle battle, IEnumerable<Battler> doers, IEnumerable<Battler> targets )
		{
			this.Battle = battle;
			this.Doers = doers;
			this.Targets = targets;
		}
		public IEnumerable<SkillParamaters> GetDoerParamaters()
		{
			return Doers.Select( x => new SkillParamaters( Battle, new[] { x }, Targets ) );
		}
		public IEnumerable<SkillParamaters> GetTargetParamaters()
		{
			return Targets.Select( x => new SkillParamaters( Battle, Doers, new[] { x } ) );
		}
	}

}
