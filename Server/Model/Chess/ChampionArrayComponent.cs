using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof (GamePlayComponent))]
    public class ChampionArrayComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 就绪
        /// </summary>
        public Dictionary<long, List<ChampionInfo>> playersInventoryArr;

    }
}