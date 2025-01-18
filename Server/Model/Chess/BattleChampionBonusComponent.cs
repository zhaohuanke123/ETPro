using System.Collections.Generic;

namespace ET
{
    [ComponentOf()]
    public class BattleChampionBonusComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<long, Dictionary<int, int>> playersChampionTypeCount;

        public Dictionary<long, List<ChampionBonusConfig>> playersActiveBonus;
    }
}