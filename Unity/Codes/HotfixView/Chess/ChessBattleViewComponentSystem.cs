namespace ET
{
    [ObjectSystem]
    public class ChessBattleViewComponentAwakeSystem: AwakeSystem<ChessBattleViewComponent>
    {
        public override void Awake(ChessBattleViewComponent self)
        {
            ChessBattleViewComponent.Instance = self;
            self.ownChampionInventoryArray = new GameObjectComponent[9];
            self.gridChampionsArray = new GameObjectComponent[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        }
    }

    [ObjectSystem]
    public class ChessBattleViewComponentDestroySystem: DestroySystem<ChessBattleViewComponent>
    {
        public override void Destroy(ChessBattleViewComponent self)
        {
        }
    }

    [FriendClass(typeof (ChessBattleViewComponent))]
    public static partial class ChessBattleViewComponentSystem
    {
        // public static void Test(this ChessBattleViewComponent self)
        // {
        // }

        public static void Replace(this ChessBattleViewComponent self, GameObjectComponent showView, int index)
        {
            ref GameObjectComponent gameObjectComponent = ref self.ownChampionInventoryArray[index];
            if (gameObjectComponent != null)
            {
                gameObjectComponent.Dispose();
            }

            gameObjectComponent = showView;
        }

        public static void Clear(this ChessBattleViewComponent self)
        {
            foreach (GameObjectComponent goComponent in self.ownChampionInventoryArray)
            {
                if (goComponent != null)
                {
                    goComponent.Dispose();
                }
            }
        }
    }
}