using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [ObjectSystem]
    public class ChampionMapArrayComponentAwakeSystem: AwakeSystem<ChampionMapArrayComponent>
    {
        public override void Awake(ChampionMapArrayComponent self)
        {
            self.playersGridArray = new Dictionary<long, ChampionInfo[,]>();
        }
    }

    [ObjectSystem]
    public class ChampionMapArrayComponentDestroySystem: DestroySystem<ChampionMapArrayComponent>
    {
        public override void Destroy(ChampionMapArrayComponent self)
        {
        }
    }

    [FriendClass(typeof (ChampionMapArrayComponent))]
    public static partial class ChampionMapArrayComponentSystem
    {
        public const int hexMapSizeX = 7;
        public const int hexMapSizeZ = 8;

        public static ChampionInfo[,] Init(this ChampionMapArrayComponent self, long playerId)
        {
            ChampionInfo[,] grids = new ChampionInfo[hexMapSizeX, hexMapSizeZ];
            self.playersGridArray[playerId] = grids;
            return grids;
        }

        public static void AddToGrid(this ChampionMapArrayComponent self, Player player, ChampionInfo championInfo, int gridX, int gridZ)
        {
            if (!self.playersGridArray.TryGetValue(player.Id, out var grids))
            {
                grids = self.Init(player.Id);
            }

            grids[gridX, gridZ] = championInfo;
        }

        public static void RemoveFromGird(this ChampionMapArrayComponent self, Player player, int gridX, int gridZ)
        {
            if (!self.playersGridArray.TryGetValue(player.Id, out var grids))
            {
                grids = self.Init(player.Id);
            }

            grids[gridX, gridZ] = null;
        }

        public static void CalculateBonuses(this ChampionMapArrayComponent self, Player player)
        {
            if (!self.playersGridArray.TryGetValue(player.Id, out var grids))
            {
                return;
            }

            BattleChampionBonusComponent battleChampionBonusComponent = self.GetComponent<BattleChampionBonusComponent>();

            Dictionary<int, int> championTypeCount = battleChampionBonusComponent.GetPlayerChampionTypeCount(player);
            championTypeCount.Clear();
            for (int x = 0; x < hexMapSizeX; x++)
            {
                for (int z = 0; z < hexMapSizeZ / 2; z++)
                {
                    if (grids[x, z] != null)
                    {
                        ChampionInfo championInfo = grids[x, z];
                        int id1 = championInfo.config.type1Id;
                        int id2 = championInfo.config.type2Id;

                        if (!championTypeCount.TryAdd(id1, 1))
                        {
                            int cCount = 0;
                            championTypeCount.TryGetValue(id1, out cCount);

                            cCount++;

                            championTypeCount[id1] = cCount;
                        }

                        if (!championTypeCount.TryAdd(id2, 1))
                        {
                            int cCount = 0;
                            championTypeCount.TryGetValue(id2, out cCount);

                            cCount++;

                            championTypeCount[id2] = cCount;
                        }
                    }
                }
            }

            var activeBonusList = battleChampionBonusComponent.GetPlayerActiveBonus(player);
            activeBonusList.Clear();

            foreach (KeyValuePair<int, int> m in championTypeCount)
            {
                int id = m.Key;
                ChampionBonusConfig championBonusConfig = ChampionBonusConfigCategory.Instance.Get(id);
                if (m.Value >= championBonusConfig.championCount)
                {
                    activeBonusList.Add(championBonusConfig);
                }
            }

            G2C_UpdateBonus message = new G2C_UpdateBonus();
            message.TypeIdList = championTypeCount.Keys.ToList();
            message.CountList = championTypeCount.Values.ToList();
            player.Session.Send(message);
        }
    }
}