namespace ET
{
    [ComponentOf]
    public class ChessBattleViewComponent: Entity, IAwake, IDestroy
    {
        public static ChessBattleViewComponent Instance { get; set; }
        public GameObjectComponent[] ownChampionInventoryArray;
        public GameObjectComponent[,] gridChampionsArray;

        public GameObjectComponent draggedChampion = null;
        public TriggerInfo dragStartTrigger = null;
        
        // 添加地图上英雄数量的计数器
        public int championsOnMapCount = 0;

        public int ChampionsOnMapCount => championsOnMapCount;
    }
}