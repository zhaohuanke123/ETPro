using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIBattle))]
    public class UIBattleOnCreateSystem: OnCreateSystem<UIBattle>
    {
        public override void OnCreate(UIBattle self)
        {
            self.GoldText = self.AddUIComponent<UIText>("Gold/Text");
            self.CountDownText = self.AddUIComponent<UIText>("Placement/Timer/Text");
            self.HpText = self.AddUIComponent<UIText>("Hp/Text");
        }
    }

    [ObjectSystem]
    [FriendClass(typeof (UIBattle))]
    public class UIBattleLoadSystem: LoadSystem<UIBattle>
    {
        public override void Load(UIBattle self)
        {
        }
    }

    [FriendClass(typeof (UIBattle))]
    public static class UIBattleSystem
    {
    }
}