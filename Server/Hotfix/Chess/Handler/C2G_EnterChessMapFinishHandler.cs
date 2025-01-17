using System;

namespace ET
{
    public class C2G_EnterChessMapFinishHandler: AMHandler<C2G_EnterChessMapFinish>
    {
        protected override void Run(Session session, C2G_EnterChessMapFinish message)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
            shopComponent.RefreshShop(player, true);
        }
    }
}