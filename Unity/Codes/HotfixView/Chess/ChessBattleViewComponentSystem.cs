using System.Xml.Schema;

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

        public static void StartDrag(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
        {
            if (triggerInfo != null)
            {
                self.dragStartTrigger = triggerInfo;

                GameObjectComponent championGO = self.GetChampionFromTriggerInfo(triggerInfo);

                if (championGO != null)
                {
                    Map.Instance.ShowIndicators();

                    self.draggedChampion = championGO;

                    ChampionControllerComponent championControllerComponent = championGO.GetComponent<ChampionControllerComponent>();
                    championControllerComponent.SetDrag(true);
                }
            }
        }

        public static void EndDrag(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
        {
            Map.Instance.HideIndicators();

            if (self.draggedChampion != null)
            {
                self.draggedChampion.GetComponent<ChampionControllerComponent>().SetDrag(false);

                if (triggerInfo != null)
                {
                    GameObjectComponent currentTriggerChampion = self.GetChampionFromTriggerInfo(triggerInfo);

                    if (currentTriggerChampion != null)
                    {
                        self.StoreChampionInArray(self.dragStartTrigger.gridType, self.dragStartTrigger.gridX, self.dragStartTrigger.gridZ,
                            currentTriggerChampion);

                        self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);
                    }
                    else
                    {
                        if (triggerInfo.gridType == Map.GRIDTYPE_HEXA_MAP)
                        {
                            // if (championsOnField < currentChampionLimit || dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            if (true || self.dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            {
                                self.RemoveChampionFromArray(self.dragStartTrigger.gridType, self.dragStartTrigger.gridX,
                                    self.dragStartTrigger.gridZ);

                                self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);

                                // if (dragStartTrigger.gridType != Map.GRIDTYPE_HEXA_MAP)
                                //     championsOnField++;
                            }
                        }
                        else if (triggerInfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
                        {
                            self.RemoveChampionFromArray(self.dragStartTrigger.gridType, self.dragStartTrigger.gridX, self.dragStartTrigger.gridZ);

                            self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);

                            // if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            //     championsOnField--;
                        }
                    }
                }
            }

            self.draggedChampion = null;
        }

        public static GameObjectComponent GetChampionFromTriggerInfo(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
        {
            if (triggerInfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
            {
                return self.ownChampionInventoryArray[triggerInfo.gridX];
            }

            if (triggerInfo.gridType == Map.GRIDTYPE_HEXA_MAP)
            {
                return self.gridChampionsArray[triggerInfo.gridX, triggerInfo.gridZ];
            }

            Log.Error($"GetChampionFromTriggerInfo error : {triggerInfo}");
            return null;
        }

        public static void StoreChampionInArray(this ChessBattleViewComponent self, int gridType, int gridX, int gridZ, GameObjectComponent champion)
        {
            var championController = champion.GetComponent<ChampionControllerComponent>();
            championController.SetGridPosition(gridType, gridX, gridZ);
            Log.Warning($"StoreChampionInArray : {gridType} {gridX} {gridZ}");

            if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
            {
                self.ownChampionInventoryArray[gridX] = champion;
            }
            else if (gridType == Map.GRIDTYPE_HEXA_MAP)
            {
                self.gridChampionsArray[gridX, gridZ] = champion;
            }
        }

        public static void RemoveChampionFromArray(this ChessBattleViewComponent self, int type, int gridX, int gridZ)
        {
            if (type == Map.GRIDTYPE_OWN_INVENTORY)
            {
                self.ownChampionInventoryArray[gridX] = null;
            }
            else if (type == Map.GRIDTYPE_HEXA_MAP)
            {
                self.gridChampionsArray[gridX, gridZ] = null;
            }
        }
    }
}