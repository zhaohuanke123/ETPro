using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ET
{
    [ObjectSystem]
    public class ShopComponentAwakeSystem: AwakeSystem<ShopComponent>
    {
        public override void Awake(ShopComponent self)
        {
            self.availableChampionIdArray = new Dictionary<long, List<int>>();
            self.playersGoldDict = new Dictionary<long, int>();
            self.playerCurrentChampionLimit = new Dictionary<long, int>();
            self.championIdsArray = ChampionConfigCategory.Instance.GetAll().Keys.ToArray();
            self.championTypeIdsArray = ChampionTypeConfigCategory.Instance.GetAll().Keys.ToArray();
        }
    }

    [ObjectSystem]
    public class ShopComponentDestroySystem: DestroySystem<ShopComponent>
    {
        public override void Destroy(ShopComponent self)
        {
        }
    }

    [FriendClass(typeof (ShopComponent))]
    public static partial class ShopComponentSystem
    {
        // public static void Test(this ShopComponent self)
        // {
        // }
        public static void AddPlayer(this ShopComponent self, Player player)
        {
            if (!self.availableChampionIdArray.TryAdd(player.Id, new List<int>()))
            {
                throw new ArgumentException("玩家已存在");
            }

            if (!self.playersGoldDict.TryAdd(player.Id, GamePlayComponent.InitGold))
            {
            }

            if (!self.playerCurrentChampionLimit.TryAdd(player.Id, GamePlayComponent.InitChampionLimit))
            {
            }
        }

        public static void AddPlayerGold(this ShopComponent self, Player player, int gold)
        {
            if (!self.playersGoldDict.TryGetValue(player.Id, out int count))
            {
                throw new ArgumentException("玩家不存在");
            }

            int res = count + gold;
            self.playersGoldDict[player.Id] = res;

            self.SendRefreshGold(player, count + res);
        }

        public static void SubPlayerGold(this ShopComponent self, Player player, int gold)
        {
            if (self.playersGoldDict.TryGetValue(player.Id, out int count))
            {
                if (count >= gold)
                {
                    self.playersGoldDict[player.Id] -= gold;
                    self.SendRefreshGold(player, count - gold);
                }
                else
                {
                    throw new InvalidOperationException("金币不足");
                }
            }
        }

        public static bool TrySubPlayerGold(this ShopComponent self, Player player, int goldCount)
        {
            if (self.playersGoldDict.TryGetValue(player.Id, out int count))
            {
                if (count >= goldCount)
                {
                    self.playersGoldDict[player.Id] -= goldCount;
                    self.SendRefreshGold(player, count - goldCount);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsEnoughGold(this ShopComponent self, long playerId, int gold)
        {
            if (!self.playersGoldDict.TryGetValue(playerId, out int value))
            {
                return false;
            }

            return value >= gold;
        }

        /// <summary>
        /// 测试，获取随机英雄
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetRandomChampionId(this ShopComponent self)
        {
            int rand = RandomHelper.RandomNumber(0, self.championIdsArray.Length);

            return self.championIdsArray[rand];
        }

        /// <summary>
        /// 刷新商店
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="isFree"></param>
        public static bool RefreshShop(this ShopComponent self, Player player, bool isFree = false)
        {
            if (self.playersGoldDict.TryGetValue(player.Id, out int gold))
            {
                if (gold < 2 && isFree == false)
                {
                    return false;
                }
            }

            if (self.availableChampionIdArray.ContainsKey(player.Id))
            {
                self.availableChampionIdArray.Remove(player.Id);
            }

            // TODO 临时
            const int count = 5;
            var availableChampionArray = new List<int>(count);
            self.availableChampionIdArray.Add(player.Id, availableChampionArray);

            for (int i = 0; i < count; i++)
            {
                int randomChampionId = self.GetRandomChampionId();

                availableChampionArray.Add(randomChampionId);
            }

            if (isFree == false)
            {
                self.SubPlayerGold(player, 2);
            }

            return true;
        }

        public static List<int> GetAvailableChampionIdArray(this ShopComponent self, long id)
        {
            if (!self.availableChampionIdArray.TryGetValue(id, out List<int> value))
            {
                throw new ArgumentException("玩家不存在");
            }

            return value;
        }

        public static void SendRefreshGold(this ShopComponent self, Player player)
        {
            if (self.playersGoldDict.TryGetValue(player.Id, out int gold))
            {
                self.SendRefreshGold(player, gold);
            }
            else
            {
                throw new ArgumentException("玩家不存在");
            }
        }

        public static void SendRefreshGold(this ShopComponent self, Player player, int goldCount)
        {
            player.Session.Send(new G2C_RefreshGold() { GlodCount = goldCount });
        }

        public static int GetIdByIndex(this ShopComponent self, Player player, int index)
        {
            if (!self.availableChampionIdArray.TryGetValue(player.Id, out var availableList))
            {
                return -1;
            }

            if (index >= availableList.Count)
            {
                return -1;
            }

            return availableList[index];
        }
    }
}