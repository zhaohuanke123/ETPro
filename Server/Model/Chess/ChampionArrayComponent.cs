using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof (GamePlayComponent))]
    public class ChampionArrayComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<long, List<ChampionInfo>> playersInventoryArr;
    }
}