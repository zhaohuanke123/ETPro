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
    public class UIBonusOnCreateSystem: OnCreateSystem<UIBonus>
    {
        public override void OnCreate(UIBonus self)
        {
            self.icon = self.AddUIComponent<UIImage>("icon");
            self.name = self.AddUIComponent<UIText>("name");
            self.count = self.AddUIComponent<UIText>("count");
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
    }
}