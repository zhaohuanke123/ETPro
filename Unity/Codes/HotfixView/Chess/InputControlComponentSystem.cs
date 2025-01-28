using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class InputControlComponentAwakeSystem: AwakeSystem<InputControlComponent>
	{
		public override void Awake(InputControlComponent self)
		{
			self.mainCamera = Camera.main;
			self.triggerLayer = 1 << 10;
		}
	}

	[ObjectSystem]
	public class InputControlComponentDestroySystem: DestroySystem<InputControlComponent>
	{
		public override void Destroy(InputControlComponent self)
		{
		}
	}

    [ObjectSystem]
    [FriendClassAttribute(typeof(ET.GamePlayComponent))]
    public class InputControlComponentUpdateSystem : UpdateSystem<InputControlComponent>
    {
        public override void Update(InputControlComponent self)
        {
            GamePlayComponent gamePlayComponent = self.GetParent<GamePlayComponent>();
            if (gamePlayComponent.currentGameStage == GameStage.Combat)
            {
                return;
            }

            self.triggerInfo = null;
            Map.Instance.resetIndicators();

            UnityEngine.RaycastHit hit;

            UnityEngine.Ray ray = self.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out hit, 100f, self.triggerLayer, QueryTriggerInteraction.Collide))
            {
                self.triggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();
                // Log.Warning($"this.triggerInfo : {self.triggerInfo}");

                if (self.triggerInfo != null)
                {
                    GameObject indicator = Map.Instance.GetIndicatorFromTriggerInfo(self.triggerInfo);

                    indicator.GetComponent<MeshRenderer>().material.color = Map.Instance.indicatorActiveColor;
                }
                else
                {
                    Map.Instance.resetIndicators();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                ChessBattleViewComponent.Instance.StartDrag(self.triggerInfo);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ChessBattleViewComponent.Instance.EndDrag(self.triggerInfo);
            }

            self.mousePosition = Input.mousePosition;
        }
    }

    [FriendClass(typeof (InputControlComponent))]
	public static partial class InputControlComponentSystem
	{
		public static void Test(this InputControlComponent self)
		{
		}
	}
}
