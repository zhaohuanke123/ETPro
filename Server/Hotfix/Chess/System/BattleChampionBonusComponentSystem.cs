using System;
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
        public static void AddPlayer(this BattleChampionBonusComponent self, Player player)
        {
            if (!self.playersChampionTypeCount.TryAdd(player.Id, new Dictionary<int, int>()))
            {
                Log.Error("BattleChampionBonusComponent AddPlayer 玩家已经存在");
            }

            if (!self.playersActiveBonus.TryAdd(player.Id, new List<ChampionBonusConfig>()))
            {
                Log.Error("BattleChampionBonusComponent AddPlayer 玩家已经存在");
            }
        }

        // public static void Init(this BattleChampionBonusComponent self, Player player)
        // {
        //     var typeCount = new Dictionary<int, int>();
        //     var activeBonus = new List<ChampionBonusConfig>();
        //
        //     self.playersActiveBonus[player.Id] = activeBonus;
        //     self.playersChampionTypeCount[player.Id] = typeCount;
        // }

        public static Dictionary<int, int> GetPlayerChampionTypeCount(this BattleChampionBonusComponent self, Player player)
        {
            if (!self.playersChampionTypeCount.TryGetValue(player.Id, out var typeCount))
            {
                throw new ArgumentException("玩家不存在");
            }

            return typeCount;
        }

        public static List<ChampionBonusConfig> GetPlayerActiveBonus(this BattleChampionBonusComponent self, Player player)
        {
            if (!self.playersActiveBonus.TryGetValue(player.Id, out var activeBonus))
            {
                throw new ArgumentException("玩家不存在");
            }

            return activeBonus;
        }
    }
}