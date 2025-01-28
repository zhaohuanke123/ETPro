using UnityEngine;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIChampionContainer))]
    public class UIChampionContainerCreateSystem: OnCreateSystem<UIChampionContainer, int>
    {
        public override void OnCreate(UIChampionContainer self, int index)
        {
            self.championBtn = self.AddUIComponent<UIButton>("champion");

            self.Sk1 = self.AddUIComponent<UIIconName>("champion/top/SK1");
            self.Sk2 = self.AddUIComponent<UIIconName>("champion/top/SK2");

            self.cost = self.AddUIComponent<UICostIN>("champion/bottom/CostGo");

            self.championBtn.SetOnClickAsync(self.OnChampionBtnClick);
            self.index = index;
        }
    }

    [FriendClass(typeof (UIChampionContainer))]
    public static class UIChampionContainerSystem
    {
        public static void SetChampion(this UIChampionContainer self, int id)
        {
            self.id = id;
            ChampionConfig config = ChampionConfigCategory.Instance.Get(id);
            int type1Id = config.type1Id;
            int type2Id = config.type2Id;

            ChampionTypeConfig type1Config = ChampionTypeConfigCategory.Instance.Get(type1Id);
            ChampionTypeConfig type2Config = ChampionTypeConfigCategory.Instance.Get(type2Id);

            self.Sk1.SetIconAndName(type1Config.icon, type1Config.displayName);
            self.Sk2.SetIconAndName(type2Config.icon, type2Config.displayName);
            self.cost.SetNumber(config.cost);
        }

        public static async ETTask OnChampionBtnClick(this UIChampionContainer self)
        {
            await ChessBattleHelper.TryBuyChampion(self.ZoneScene(),self.index);
        }
    }
}