using System;
using System.Collections.Generic;

namespace ET
{
	public static class ItemHelper
	{
		// 从服务器获取指定索引位置的物品信息
		public static async ETTask<List<ItemInfo>> GetItemInfo(Scene scene)
		{
			try
			{
				// 获取背包数据
				G2C_BagInfo bagInfo = await scene.GetComponent<SessionComponent>().Session.Call(new C2G_BagInfo()) as G2C_BagInfo;

				if (bagInfo.Error != ErrorCode.ERR_Success)
				{
					Log.Error($"获取背包数据失败: {bagInfo.Error} {bagInfo.Message}");
					return null;
				}
				
				return bagInfo.Items;
			}
			catch (Exception e)
			{
				Log.Error($"GetItemInfo error: {e}");
				return null;
			}
		}

		// 获取物品图标名称
		public static string GetItemIconName(int itemConfigId)
		{
			// 这里可以根据配置表获取物品图标
			// 示例：return ItemConfigCategory.Instance.Get(itemConfigId).Icon;
			return $"ItemIcon_{itemConfigId}";
		}

		// 获取物品名称
		public static string GetItemName(int itemConfigId)
		{
			// 这里可以根据配置表获取物品名称
			// 示例：return ItemConfigCategory.Instance.Get(itemConfigId).Name;
			return $"Item_{itemConfigId}";
		}

		// 使用物品
		public static async ETTask<bool> UseItem(Scene scene, int itemId, int count = 1)
		{
			try
			{
				G2C_UseItem response = await scene.GetComponent<SessionComponent>().Session.Call(new C2G_UseItem()
				{
					ItemId = itemId,
					Count = count
				}) as G2C_UseItem;

				if (response.Error != ErrorCode.ERR_Success)
				{
					Log.Error($"使用物品失败: {response.Error} {response.Message}");
					return false;
				}

				return true;
			}
			catch (Exception e)
			{
				Log.Error($"UseItem error: {e}");
				return false;
			}
		}

		// 获取物品
		public static async ETTask<int> GetItemCount(Scene scene, int itemId)
		{
			try
			{
				G2C_GetItem response = await scene.GetComponent<SessionComponent>().Session.Call(new C2G_GetItem()
				{
					ItemId = itemId
				}) as G2C_GetItem;

				if (response.Error != ErrorCode.ERR_Success)
				{
					Log.Error($"获取物品失败: {response.Error} {response.Message}");
					return 0;
				}

				return response.Count;
			}
			catch (Exception e)
			{
				Log.Error($"GetItem error: {e}");
				return 0;
			}
		}

		// 添加物品
		public static async ETTask<int> AddItem(Scene scene, int itemId, int count)
		{
			try
			{
				G2C_AddItem response = await scene.GetComponent<SessionComponent>().Session.Call(new C2G_AddItem()
				{
					ItemId = itemId,
					Count = count
				}) as G2C_AddItem;

				if (response.Error != ErrorCode.ERR_Success)
				{
					Log.Error($"添加物品失败: {response.Error} {response.Message}");
					return 0;
				}

				return response.Count;
			}
			catch (Exception e)
			{
				Log.Error($"AddItem error: {e}");
				return 0;
			}
		}
	}
}
