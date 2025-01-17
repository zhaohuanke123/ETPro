namespace ET
{
    [UIComponent]
    public class UIBattle: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBattle.prefab";
        public UIText GoldText;
        public UIText CountDownText;
        public UIText HpText;
        public UICostIN allCoin;
        public UIButton returnBtn;
        public UIText championLimitText;
        public UIChampionContainer[] cContainers;
        public UIButton refreshShopButton;
    }
}