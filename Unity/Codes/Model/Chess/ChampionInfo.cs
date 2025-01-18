namespace ET
{
    public enum GridType
    {
        OwnInventory = 0,

        // Oponent_Inventory,
        ChessMap = 1,
    }

    [ChildOf]
    public class ChampionInfo: Entity, IAwake, IDestroy
    {
        public ChampionConfig config => ChampionConfigCategory.Instance.Get(this.configId);

        public int configId;

        // public int lv;
        // public float currentHealth = 0;

        public int gridPositionX = 0;
        public int gridPositionZ = 0;
        public GridType gridType;
    }
}