using System;

namespace ET
{
    public class G2C_PassGalHandler: AMRpcHandler<C2G_PassGal, G2C_PassGal>
    {
        protected override async ETTask Run(Session session, C2G_PassGal request, G2C_PassGal response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GalComponent galComponent = player.GetComponent<GalComponent>();
            response.NextGalId = galComponent.PassGal();
            reply();
            await ETTask.CompletedTask;
        }
    }
}