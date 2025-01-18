namespace ET
{
    [ComponentOf]
    public class ChessBattleViewComponent: Entity, IAwake, IDestroy
    {
        public static ChessBattleViewComponent Instance { get; set; }
        public GameObjectComponent[] ownChampionInventoryArray;
        public GameObjectComponent[,] gridChampionsArray;
    }
}