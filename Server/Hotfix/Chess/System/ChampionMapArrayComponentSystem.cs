using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using MongoDB.Driver.Core.Events;
using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class ChampionMapArrayComponentAwakeSystem: AwakeSystem<ChampionMapArrayComponent>
	{
		public override void Awake(ChampionMapArrayComponent self)
		{
			self.grid = new ChampionInfo[GPDefine.HexMapSizeX, GPDefine.HexMapSizeZ];
		}
	}

	[ObjectSystem]
	public class ChampionMapArrayComponentDestroySystem: DestroySystem<ChampionMapArrayComponent>
	{
		public override void Destroy(ChampionMapArrayComponent self)
		{
		}
	}

	[FriendClass(typeof (ChampionMapArrayComponent))]
	[FriendClassAttribute(typeof (ET.ChampionInfo))]
	[FriendClassAttribute(typeof (ET.Player))]
	public static partial class ChampionMapArrayComponentSystem
	{
		public static void AddPlayer(this ChampionMapArrayComponent self, Player player)
		{
			BattleChampionBonusComponent battleChampionBonusComponent = self.GetComponent<BattleChampionBonusComponent>();
			battleChampionBonusComponent.AddPlayer(player);
		}

		public static void AddToGrid(this ChampionMapArrayComponent self, Player player, ChampionInfo championInfo, int gridX, int gridZ)
		{
			if (player.camp != Camp.Player1)
			{
				gridX = GPDefine.HexMapSizeX - 1 - gridX;
				gridZ = GPDefine.HexMapSizeZ - 1 - gridZ;
			}

			self.AddChild(championInfo);
			championInfo.gridPositionX = gridX;
			championInfo.gridPositionZ = gridZ;
			self.grid[gridX, gridZ] = championInfo;
		}

		public static void Replace(this ChampionMapArrayComponent self, Player player, ChampionInfo championInfo, int gridX, int gridZ)
		{
			if (player.camp != Camp.Player1)
			{
				gridX = GPDefine.HexMapSizeX - 1 - gridX;
				gridZ = GPDefine.HexMapSizeZ - 1 - gridZ;
			}

			championInfo.gridPositionX = gridX;
			championInfo.gridPositionZ = gridZ;
			self.grid[gridX, gridZ] = championInfo;
		}

		public static ChampionInfo RemoveFromGird(this ChampionMapArrayComponent self, Player player, int gridX, int gridZ)
		{
			if (player.camp != Camp.Player1)
			{
				gridX = GPDefine.HexMapSizeX - 1 - gridX;
				gridZ = GPDefine.HexMapSizeZ - 1 - gridZ;
			}

			ChampionInfo championInfo = self.grid[gridX, gridZ];
			self.grid[gridX, gridZ] = null;
			return championInfo;
		}

		public static List<ChampionInfo> GetChampionInfos(this ChampionMapArrayComponent self, Player player)
		{
			int fromX, toX, fromZ, toZ;
			if (player.camp == Camp.Player1)
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = 0;
				toZ = GPDefine.HexMapSizeZ / 2;
			}
			else
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = GPDefine.HexMapSizeZ / 2;
				toZ = GPDefine.HexMapSizeZ;
			}

			List<ChampionInfo> championInfos = new List<ChampionInfo>();
			for (int x = fromX; x < toX; x++)
			{
				for (int z = fromZ; z < toZ; z++)
				{
					if (self.grid[x, z] != null)
					{
						championInfos.Add(self.grid[x, z]);
					}
				}
			}

			return championInfos;
		}

		public static void CalculateBonuses(this ChampionMapArrayComponent self, Player player)
		{
			int fromX, toX, fromZ, toZ;
			if (player.camp == Camp.Player1)
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = 0;
				toZ = GPDefine.HexMapSizeZ / 2;
			}
			else
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = GPDefine.HexMapSizeZ / 2;
				toZ = GPDefine.HexMapSizeZ;
			}

			BattleChampionBonusComponent battleChampionBonusComponent = self.GetComponent<BattleChampionBonusComponent>();

			Dictionary<int, int> championTypeCount = battleChampionBonusComponent.GetPlayerChampionTypeCount(player);
			championTypeCount.Clear();
			for (int x = fromX; x < toX; x++)
			{
				for (int z = fromZ; z < toZ; z++)
				{
					if (self.grid[x, z] != null)
					{
						ChampionInfo championInfo = self.grid[x, z];
						int id1 = championInfo.config.type1Id;
						int id2 = championInfo.config.type2Id;

						if (!championTypeCount.TryAdd(id1, 1))
						{
							int cCount = 0;
							championTypeCount.TryGetValue(id1, out cCount);

							cCount++;

							championTypeCount[id1] = cCount;
						}

						if (!championTypeCount.TryAdd(id2, 1))
						{
							int cCount = 0;
							championTypeCount.TryGetValue(id2, out cCount);

							cCount++;

							championTypeCount[id2] = cCount;
						}
					}
				}
			}

			var activeBonusList = battleChampionBonusComponent.GetPlayerActiveBonus(player);
			activeBonusList.Clear();

			foreach (KeyValuePair<int, int> m in championTypeCount)
			{
				int id = m.Key;
				ChampionBonusConfig championBonusConfig = ChampionBonusConfigCategory.Instance.Get(id);
				if (m.Value >= championBonusConfig.championCount)
				{
					activeBonusList.Add(championBonusConfig);
				}
			}

			G2C_UpdateBonus message = new G2C_UpdateBonus();
			message.TypeIdList = championTypeCount.Keys.ToList();
			message.CountList = championTypeCount.Values.ToList();
			player.SendMessage(message);
		}

		public static Quaternion GetPlayerChampionRotate(this ChampionMapArrayComponent self, Player player)
		{
			if (player.camp == Camp.Player1)
			{
				return Quaternion.Euler(new Vector3(0, 200, 0));
			}
			else
			{
				return Quaternion.Euler(new Vector3(0, 20, 0));
			}
		}

		public static (int x, int z) GetEmptyPos(this ChampionMapArrayComponent self, Player player)
		{
			int fromX, toX, fromZ, toZ;
			if (player.camp == Camp.Player1)
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = 0;
				toZ = GPDefine.HexMapSizeZ / 2;
			}
			else
			{
				fromX = 0;
				toX = GPDefine.HexMapSizeX;
				fromZ = GPDefine.HexMapSizeZ / 2;
				toZ = GPDefine.HexMapSizeZ;
			}

			ChampionInfo[,] championInfos = self.grid;

			for (int x = fromX; x < toX; x++)
			{
				for (int z = fromZ; z < toZ; z++)
				{
					if (championInfos[x, z] != null)
					{
						continue;
					}

					return (x, z);
				}
			}
			return (-1, -1);
		}
	}
}
