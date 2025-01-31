using MongoDB.Driver.Core.Events;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpCombatComponent))]
	public static class DamageHelper
	{
		public static async ETTask Damage(GamePlayComponent gamePlayComponent, Unit unit, Unit target, int damage, long attacktime)
		{
			target.GetComponent<NumericComponent>().Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - damage);

			int hp = target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp);

			G2C_AttackDamage message = new G2C_AttackDamage();
			message.FromId = unit.Id;
			message.ToId = target.Id;
			message.Damage = damage;
			message.AttackTime = attacktime;
			message.HP = hp;
			message.MaxHP = target.GetComponent<NumericComponent>().GetAsInt(NumericType.MaxHp);
			gamePlayComponent.Broadcast(message);

			await TimerComponent.Instance.WaitAsync(attacktime);
			if (hp <= 0)
			{
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
				if (!unit.IsDisposed)
				{
					Log.Info($"{unit.Id} attack {target.Id} damage {damage}");
					Log.Info($"{target.Id} hp is {hp}");
				}
			}
		}
	}
}
