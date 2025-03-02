namespace ET
{
    [FriendClass(typeof (CpCombatComponent))]
    [FriendClass(typeof (GamePlayComponent))]
    [FriendClass(typeof (Player))]
    public static class DamageHelper
    {
        public static int Damage(Unit attacker, Unit target, out bool isCritical)
        {
            int finalDamage = CalculateFinalDamage(attacker, out isCritical);

            return finalDamage;
        }

        private static int CalculateFinalDamage(Unit attacker, out bool isCritical)
        {
            NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();
            float CRI = numericComponent.GetAsFloat(NumericType.CRI);
            float CRIDamage = numericComponent.GetAsFloat(NumericType.CRIDamage);

            int damage = numericComponent.GetAsInt(NumericType.ATK);
            isCritical = IsCRT(CRI);
            if (isCritical)
            {
                damage = (int)((1 + 1.0f * CRIDamage / 100) * damage);
                Log.Info("IsCritical ed ||  damage : " + damage);
            }

            return damage;
        }

        public static bool IsCRT(float CRI)
        {
            return RandomHelper.RandomNumber(1, 100) < CRI;
        }
    }
}