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

            if (gateMapComponent == null)
            {
                response.Error = ErrorCode.ERR_NotInMap;
                reply();
                return;
            }

            UnitComponent unitComponent = gateMapComponent.Scene.GetComponent<UnitComponent>();
            if (unitComponent == null)
            {
                response.Error = ErrorCode.ERR_NotInMap;
                reply();
                return;
            }

            unitComponent.Remove(player.Id);
            player.RemoveComponent<GateMapComponent>();

            session.Send(new M2C_StartSceneChangeToLogin());

            await ETTask.CompletedTask;
        }
    }
}