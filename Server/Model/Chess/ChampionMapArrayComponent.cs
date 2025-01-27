using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class ChampionMapArrayComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 地图中的
        /// </summary>
        public ChampionInfo[,] grid;
    }
}