using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nac;
using System.Reactive.Subjects;
using System.Collections;

namespace BattlePrompt
{
	class Program
	{
		public static Messenger Messenger = new Messenger();

		static void Main( string[] args )
		{
			Messenger.Input = Console.ReadLine;
			Messenger.Message.Subscribe( MessageWriter );

			var hero = new HeroBattler( Messenger, "勇者", 20, 20 );
			var witch = new HeroBattler( Messenger, "魔女", 18, 18 );
			var enemy = new Battler( Messenger, "スキュラ", 20, 20 );
			var enemy2 = new Battler( Messenger, "モノアイ", 16, 16 );

			var atackSkill = new Skill( "攻撃", 1, 1, Atack );
			var pluralAtackSkill = new Skill( "連続攻撃", 1, 1, PluralAtack );
			var starSkill = new Skill( "星を落とす魔法", 1, 3, StarMagic );
			var fireSkill = new Skill( "炎の剣", 2, 1, FireSlash );

			hero.AddSkill( atackSkill );
			hero.AddSkill( pluralAtackSkill );
			hero.AddSkill( fireSkill );
			witch.AddSkill( atackSkill );
			witch.AddSkill( starSkill );
			enemy.AddSkill( atackSkill );
			enemy.AddSkill( pluralAtackSkill );
			enemy2.AddSkill( atackSkill );

			var battle = new Battle( Messenger, new[] { hero, witch }, new[] { enemy, enemy2 } );
			var sub = battle.Run().ToMicrothread();
			while( sub.Current == BattleState.Fighting )
			{
				sub.Yield();
			}

			Console.WriteLine( "End" );
			Console.ReadLine();
		}
		static void MessageWriter( MessageEvent message )
		{
			if( message.Wait )
			{
				CUIMessage.Interact( message.Message );
			}
			else
			{
				Console.Write( message.Message );
			}
		}

		#region イベント
		static IEnumerable<EventNode> StarMagic( SkillParamaters paramaters )
		{
			Messenger.AddLine( true, "{0}は星を落とす魔法を使った！", paramaters.Doer.Name );
			foreach( var item in paramaters.GetTargetParamaters() )
			{
				yield return Damage( item, 4 ).ToEventNode();
			}

			Messenger.AddLine();
			yield break;
		}
		static IEnumerable<EventNode> FireSlash( SkillParamaters paramaters )
		{
			if( paramaters.Doers.Count() < 2 ) yield break;
			Messenger.AddLine( true, "{0}は{1}の協力でもって炎の剣を繰り出した！", paramaters.Doers.ElementAt( 0 ).Name, paramaters.Doers.ElementAt( 1 ).Name );
			yield return Damage( paramaters, 12 ).ToEventNode();

			Messenger.AddLine();
			yield break;
		}
		static IEnumerable<EventNode> PluralAtack( SkillParamaters paramaters )
		{
			Messenger.AddLine( true, "{0}の連続攻撃！", paramaters.Doer.Name );

			var rand = new Random().Next() % 3 + 2;
			for( int i = 0; i < rand; i++ )
			{
				yield return Damage( paramaters, 2 ).ToEventNode();
			}

			Messenger.AddLine();
			yield break;
		}
		static IEnumerable<EventNode> Atack( SkillParamaters paramaters )
		{
			Messenger.AddLine( true, "{0}は{1}に殴りかかった！", paramaters.Doer.Name, paramaters.Target.Name );
			yield return Damage( paramaters, 5 ).ToEventNode();

			Messenger.AddLine();
			yield break;
		}
		static IEnumerable<EventNode> Damage( SkillParamaters paramaters, int damage )
		{
			Messenger.AddLine( true, "{0}に{1}ダメージ！", paramaters.Target.Name, damage );
			paramaters.Target.Score.Hp.Value -= damage;
			yield return null;
		}
		#endregion
	}
}
