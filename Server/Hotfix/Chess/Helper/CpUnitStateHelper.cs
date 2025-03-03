using System;

namespace ET
{
    public static class CpUnitStateHelper
    {
        public static bool CanAction(this Unit self)
        {
            CpBuffComponent cpBuffComponent = self.GetComponent<CpBuffComponent>();
            if (cpBuffComponent == null)
            {
                throw new ArgumentException($"错误的 Unit {self.Id}");
            }

            return cpBuffComponent.HasBuff(1006) || cpBuffComponent.HasBuff(1007);
        }
    }
}