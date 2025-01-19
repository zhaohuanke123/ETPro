using ET.Chess;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ChampionControllerComponentAwakeSystem: AwakeSystem<ChampionControllerComponent, GameObjectComponent>
    {
        public override void Awake(ChampionControllerComponent self, GameObjectComponent gameObjectComponent)
        {
            self.championController = gameObjectComponent.GameObject.GetComponent<ChampionController>();
        }
    }

    [ObjectSystem]
    public class ChampionControllerComponentDestroySystem: DestroySystem<ChampionControllerComponent>
    {
        public override void Destroy(ChampionControllerComponent self)
        {
            self.championController = null;
        }
    }

    [FriendClass(typeof (ChampionControllerComponent))]
    public static partial class ChampionControllerComponentSystem
    {
        public static void Init(this ChampionControllerComponent self, int index)
        {
            self.championController.gridType = GamePlayComponent.GridTypeOwnInventory;
            self.championController.Init(null, GamePlayComponent.TeamId_Player);
            self.championController.gridPositionX = index;
            self.championController.SetWorldPosition();
            self.championController.SetWorldRotation();
            // GamePlayController.Instance.ownChampionInventoryArray[index] = self.championController.gameObject;
        }

        public static void SetLevel(this ChampionControllerComponent self, int lv)
        {
            float newScale = 1;
            if (lv == 2)
            {
                newScale = 1.5f;
            }
            else if (lv == 3)
            {
                newScale = 2f;
            }

            GameObjectComponent gameObjectComponent = self.Parent as GameObjectComponent;
            gameObjectComponent.GameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        public static void SetDrag(this ChampionControllerComponent self, bool isDrag)
        {
            self.championController.IsDragged = isDrag;
        }

        public static void SetGridPosition(this ChampionControllerComponent self, int gridType, int gridX, int gridZ)
        {
            self.championController.SetGridPosition(gridType, gridX, gridZ);
        }
    }
}