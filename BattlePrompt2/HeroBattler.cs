using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using Nac;

namespace BattlePrompt
{
	public class HeroBattler : Battler
	{
		public HeroBattler( Messenger messenger, string name, int maxHp, int hp )
			: base( messenger, name, maxHp, hp )
		{
		}

		public override IEnumerable Think()
		{
			Skill skill = null;
			List<Battler> doers = new List<Battler>();
			List<Battler> targets = new List<Battler>();

			Messenger.AddLine( false, "{0}の行動は？", Name );
			skill = CUIMessage.Select( Score.Skills, s => s.Name );

			#region Doer/Target
			var members = Battle.GetParty( this, false, true );
			if( skill.DoerNum.HasValue )
			{
				if( skill.DoerNum > members.Count() + 1 )
				{
					yield break;
				}

				doers.Add( this );

				if( skill.DoerNum > 1 )
				{
					Messenger.AddLine( false, "誰に協力してもらう？" );
					doers.AddRange( CUIMessage.Select( members, skill.DoerNum.Value - 1, b => b.Name ) );
				}
			}
			else
			{
				doers.AddRange( members );
			}

			var rivals = Battle.GetRival( this, true );
			if( skill.TargetNum.HasValue )
			{
				if( rivals.Count() == 1 )
				{
					targets.Add( rivals.First() );
				}
				else
				{
					Messenger.AddLine( false, "だれに？" );
					targets.AddRange( CUIMessage.Select( rivals, skill.TargetNum.Value, b => b.Name ) );
				}
			}
			else
			{
				targets.AddRange( rivals );
			}
			#endregion

			if( skill != null )
				Tactics = new Tactics( skill, doers, targets );

			Messenger.AddLine();

			yield break;
		}
	}
}
