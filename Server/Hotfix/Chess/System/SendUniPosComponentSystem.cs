using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class SendUniPosComponentAwakeSystem: AwakeSystem<SendUniPosComponent>
	{
		public override void Awake(SendUniPosComponent self)
		{
		}
	}

	[ObjectSystem]
	public class SendUniPosComponentDestroySystem: DestroySystem<SendUniPosComponent>
	{
		public override void Destroy(SendUniPosComponent self)
		{
		}
	}

    [ObjectSystem]
    [FriendClassAttribute(typeof(ET.SendUniPosComponent))]
    [FriendClassAttribute(typeof(ET.GamePlayComponent))]
    public class SendUniPosComponentFixedUpdateSystem : FixedUpdateSystem<SendUniPosComponent>
    {
        public override void FixedUpdate(SendUniPosComponent self)
        {
            GamePlayComponent gamePlayComponent = self.GetParent<GamePlayComponent>();
            if (gamePlayComponent.currentGameStage != GameStage.Combat)
            {
                return;
            }
            //
            // G2C_SyncUnitPos message = new G2C_SyncUnitPos();
            // Unit unit = self.selfUnit;
            // MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            // Vector3 foward = unit.Forward;
            // if (moveComponent.IsArrived())
            // {
            //     if (self.isSendArrived == false)
            //     {
            //         self.isSendArrived = true;
            //         message.UnitId = self.selfUnit.Id;
            //         message.X = unit.Position.x;
            //         message.Y = unit.Position.y;
            //         message.Z = unit.Position.z;
            //         message.ForwardX = foward.x;
            //         message.ForwardY = foward.y;
            //         message.ForwardZ = foward.z;
            //         // Log.Warning($"SendUniPosComponentFixedUpdateSystem:{message.ToString()}");
            //         self.toPlayer.SendMessage(message);
            //     }
            //     return;
            // }
            //
            // self.isSendArrived = false;
            // message.UnitId = self.selfUnit.Id;
            // message.X = unit.Position.x;
            // message.Y = unit.Position.y;
            // message.Z = unit.Position.z;
            // message.ForwardX = foward.x;
            // message.ForwardY = foward.y;
            // message.ForwardZ = foward.z;
            // // Log.Warning($"SendUniPosComponentFixedUpdateSystem:{message.ToString()}");
            // self.toPlayer.SendMessage(message);
        }
    }

    [FriendClass(typeof (SendUniPosComponent))]
	public static partial class SendUniPosComponentSystem
	{
		public static void Test(this SendUniPosComponent self)
		{
		}
	}
}
