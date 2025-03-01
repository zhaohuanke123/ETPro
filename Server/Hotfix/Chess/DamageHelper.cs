namespace ET
{
	[FriendClass(typeof (CpCombatComponent))]
	[FriendClass(typeof (GamePlayComponent))]
	[FriendClass(typeof (Player))]
	public static class DamageHelper
	{
		public static int Damage(Unit attacker, Unit target)
		{
			int finalDamage = CalculateFinalDamage(attacker);

			return finalDamage;
		}

		private static int CalculateFinalDamage(Unit attacker)
		{
			NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();

			return numericComponent.GetAsInt(NumericType.ATK);
		}
	}
}
