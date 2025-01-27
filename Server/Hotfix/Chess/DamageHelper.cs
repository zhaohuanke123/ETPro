using MongoDB.Driver.Core.Events;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpCombatComponent))]
	public static class DamageHelper
	{
		public static void Damage(GamePlayComponent gamePlayComponent, Unit unit, Unit target, int damage, long attacktime)
		{
			target.GetComponent<NumericComponent>().Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - damage);
			G2C_AttackDamage message = new G2C_AttackDamage();
			message.FromId = unit.Id;
			message.ToId = target.Id;
			message.Damage = damage;
			message.attackTime = attacktime;
			if (target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
			{
				message.IsDead = true;
				Log.Info($"{target.Id} is dead");
				CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				cpCombatComponent.target = null;
			}
			else
			{
				message.IsDead = false;
			}
			
			gamePlayComponent.Broadcast(message);
		}
	}
}
