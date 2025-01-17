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
            self.GoldText = self.AddUIComponent<UIText>("Gold/CostGo/cost");
            self.CountDownText = self.AddUIComponent<UIText>("Placement/Timer/Text");
            self.allCoin = self.AddUIComponent<UICostIN>("Gold/CostGo");
            self.championLimitText = self.AddUIComponent<UIText>("championLimit/Text");
            self.HpText = self.AddUIComponent<UIText>("Hp/Text");

            self.refreshShopButton = self.AddUIComponent<UIButton>("Shop/left menu/refresh");
            self.refreshShopButton.SetOnClickAsync(self.OnRefreshShopBtnClick);

            self.returnBtn = self.AddUIComponent<UIButton>("ReturnBtn");
            self.returnBtn.SetOnClickAsync(self.OnReturnBtnClick);

            const int count = 5;
            self.cContainers = new UIChampionContainer[count];
            for (int i = 0; i < self.cContainers.Length; i++)
            {
                self.cContainers[i] = self.AddUIComponent<UIChampionContainer, int>($"Shop/Layout/CC{i}", i);
            }
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
            for (int i = 0; i < self.cContainers.Length; i++)
            {
                self.cContainers[i].SetChampion(championIds[i]);
            }
        }

        public static void SetChampionLimit(this UIBattle self, int championLimit)
        {
            self.championLimitText.SetText(championLimit.ToString());
        }

        public static void SetHp(this UIBattle self, int hp)
        {
            self.HpText.SetText(hp.ToString());
        }

        public static async ETTask OnRefreshShopBtnClick(this UIBattle self)
        {
            await ChessBattleHelper.RefreshShopChampionList(self.ZoneScene());
        }
    }
}