using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ChampionControllerComponentAwakeSystem: AwakeSystem<ChampionControllerComponent, GameObjectComponent>
    {
        public override void Awake(ChampionControllerComponent self, GameObjectComponent gameObjectComponent)
        {
            self.championController = gameObjectComponent.GameObject.GetComponent<ChampionController>();
            self.championController.Init(null, ChampionController.TEAMID_PLAYER);
            self.championController.gridType = Map.GRIDTYPE_OWN_INVENTORY;
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
            self.championController.Init(null, ChampionController.TEAMID_PLAYER);
            self.championController.gridPositionX = index;
            self.championController.SetWorldPosition();
            self.championController.SetWorldRotation();
            GamePlayController.Instance.ownChampionInventoryArray[index] = self.championController.gameObject;
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
    }
}