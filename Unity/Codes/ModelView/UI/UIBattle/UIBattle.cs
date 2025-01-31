using System.Collections.Generic;

namespace ET
{
    [UIComponent]
    public class UIBattle: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBattle.prefab";
        public UIText GoldText;
        public UITextmesh UITimerText;
        // public UICostIN allCoin;
        public UIText HpText;
        public UISlider HpSlider;
        public UIButton returnBtn;
        public UIText championLimitText;
        public UIChampionContainer[] cContainers;

        public UIBonus[] bonusList;
        public UIButton refreshShopButton;
    }
}