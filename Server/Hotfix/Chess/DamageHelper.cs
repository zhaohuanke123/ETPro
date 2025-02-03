using MongoDB.Driver.Core.Events;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpCombatComponent))]
	public static class DamageHelper
	{
		public static async ETTask Damage(GamePlayComponent gamePlayComponent, Unit unit, Unit target, int damage, ChampionConfig config)
		{
			target.GetComponent<NumericComponent>().Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - damage);

			int hp = target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp);
			long attackTime = config.allAttacktime;

			G2C_AttackDamage message = new G2C_AttackDamage();
			message.FromId = unit.Id;
			message.ToId = target.Id;
			message.Damage = damage;
			message.AttackTime = attackTime;
			message.HP = hp;
			message.MaxHP = target.GetComponent<NumericComponent>().GetAsInt(NumericType.MaxHp);
			gamePlayComponent.Broadcast(message);

			await TimerComponent.Instance.WaitAsync(attackTime);

			if (config.attackProjectile != "")
			{
				float distance = Vector3.Distance(unit.Position, target.Position);
				float time = distance / config.projSpeed * 1000;
				await TimerComponent.Instance.WaitAsync((long)time);
			}
			if (hp <= 0)
			{
				gamePlayComponent.Broadcast(new G2C_UnitDead()
				{
					UnitId = target.Id
				});
				Log.Info($"{target.Id} is dead");
				CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				cpCombatComponent.target = null;
				await TimerComponent.Instance.WaitAsync(attackTime);
			}
			else
			{
				if (!unit.IsDisposed)
				{
					Log.Info($"{unit.Id} attack {target.Id} damage {damage}");
					Log.Info($"{target.Id} hp is {hp}");
				}
			}
		}
	}
}
