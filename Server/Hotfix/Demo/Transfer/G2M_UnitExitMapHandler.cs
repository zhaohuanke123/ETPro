namespace ET
{
    [ActorMessageHandler]
    public class G2M_UnitExitMapHandler: AMActorHandler<Scene, G2M_UnitExitMap>
    {
        protected override async ETTask Run(Scene scene, G2M_UnitExitMap message)
        {
            long plyerId = message.PlyerId;

            // 移除玩家对应的Unit
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.Get(plyerId);

            if (unit != null)
            {
                unitComponent.Remove(plyerId);
                unit.Dispose();
                Log.Info($"玩家{plyerId}离开地图");
            }

            await ETTask.CompletedTask;
        }
    }
}