using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class ChampionArrayComponentAwakeSystem: AwakeSystem<ChampionArrayComponent>
    {
        public override void Awake(ChampionArrayComponent self)
        {
            self.playersInventoryArr = new Dictionary<long, List<ChampionInfo>>();
        }
    }

    [ObjectSystem]
    public class ChampionArrayComponentDestroySystem: DestroySystem<ChampionArrayComponent>
    {
        public override void Destroy(ChampionArrayComponent self)
        {
        }
    }

    [FriendClass(typeof (ChampionArrayComponent))]
    [FriendClassAttribute(typeof (ET.ChampionInfo))]
    public static partial class ChampionArrayComponentSystem
    {
        private const int InventorySize = 9;

        public static List<ChampionInfo> InitPlayerArray(this ChampionArrayComponent self, long playerId)
        {
            var list = new List<ChampionInfo>(InventorySize);
            list.AddMany(null, InventorySize);
            self.playersInventoryArr[playerId] = list;
            return list;
        }

        public static int FindEmptyIndex(this ChampionArrayComponent self, Player player)
        {
            if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
            {
                list = self.InitPlayerArray(player.Id);
            }

            int emptyIndex = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    emptyIndex = i;
                    break;
                }
            }

            return emptyIndex;
        }

        public static bool TryAdd(this ChampionArrayComponent self, Player player, ChampionInfo championInfo)
        {
            if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
            {
                list = self.InitPlayerArray(player.Id);
            }

            int index = self.FindEmptyIndex(player);
            if (index == -1)
            {
                return false;
            }

            championInfo.gridType = GridType.OwnInventory;
            championInfo.gridPositionX = index;
            list[index] = championInfo;
            return true;
        }
    }
}