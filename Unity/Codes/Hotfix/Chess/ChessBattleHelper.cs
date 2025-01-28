using System;

namespace ET
{
	public static class ChessBattleHelper
	{
		public static async ETTask RefreshShopChampionList(Scene zoneScene)
		{
			Session session = zoneScene.GetComponent<SessionComponent>().Session;
			G2C_RefreshShop g2CRefreshShop = await session.Call(new C2G_RefreshShop()) as G2C_RefreshShop;

			if (g2CRefreshShop.Error != ErrorCode.ERR_Success)
			{
				return;
			}

			Game.EventSystem.Publish(new UIEventType.RefreshShop()
			{
				championIds = g2CRefreshShop.championIds
			});
		}

		public static async ETTask TryBuyChampion(Scene zoneScene, int index)
		{
			Session session = zoneScene.GetComponent<SessionComponent>().Session;
			C2G_BuyChampion request = new C2G_BuyChampion();
			request.SlopIndex = index;

			G2C_BuyChampion response = await session.Call(request) as G2C_BuyChampion;

			if (response.Error != ErrorCode.ERR_Success)
			{
				return;
			}

			// int responseCpId = response.CPId;
			// Scene currentScene = zoneScene.CurrentScene();
			// Unit unit = UnitFactory.Create(currentScene, response.UnitInfo);

			await Game.EventSystem.PublishAsync(new EventType.GenChampions()
			{
				zoneScene = zoneScene,
				CPInfos = response.CPInfos,
				// unit = unit
			});
		}

		// champion 位置修改后发送request到服务器， 使用C2G_DragChampion,
		public static async ETTask SendDragMessage(Scene zoneScene, TriggerInfo preInfo, TriggerInfo newInfo)
		{
			Session session = zoneScene.GetComponent<SessionComponent>().Session;

			C2G_DragChampion request = new C2G_DragChampion();
			request.OldGridType = preInfo.gridType;
			request.OldGridPositionX = preInfo.gridX;
			request.OldGridPositionZ = preInfo.gridZ;
			request.NewGridType = newInfo.gridType;
			request.NewGridPositionX = newInfo.gridX;
			request.NewGridPositionZ = newInfo.gridZ;

			try
			{
				G2C_DragChampion response = await session.Call(request) as G2C_DragChampion;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async ETTask SendExitChessMap(Scene zoneScene)
		{
			Session session = zoneScene.GetComponent<SessionComponent>().Session;
			session.Send(new C2G_ExitChessMap());
			await TimerComponent.Instance.WaitAsync(1000);
		}
	}
}
