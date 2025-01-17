using System;

namespace ET
{
    public class G2C_RefreshShopHandler: AMRpcHandler<C2G_RefreshShop, G2C_RefreshShop>
    {
        protected override async ETTask Run(Session session, C2G_RefreshShop request, G2C_RefreshShop response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
            shopComponent.RefreshShop(player.Id, true);
            response.championIds = shopComponent.GetAvailableChampionIdArray(player.Id);

            reply();
            await ETTask.CompletedTask;
        }
    }
}