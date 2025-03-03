using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class CpBuffComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, CpBuff> CpBuffMap;
    }
}