using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIBonus))]
    public class UIBonusOnCreateSystem: OnCreateSystem<UIBonus, int>
    {
        public override void OnCreate(UIBonus self, int index)
        {
            self.index = index;
            self.icon = self.AddUIComponent<UIImage>("icon");
            self.name = self.AddUIComponent<UIText>("name");
            self.count = self.AddUIComponent<UIText>("count");
            self.activeBg = self.AddUIComponent<UIImage>("ActiveBG");
            self.SetActive(false);
        }
    }

    [ObjectSystem]
    [FriendClass(typeof (UIBonus))]
    public class UIBonusLoadSystem: LoadSystem<UIBonus>
    {
        public override void Load(UIBonus self)
        {
        }
    }

    [FriendClass(typeof (UIBonus))]
    public static class UIBonusSystem
    {
        public static void SetBonus(this UIBonus self, int bonusId, int count)
        {
            if (bonusId != -1)
            {
                ChampionTypeConfig championTypeConfig = ChampionTypeConfigCategory.Instance.Get(bonusId);
                ChampionBonusConfig championBonusConfig = ChampionBonusConfigCategory.Instance.Get(championTypeConfig.championBonusId);

                self.SetActive(true);
                self.icon.SetSpritePath(championTypeConfig.icon).Coroutine();
                self.name.SetText(championTypeConfig.displayName);

                int curCount = Math.Min(count, championBonusConfig.championCount);
                self.count.SetText($"{curCount}/{championBonusConfig.championCount}");
                self.activeBg.SetActive(curCount >= championBonusConfig.championCount);
            }
            else
            {
                self.SetActive(false);
            }
        }
    }
}