using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof (ET.ChampionInfo))]
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

            // 加角色
            // ChampionInfo championInfo = championArrayComponent.AddChild<ChampionInfo>();
            // NumericComponent numericComponent = championInfo.AddComponent<NumericComponent>();
            // numericComponent.Set(NumericType.Lv, 1);

            // numericComponent.Set();
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
            reply();

            Unit unit = UnitFactory.Create(session.DomainScene(), player.Id, UnitType.Monster);
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            await moveComponent.MoveToAsync(new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 5, 0), new Vector3(0, 10, 0) }, 1);
            Log.Warning(unit.Position.ToString());

            await ETTask.CompletedTask;
        }
    }
}