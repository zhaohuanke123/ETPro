namespace ET
{
    public class UIBonus: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBonus.prefab";
        public UIImage icon;
        public UIText name;
        public UIText count;
    }
}