namespace ET
{
    [ObjectSystem]
    public class SendUniPosComponentAwakeSystem: AwakeSystem<SendUniPosComponent, Player>
    {
        public override void Awake(SendUniPosComponent self, Player player)
        {
            self.selfUnit = self.Parent as Unit;
            self.toPlayer = player;
        }
    }

    [ObjectSystem]
    public class SendUniPosComponentDestroySystem: DestroySystem<SendUniPosComponent>
    {
        public override void Destroy(SendUniPosComponent self)
        {
            self.selfUnit = null;
        }
    }

    [ObjectSystem]
    [FriendClassAttribute(typeof (ET.SendUniPosComponent))]
    public class SendUniPosComponentFixedUpdateSystem: FixedUpdateSystem<SendUniPosComponent>
    {
        public override void FixedUpdate(SendUniPosComponent self)
        {
            if (self.selfUnit == null)
            {
                return;
            }

            G2C_SyncUnitPos message = new G2C_SyncUnitPos();
            Unit unit = self.selfUnit;
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent.IsArrived())
            {
                return;
            }

            message.UnitId = self.selfUnit.Id;
            message.X = unit.Position.x;
            message.Y = unit.Position.y;
            message.Z = unit.Position.z;
            Log.Warning($"SendUniPosComponentFixedUpdateSystem:{message.ToString()}");
            self.toPlayer.Session.Send(message);
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