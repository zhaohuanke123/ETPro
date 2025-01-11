namespace ET
{
    public class UIBattle: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBattle.prefab";
        public UIText GoldText;
        public UIText CountDownText;
        public UIText HpText;
    }
}