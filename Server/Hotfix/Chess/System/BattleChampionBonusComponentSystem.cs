using System.Collections.Generic;
using System.Reflection;

namespace ET
{
    [ObjectSystem]
    public class BattleChampionBonusComponentAwakeSystem: AwakeSystem<BattleChampionBonusComponent>
    {
        public override void Awake(BattleChampionBonusComponent self)
        {
            self.playersChampionTypeCount = new Dictionary<long, Dictionary<int, int>>();
            self.playersActiveBonus = new Dictionary<long, List<ChampionBonusConfig>>();
        }
    }

    [ObjectSystem]
    public class BattleChampionBonusComponentDestroySystem: DestroySystem<BattleChampionBonusComponent>
    {
        public override void Destroy(BattleChampionBonusComponent self)
        {
        }
    }

    [FriendClass(typeof (BattleChampionBonusComponent))]
    public static partial class BattleChampionBonusComponentSystem
    {
        public static (Dictionary<int, int> typeCount, List<ChampionBonusConfig> activeBonus)
                Init(this BattleChampionBonusComponent self, Player player)
        {
            var typeCount = new Dictionary<int, int>();
            var activeBonus = new List<ChampionBonusConfig>();

            self.playersActiveBonus[player.Id] = activeBonus;
            self.playersChampionTypeCount[player.Id] = typeCount;

            return (typeCount, activeBonus);
        }

        public static Dictionary<int, int> GetPlayerChampionTypeCount(this BattleChampionBonusComponent self, Player player)
        {
            if (!self.playersChampionTypeCount.TryGetValue(player.Id, out var typeCount))
            {
                var tmp = self.Init(player);
                typeCount = tmp.typeCount;
            }

            return typeCount;
        }

        public static List<ChampionBonusConfig> GetPlayerActiveBonus(this BattleChampionBonusComponent self, Player player)
        {
            if (!self.playersActiveBonus.TryGetValue(player.Id, out var activeBonus))
            {
                var tmp = self.Init(player);
                activeBonus = tmp.activeBonus;
            }

            return activeBonus;
        }
    }
}