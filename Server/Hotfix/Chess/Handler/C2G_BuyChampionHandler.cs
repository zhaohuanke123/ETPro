using System;

namespace ET
{
    public class C2G_BuyChampionHandler: AMRpcHandler<C2G_BuyChampion, G2C_BuyChampion>
    {
        protected override async ETTask Run(Session session, C2G_BuyChampion request, G2C_BuyChampion response, Action reply)
        {
            // 获取player
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
            // 查询champion，是否够金币

            int id = shopComponent.GetIdByIndex(player, request.slopIndex);
            if (id == -1)
            {
                response.Error = ErrorCode.ArgumentNotRight;
                reply();
                return;
            }

            ChampionConfig config = ChampionConfigCategory.Instance.Get(id);
            if (!shopComponent.TrySubPlayerGold(player, config.cost))
            {
                response.Error = ErrorCode.GoldNotEnough;
                reply();
                return;
            }

            response.cpId = id;
            reply();
            await ETTask.CompletedTask;
        }
    }
}