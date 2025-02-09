using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [FriendClass(typeof (HeroComponent))]
    public static class HeroComponentSystem
    {
        public static void Awake(this HeroComponent self)
        {
            self.Heroes.Clear();
        }

        // 添加英雄
        public static async ETTask<bool> AddHero(this HeroComponent self, int configId)
        {
            if (self.Heroes.ContainsKey(configId))
            {
                return false;
            }

            // Hero hero = self.AddChild<Hero, int>(configId);
            self.Heroes.Add(configId, configId);

            // 保存到数据库
            DBComponent dbComponent = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            await dbComponent.Save(self);

            return true;
        }

        // 检查是否拥有英雄
        public static bool HasHero(this HeroComponent self, int configId)
        {
            return self.Heroes.ContainsKey(configId);
        }

        // 获取所有英雄
        public static List<int> GetAllHeroIds(this HeroComponent self)
        {
            return self.Heroes.Keys.ToList();
        }

        // public static List<Hero> GetAllHeroes(this HeroComponent self)
        // {
        //     return self.Heroes.Values.ToList();
        // }
    }
}