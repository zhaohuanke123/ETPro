namespace ET
{
    [FriendClass(typeof (CpCombatComponent))]
    [FriendClass(typeof (GamePlayComponent))]
    [FriendClass(typeof (Player))]
    public static class DamageHelper
    {
        public static int Damage(Unit attacker,
         Unit target, int damageType, out bool isCritical)
        {
            NumericComponent numericComponent = target.GetComponent<NumericComponent>();
            int def = 1;
            if (damageType == 1)
                def = numericComponent.GetAsInt(NumericType.NorDef);
            else
                def = numericComponent.GetAsInt(NumericType.MagDef);
            return CalculateFinalDamage(attacker, target, def, out isCritical);
        }

        public static int DamageHeal(Unit attacker, Unit target)
        {
            NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();
            int atk = numericComponent.GetAsInt(NumericType.ATK);
            return atk;
        }

        private static int CalculateFinalDamage(Unit attacker,
         Unit target, int def, out bool isCritical)
        {
            NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();
            float CRI = numericComponent.GetAsFloat(NumericType.CRI);
            float CRIDamage = numericComponent.GetAsFloat(NumericType.CRIDamage);

            int atk = numericComponent.GetAsInt(NumericType.ATK);
            int damage = (int)(atk * (1 - 1.0f * (def) / (def + 100)));
            isCritical = IsCRT(CRI);
            if (isCritical)
                damage = (int)((1 + 1.0f * CRIDamage / 100) * damage);

            return damage;
        }

        public static bool IsCRT(float CRI)
        {
            return RandomHelper.RandomNumber(1, 100) < CRI;
        }
    }
}