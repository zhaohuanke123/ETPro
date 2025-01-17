﻿using System;

namespace ET
{
    [FriendClassAttribute(typeof (ET.ChampionInfo))]
    public class C2G_DragChampionHandler: AMRpcHandler<C2G_DragChampion, G2C_DragChampion>
    {
        protected override async ETTask Run(Session session, C2G_DragChampion request, G2C_DragChampion response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
            ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
            ChampionArrayComponent championArrayComponent = gamePlayComponent.GetComponent<ChampionArrayComponent>();
            ChampionMapArrayComponent mapArrayComponent = gamePlayComponent.GetComponent<ChampionMapArrayComponent>();

            if (request.OldGridType == GamePlayComponent.GridTypeOwnInventory)
            {
                ChampionInfo championInfo = championArrayComponent.RemoveFromArray(player, request.OldGridPositionX, out var res);
                //TODO 校验
                // ChampionInfo championInfo = remove;

                if (request.NewGridType == GamePlayComponent.GridTypeMap)
                {
                    mapArrayComponent.AddToGrid(player, championInfo, request.NewGridPositionX, request.NewGridPositionZ);
                }
                else
                {
                    championInfo.gridPositionX = request.NewGridPositionX;
                    championArrayComponent.Replace(player, championInfo);
                }
            }
            else
            {
                ChampionInfo championInfo = mapArrayComponent.RemoveFromGird(player, request.OldGridPositionX, request.OldGridPositionZ);

                if (request.NewGridType == GamePlayComponent.GridTypeMap)
                {
                    ChampionInfo toChampionInfo = mapArrayComponent.RemoveFromGird(player, request.NewGridPositionX, request.NewGridPositionZ);
                    if (toChampionInfo != null)
                    {
                        mapArrayComponent.Replace(player, toChampionInfo, request.OldGridPositionX, request.OldGridPositionZ);
                    }

                    mapArrayComponent.Replace(player, championInfo, request.NewGridPositionX, request.NewGridPositionZ);
                }
                else
                {
                    championInfo.gridPositionX = request.NewGridPositionX;
                    championArrayComponent.AddToArray(player, championInfo);
                }
            }

            mapArrayComponent.CalculateBonuses(player);

            reply();
            await ETTask.CompletedTask;
        }
    }
}