using System.Collections.Generic;

namespace ET
{
    public enum CpBuffType
    {
        Self,
        Other,
    }
    [ComponentOf(typeof(Unit))]
    public class CpBuffComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, CpBuff> CpBuffMap;
    }
}