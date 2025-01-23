using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof (ET.ChampionInfo))]
    [FriendClassAttribute(typeof (ET.MapComponent))]
    [FriendClassAttribute(typeof (ET.Unit))]
    public class C2G_BuyChampionHandler: AMRpcHandler<C2G_BuyChampion, G2C_BuyChampion>
    {
        protected override async ETTask Run(Session session, C2G_BuyChampion request, G2C_BuyChampion response, Action reply)
        {
            // 获取player
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
            ChampionArrayComponent championArrayComponent = gamePlayComponent.GetComponent<ChampionArrayComponent>();

            int id = shopComponent.GetIdByIndex(player, request.SlopIndex);
            if (id == -1)
            {
                response.Error = ErrorCode.ArgumentNotRight;
                reply();
                return;
            }

            ChampionConfig config = ChampionConfigCategory.Instance.Get(id);

            //  钱够不 
            if (!shopComponent.IsEnoughGold(player.Id, config.cost))
            {
                response.Error = ErrorCode.GoldNotEnough;
                reply();
                return;
            }

            if (!championArrayComponent.TryAdd(player, id))
            {
                response.Error = ErrorCode.ChampionArrayFull;
                reply();
                return;
            }

            // 扣钱
            shopComponent.SubPlayerGold(player, config.cost);

            response.CPInfos = championArrayComponent.GetAllInventoryChampionInfo(player);

            // response.CPId = id;
            // response.InventoryIndex = championInfo.gridPositionX;

            // Unit unit = UnitFactory.Create(session.DomainScene(), IdGenerater.Instance.GenerateUnitId(session.DomainZone()), UnitType.Monster);
            // unit.ConfigId = config.unitId;
            //
            // Log.Warning($"unitID : {unit.Id.ToString()}");
            // MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            // unit.Position = MapComponent.Instance.mapGridPositions[0, 0];
            // unit.AddComponent<SendUniPosComponent, Player>(player);
            // Log.Warning($"unitPos : {unit.Position.ToString()}");
            // response.UnitInfo = UnitHelper.CreateUnitInfo(unit);

            reply();

            // await TimerComponent.Instance.WaitAsync(1);
            // await moveComponent.MoveToAsync(MapComponent.Instance.mapGridPositions[6, 6], 6);
            // await TimerComponent.Instance.WaitAsync(1);
            // await moveComponent.MoveToAsync(MapComponent.Instance.mapGridPositions[1, 1], 6);
            // await TimerComponent.Instance.WaitAsync(1);
            // await moveComponent.MoveToAsync(MapComponent.Instance.mapGridPositions[6, 1], 6);
            // await TimerComponent.Instance.WaitAsync(1);
            // await moveComponent.MoveToAsync(MapComponent.Instance.mapGridPositions[1, 6], 6);

            await ETTask.CompletedTask;
        }
    }
}