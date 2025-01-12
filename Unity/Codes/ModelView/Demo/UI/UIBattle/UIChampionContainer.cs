namespace ET
{
    [UIComponent]
    public class UIChampionContainer: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIChampionContainer.prefab";
        public UIButton championBtn;
        public UIIconName Sk1;
        public UIIconName Sk2;
        public UICostIN cost;
    }
}