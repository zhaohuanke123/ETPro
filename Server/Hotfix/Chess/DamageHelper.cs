using MongoDB.Driver.Core.Events;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpCombatComponent))]
	[FriendClassAttribute(typeof (ET.GamePlayComponent))]
	[FriendClassAttribute(typeof (ET.Player))]
	public static class DamageHelper
	{
		public static int Damage(Unit attacker, Unit target)
		{
			int finalDamage = CalculateFinalDamage(attacker);
			target.GetComponent<NumericComponent>()
					.Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - finalDamage);

			Log.Info($"attacker: {attacker.Id}, target: {target.Id}, damage: {finalDamage}");

			return finalDamage;
		}

		private static int CalculateFinalDamage(Unit attacker)
		{
			NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();

			return numericComponent.GetAsInt(NumericType.ATK);
		}
	}
}
