using System;

namespace ET
{
	public static class EnterMapHelper
	{
		public static async ETTask EnterMapAsync(Scene zoneScene)
		{
			try
			{
				var waiter = zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
				G2C_EnterMap g2CEnterMap = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
				zoneScene.GetComponent<PlayerComponent>().MyId = g2CEnterMap.MyId;

				// 等待场景切换完成
				await waiter;

				Game.EventSystem.Publish(new EventType.EnterMapFinish()
				{
					ZoneScene = zoneScene
				});
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async ETTask ExitMapAsync(Scene zoneScene)
		{
			G2C_ExitMap g2CExitMap = default;
			try
			{
				var waiter = zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
				g2CExitMap = (G2C_ExitMap)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_ExitMap());

				await waiter;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async ETTask EnterChessMapAsync(Scene zoneScene)
		{
			try
			{
				Session session = zoneScene.GetComponent<SessionComponent>().Session;
				G2C_EnterChessMap g2CEnterChessMap =
						await session.Call(new C2G_EnterChessMap()) as G2C_EnterChessMap;
				zoneScene.GetComponent<PlayerComponent>().MyId = g2CEnterChessMap.MyId;

				await SceneChangeHelper.SceneChangeToChessMap(zoneScene, g2CEnterChessMap.SceneName, g2CEnterChessMap.SceneInstanceId);

				// session.Send(new C2G_EnterChessMap());
#if !NOT_UNITY
				G2C_RefreshShop g2CRefreshShop = (G2C_RefreshShop)await session.Call(new C2G_RefreshShop());
				Game.EventSystem.Publish(new UIEventType.RefreshShop()
				{
					championIds = g2CRefreshShop.championIds
				});
#endif
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async ETTask EnterMatchChessMapAsync(Scene zoneScene)
		{
			await SceneChangeHelper.SceneChangeToChessMap(zoneScene, "Main", 0);

#if !NOT_UNITY
			Session session = zoneScene.GetComponent<SessionComponent>().Session;
			G2C_RefreshShop g2CRefreshShop = (G2C_RefreshShop)await session.Call(new C2G_RefreshShop());
			Game.EventSystem.Publish(new UIEventType.RefreshShop()
			{
				championIds = g2CRefreshShop.championIds
			});
#endif
		}
	}
}
