using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum GridType
    {
        OwnInventory = 0,
        ChessMap = 1,
        // Oponent_Inventory,
    }

    [ChildOf]
    public class ChampionInfo: Entity, IAwake, IDestroy
    {
        [BsonIgnore]
        public ChampionConfig config => ChampionConfigCategory.Instance.Get(this.configId);

        public int configId;

        // public int lv;
        // public float currentHealth = 0;

        public int gridPositionX = 0;
        public int gridPositionZ = 0;
        public GridType gridType;
        
        public int X => gridPositionX;
        public int Z => gridPositionZ;
    }
}