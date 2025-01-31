using System;

namespace ET
{
	public static class ItemHelper
	{
		// 向服务器查询item
		public static async ETTask<int> GetItemCount(Scene zoneScene, int id)
		{
			C2G_GetItem request = new C2G_GetItem()
			{
				ItemId = id
			};

			G2C_GetItem response = null;
			try
			{
				response = await zoneScene.GetComponent<SessionComponent>().Session.Call(request) as G2C_GetItem;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}

			if (response.Error != ErrorCode.ERR_Success)
			{
				Log.Error($"GetItemCount error : {response.Error.ToString()}");
				return 0;
			}

			return response.Count;
		}

		// 向服务器请求添加道具
		public static async ETTask<int> AddItem(Scene zoneScene, int itemId, int count = 1)
		{
			C2G_AddItem request = new C2G_AddItem()
			{
				ItemId = itemId,
				Count = count
			};

			G2C_AddItem response = null;
			try
			{
				response = await zoneScene.GetComponent<SessionComponent>().Session.Call(request) as G2C_AddItem;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return 0;
			}

			if (response.Error != ErrorCode.ERR_Success)
			{
				Log.Error($"AddItem error: {response.Error}");
				return 0;
			}

			return response.Count;
		}
	}
}
