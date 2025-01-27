using System;
using System.Collections.Generic;

namespace ET
{
	[ObjectSystem]
	public class ChampionArrayComponentAwakeSystem: AwakeSystem<ChampionArrayComponent>
	{
		public override void Awake(ChampionArrayComponent self)
		{
			self.playersInventoryArr = new Dictionary<long, List<ChampionInfo>>();
		}
	}

	[ObjectSystem]
	public class ChampionArrayComponentDestroySystem: DestroySystem<ChampionArrayComponent>
	{
		public override void Destroy(ChampionArrayComponent self)
		{
		}
	}

	[FriendClass(typeof (ChampionArrayComponent))]
	[FriendClass(typeof (ET.ChampionInfo))]
	public static class ChampionArrayComponentSystem
	{
		public static void AddPlayer(this ChampionArrayComponent self, Player player)
		{
			if (!self.playersInventoryArr.TryAdd(player.Id, new List<ChampionInfo>(new ChampionInfo[GPDefine.InventorySize])))
			{
				Log.Error("ChampionArray AddPlayer 玩家已经存在");
			}
		}

		// public static List<ChampionInfo> InitPlayerArray(this ChampionArrayComponent self, long playerId)
		// {
		//     var list = new List<ChampionInfo>(new ChampionInfo[GamePlayComponent.InventorySize]);
		//     self.playersInventoryArr[playerId] = list;
		//     return list;
		// }

		public static int FindEmptyIndex(this ChampionArrayComponent self, Player player)
		{
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			int emptyIndex = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null)
				{
					emptyIndex = i;
					break;
				}
			}

			return emptyIndex;
		}

		public static bool TryAdd(this ChampionArrayComponent self, Player player, int configId)
		{
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			// 先看看能不能合并升级
			if (self.TryUpgrade(list, configId))
			{
				return true;
			}

			// 找到空位
			int index = self.FindEmptyIndex(player);
			if (index == -1)
			{
				return false;
			}

			ChampionInfo championInfo = self.AddChild<ChampionInfo>();
			championInfo.configId = configId;
			NumericComponent numericComponent = championInfo.AddComponent<NumericComponent>();
			numericComponent.Set(NumericType.Lv, 1);

			championInfo.gridType = GridType.OwnInventory;
			championInfo.gridPositionX = index;
			list[index] = championInfo;
			return true;
		}

		public static bool AddToArray(this ChampionArrayComponent self, Player player, ChampionInfo championInfo)
		{
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			self.AddChild(championInfo);
			int index = championInfo.gridPositionX;
			list[index] = championInfo;
			return true;
		}

		public static bool Replace(this ChampionArrayComponent self, Player player, ChampionInfo championInfo)
		{
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			list[championInfo.gridPositionX] = championInfo;
			return true;
		}

		public static bool TryUpgrade(this ChampionArrayComponent self, List<ChampionInfo> list, int configId)
		{
			var championList_lvl_1 = new List<ChampionInfo>();
			var championList_lvl_2 = new List<ChampionInfo>();

			foreach (ChampionInfo info in list)
			{
				if (info != null)
				{
					if (info.configId == configId)
					{
						int lv = info.GetComponent<NumericComponent>().GetAsInt(NumericType.Lv);
						if (lv == 1)
						{
							championList_lvl_1.Add(info);
						}
						else if (lv == 2)
						{
							championList_lvl_2.Add(info);
						}
					}
				}
			}

			// 3个1 级合成 1个2级
			if (championList_lvl_1.Count == 2)
			{
				NumericComponent numericComponent = championList_lvl_1[0].GetComponent<NumericComponent>();
				numericComponent.Set(NumericType.Lv, 2);

				int index = championList_lvl_1[1].gridPositionX;
				ChampionInfo info = list[index];
				info.Dispose();
				list[index] = null;

				// 3个2级合成1个3级
				if (championList_lvl_2.Count == 2)
				{
					numericComponent.Set(NumericType.Lv, 3);

					index = championList_lvl_2[0].gridPositionX;
					info = list[index];
					info.Dispose();
					list[index] = null;

					index = championList_lvl_2[1].gridPositionX;
					info = list[index];
					info.Dispose();
					list[index] = null;
				}

				return true;
			}

			return false;
		}

		public static List<ChampionInfoPB> GetAllInventoryChampionInfo(this ChampionArrayComponent self, Player player)
		{
			var res = new List<ChampionInfoPB>();
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			foreach (ChampionInfo info in list)
			{
				if (info != null)
				{
					ChampionInfoPB championInfoPb = new ChampionInfoPB();
					championInfoPb.Lv = info.GetComponent<NumericComponent>().GetAsInt(NumericType.Lv);
					championInfoPb.ConfigId = info.configId;
					championInfoPb.GridPositionX = info.gridPositionX;
					championInfoPb.Type = info.gridType;
					res.Add(championInfoPb);
				}
			}

			return res;
		}

		public static ChampionInfo RemoveFromArray(this ChampionArrayComponent self, Player player, int index, out bool res)
		{
			if (!self.playersInventoryArr.TryGetValue(player.Id, out var list))
			{
				throw new ArgumentException("玩家不存在");
			}

			ChampionInfo championInfo = list[index];
			if (championInfo == null)
			{
				res = false;
			}
			else
			{
				res = true;
				list[index] = null;
			}

			return championInfo;
		}
	}
}
