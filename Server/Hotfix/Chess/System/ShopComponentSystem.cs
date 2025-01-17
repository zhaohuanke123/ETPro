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
            self.championIdsArray = ChampionConfigCategory.Instance.GetAll().Keys.ToArray();
            self.playersGoldDict = new Dictionary<long, int>();
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
        public static void AddPlayerGold(this ShopComponent self, long playerId, int gold)
        {
            if (!self.playersGoldDict.TryAdd(playerId, gold))
            {
                self.playersGoldDict[playerId] += gold;
            }
        }

        public static void SubPlayerGold(this ShopComponent self, long playerId, int gold)
        {
            if (self.playersGoldDict.ContainsKey(playerId))
            {
                if (self.playersGoldDict[playerId] >= gold)
                    self.playersGoldDict[playerId] -= gold;
                else
                    throw new InvalidOperationException("金币不足");
            }
        }

        public static bool IsEnoughGold(this ShopComponent self, int playerId, int gold)
        {
            if (self.playersGoldDict.TryGetValue(playerId, out int value))
            {
                return value >= gold;
            }

            return false;
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
        /// <param name="id"></param>
        /// <param name="isFree"></param>
        public static void RefreshShop(this ShopComponent self, long id, bool isFree)
        {
            if (self.playersGoldDict.TryGetValue(id, out int gold))
            {
                if (gold < 2 && isFree == false)
                    return;
            }

            if (self.availableChampionIdArray.ContainsKey(id))
            {
                self.availableChampionIdArray.Remove(id);
            }

            // TODO 临时
            const int count = 5;
            var availableChampionArray = new List<int>(count);
            self.availableChampionIdArray.Add(id, availableChampionArray);

            for (int i = 0; i < count; i++)
            {
                int randomChampionId = self.GetRandomChampionId();

                availableChampionArray.Add(randomChampionId);
            }

            if (isFree == false)
            {
                self.SubPlayerGold(id, 2);
            }
        }

        public static List<int> GetAvailableChampionIdArray(this ShopComponent self, long id)
        {
            if (!self.availableChampionIdArray.TryGetValue(id, out List<int> value))
            {
                throw new ArgumentException("玩家不存在");
            }

            return value;
        }
    }
}