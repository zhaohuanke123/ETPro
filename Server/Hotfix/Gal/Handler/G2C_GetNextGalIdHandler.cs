using System;

namespace ET
{
    public class G2C_GetNextGalIdHandler: AMRpcHandler<C2G_GetNextGalId, G2C_GetNextGalId>
    {
        protected override async ETTask Run(Session session, C2G_GetNextGalId request, G2C_GetNextGalId response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GalComponent galComponent = player.GetComponent<GalComponent>();

            response.GalId = galComponent.GetNextGalId();

            reply();
            await ETTask.CompletedTask;
        }
    }
}