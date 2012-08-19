using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nac;

namespace BattlePrompt
{
	public class ActorScore
	{
		public ClampedValue<int> MaxHp { get; set; }
		public ClampedValue<int> Hp { get; set; }
		public List<Skill> Skills { get; set; }

		public ActorScore( int maxHp, int hp )
		{
			this.MaxHp = new ClampedValue<int>( maxHp, 1, 999 );
			this.Hp = new ClampedValue<int>( hp, 0, this.MaxHp.Value );
			Skills = new List<Skill>();
		}
	}
}
