using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class ShopComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// MyId to GoldCount
        /// Key : MyId
        /// Value : GoldCount
        /// </summary>
        public Dictionary<long, int> playersGoldDict;

        /// <summary>
        /// MyId to [] 
        /// </summary>
        public Dictionary<long, List<int>> availableChampionIdArray;

        /// <summary>
        ///  所有的英雄
        /// </summary>
        public int[] championIdsArray;

        /// <summary>
        /// 羁绊配置
        /// </summary>
        public int[] championTypeIdsArray;
    }
}