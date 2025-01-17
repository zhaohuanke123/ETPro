using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIBattle))]
    public class UIBattleOnCreateSystem: OnCreateSystem<UIBattle>
    {
        public override void OnCreate(UIBattle self)
        {
            self.GoldText = self.AddUIComponent<UIText>("/Gold/Text");
            self.CountDownText = self.AddUIComponent<UIText>("Placement/Timer/Text");

            // ReturnBtn
            self.returnBtn = self.AddUIComponent<UIButton>("ReturnBtn");
            self.returnBtn.SetOnClickAsync(self.OnReturnBtnClick);

            //Gold/CostGo
            self.allCoin = self.AddUIComponent<UICostIN>("Gold/CostGo");
            self.allCoin.SetNumber(123123);

            // championLimit/Text
            self.championLimitText = self.AddUIComponent<UIText>("championLimit/Text");
            //Hp/Text  
            self.HpText = self.AddUIComponent<UIText>("Hp/Text");

            // Shop/layout/champion container_0
            self.championContainer = self.AddUIComponent<UIChampionContainer>("Shop/Layout/CC0");
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
        public static async ETTask OnReturnBtnClick(this UIBattle self)
        {
            await SceneManagerComponent.Instance.SwitchScene(SceneNames.Login);
            await UIManagerComponent.Instance.DestroyWindow<UIBattle>();
            await UIManagerComponent.Instance.OpenWindow<UILobbyView>(UILobbyView.PrefabPath);
            await UIManagerComponent.Instance.CloseWindow<UILoadingView>();
        }

        public static void SetGold(this UIBattle self, int gold)
        {
            self.GoldText.SetText(gold.ToString());
        }

        public static void SetShopChampionList(this UIBattle self, List<int> championIds)
        {
            self.championContainer.SetChampion(championIds[0]);
        }
    }
}