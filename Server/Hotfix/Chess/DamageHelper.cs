using MongoDB.Driver.Core.Events;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpCombatComponent))]
	public static class DamageHelper
	{
		public static async ETTask Damage(GamePlayComponent gamePlayComponent, Unit unit, Unit target, int damage, long attacktime)
		{
			target.GetComponent<NumericComponent>().Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - damage);
			G2C_AttackDamage message = new G2C_AttackDamage();
			message.FromId = unit.Id;
			message.ToId = target.Id;
			message.Damage = damage;
			message.attackTime = attacktime;
			gamePlayComponent.Broadcast(message);

			if (target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
			{
				await TimerComponent.Instance.WaitAsync(attacktime);
				gamePlayComponent.Broadcast(new G2C_UnitDead()
				{
					UnitId = target.Id
				});
				Log.Info($"{target.Id} is dead");
				CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				cpCombatComponent.target = null;
			}
			else
			{
				Log.Info($"{unit.Id} attack {target.Id} damage {damage}");
				Log.Info($"{target.Id} hp is {target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp)}");
			}
		}
	}
}
