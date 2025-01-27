using System;
using ET.Chess;
using MongoDB.Driver.Core.Events;
using UnityEngine;

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

			gamePlayComponent.AddComponent<MapComponent>();
			ShopComponent shopComponent = gamePlayComponent.AddComponent<ShopComponent>();

			gamePlayComponent.AddComponent<ChampionArrayComponent>();
			gamePlayComponent.AddComponent<UnitComponent>();

			ChampionMapArrayComponent championMapArrayComponent = gamePlayComponent.AddComponent<ChampionMapArrayComponent>();
			championMapArrayComponent.AddComponent<BattleChampionBonusComponent>();
			gamePlayComponent.AddPlayer(player);

			// TODO 临时
			player.SetCamp(Camp.Player1);

			// 添加空的player，作为人机
			gamePlayComponent.AddComponent<TmpAiChampionComponentComponent, Player>(player);
			// TODO 临时
			shopComponent.AddPlayerGold(player, 100);

			response.SceneInstanceId = 123;
			response.SceneName = map;
			response.MyId = player.Id;
			reply();

			await ETTask.CompletedTask;
		}
	}
}
