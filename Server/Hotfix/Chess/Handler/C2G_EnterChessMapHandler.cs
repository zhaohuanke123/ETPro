using System;
using MongoDB.Driver.Core.Events;

namespace ET
{
    public class C2G_EnterChessMapHandler: AMRpcHandler<C2G_EnterChessMap, G2C_EnterChessMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterChessMap request, G2C_EnterChessMap response, Action reply)
        {
            const string map = "Main";
            // StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), map);

            //TODO: 临时

            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

            session.RemoveComponent<GamePlayComponent>();
            GamePlayComponent gamePlayComponent = session.AddComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.AddComponent<ShopComponent>();
            gamePlayComponent.AddComponent<ChampionArrayComponent>();
            session.DomainScene().AddComponent<UnitComponent>();
            ChampionMapArrayComponent championMapArrayComponent = gamePlayComponent.AddComponent<ChampionMapArrayComponent>();
            championMapArrayComponent.AddComponent<BattleChampionBonusComponent>();
            shopComponent.AddPlayerGold(player, 100);

            response.SceneInstanceId = 123;
            response.SceneName = map;
            response.MyId = player.Id;
            reply();

            await ETTask.CompletedTask;
        }
    }
}