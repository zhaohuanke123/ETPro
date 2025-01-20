using ET.Chess;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class ChampionControllerComponentAwakeSystem: AwakeSystem<ChampionControllerComponent, GameObjectComponent>
    {
        public override void Awake(ChampionControllerComponent self, GameObjectComponent gameObjectComponent)
        {
            // self.championController = gameObjectComponent.GameObject.GetComponent<ChampionController>();
            self.transform = gameObjectComponent.GameObject.transform;
        }
    }

    [ObjectSystem]
    public class ChampionControllerComponentUpdateSystem: UpdateSystem<ChampionControllerComponent>
    {
        public override void Update(ChampionControllerComponent self)
        {
            if (self._isDragged)
            {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                float enter = 100.0f;
                if (Map.Instance.m_Plane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
            
                    Vector3 p = new Vector3(hitPoint.x, 1.0f, hitPoint.z);
            
                    self.transform.position = Vector3.Lerp(self.transform.position, p, 0.1f);
                }
            }
            else
            {
                // if (gamePlayController.currentGameStage == GameStage.Preparation)
                // {
                float distance = Vector3.Distance(self.gridTargetPosition, self.transform.position);
            
                if (distance > 0.25f)
                {
                    self.transform.position = Vector3.Lerp(self.transform.position, self.gridTargetPosition, 0.1f);
                }
                else
                {
                    self.transform.position = self.gridTargetPosition;
                }
                // }
            }
        }
    }

    [ObjectSystem]
    public class ChampionControllerComponentDestroySystem: DestroySystem<ChampionControllerComponent>
    {
        public override void Destroy(ChampionControllerComponent self)
        {
            // self.championController = null;
        }
    }

    [FriendClass(typeof (ChampionControllerComponent))]
    public static partial class ChampionControllerComponentSystem
    {
        public static void Init(this ChampionControllerComponent self, int index)
        {
            self.teamID = GamePlayComponent.TeamId_Player;
            self.gridType = GamePlayComponent.GridTypeOwnInventory;
            self.gridPositionX = index;
            self.SetWorldPosition();
            self.SetWorldRotation();

            // self.championController.gridType = GamePlayComponent.GridTypeOwnInventory;
            // self.championController.Init(null, GamePlayComponent.TeamId_Player);
            // self.championController.gridPositionX = index;
            // self.championController.SetWorldPosition();
            // self.championController.SetWorldRotation();
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

            self.transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        public static void SetDrag(this ChampionControllerComponent self, bool isDrag)
        {
            self._isDragged = isDrag;
            // self.championController.IsDragged = isDrag;
        }

        public static void SetGridPosition(this ChampionControllerComponent self, int gridType, int gridX, int gridZ)
        {
            self.gridType = gridType;
            self.gridPositionX = gridX;
            self.gridPositionZ = gridZ;

            self.gridTargetPosition = self.GetWorldPosition();
            // self.championController.SetGridPosition(gridType, gridX, gridZ);
        }

        private static Vector3 GetWorldPosition(this ChampionControllerComponent self)
        {
            Vector3 worldPosition = Vector3.zero;

            if (self.gridType == GamePlayComponent.GridTypeOwnInventory)
            {
                worldPosition = Map.Instance.ownInventoryGridPositions[self.gridPositionX];
            }
            else if (self.gridType == GamePlayComponent.GridTypeMap)
            {
                worldPosition = Map.Instance.mapGridPositions[self.gridPositionX, self.gridPositionZ];
            }

            return worldPosition;
        }

        public static void SetWorldPosition(this ChampionControllerComponent self)
        {
            // navMeshAgent.enabled = false;

            Vector3 worldPosition = self.GetWorldPosition();

            self.transform.position = worldPosition;

            self.gridTargetPosition = worldPosition;
        }

        public static void SetWorldRotation(this ChampionControllerComponent self)
        {
            Vector3 rotation = Vector3.zero;

            if (self.teamID == 0)
            {
                rotation = new Vector3(0, 200, 0);
            }
            else if (self.teamID == 1)
            {
                rotation = new Vector3(0, 20, 0);
            }

            self.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}