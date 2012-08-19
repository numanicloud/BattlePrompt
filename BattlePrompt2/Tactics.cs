using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePrompt
{
	public class Tactics
	{
		public Skill Skill { get; set; }
		public IEnumerable<Battler> Doers { get; set; }
		public IEnumerable<Battler> Targets { get; set; }

		public Tactics( Skill skill, IEnumerable<Battler> doers, IEnumerable<Battler> targets )
		{
			this.Skill = skill;
			this.Doers = doers;
			this.Targets = targets;
		}
	}
}
