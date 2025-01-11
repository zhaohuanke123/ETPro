using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    public class C2G_ExitHandler: AMRpcHandler<C2G_ExitMap, G2C_ExitMap>
    {
        protected override async ETTask Run(Session session, C2G_ExitMap request, G2C_ExitMap response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GateMapComponent gateMapComponent = player.GetComponent<GateMapComponent>();
            player.RemoveComponent<GateMapComponent>();

            string map = "Map1";
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), map);
            MapSceneConfig mapSceneConfig = MapSceneConfigCategory.Instance.Get(startSceneConfig.Id);

            await TransferHelper.ExitMap(player.Id, startSceneConfig.InstanceId, mapSceneConfig.Name);

            session.Send(new M2C_StartSceneChangeToLogin());

            await ETTask.CompletedTask;
        }
    }
}