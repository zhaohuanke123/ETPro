using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ET
{
	[ObjectSystem]
	public class ShopComponentAwakeSystem: AwakeSystem<ShopComponent>
	{
		public override void Awake(ShopComponent self)
		{
			self.availableChampionIdArray = new Dictionary<long, List<int>>();
			self.playersGoldDict = new Dictionary<long, int>();
			self.playerCurrentChampionLimit = new Dictionary<long, int>();
			self.championIdsArray = ChampionConfigCategory.Instance.GetAll().Keys.ToArray();
			self.championTypeIdsArray = ChampionTypeConfigCategory.Instance.GetAll().Keys.ToArray();
			self.playerLevelDict = new Dictionary<long, int>();
			self.playerHeroLevelDict = new Dictionary<long, int>();
		}
	}

	[ObjectSystem]
	public class ShopComponentDestroySystem: DestroySystem<ShopComponent>
	{
		public override void Destroy(ShopComponent self)
		{
		}
	}

	[FriendClass(typeof (ShopComponent))]
	public static partial class ShopComponentSystem
	{
		public static void AddPlayer(this ShopComponent self, Player player)
		{
			void TryAddWithException<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value, string errorMessage)
			{
				if (!dictionary.TryAdd(key, value))
				{
					throw new ArgumentException(errorMessage);
				}
			}

			void TryAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
			{
				dictionary.TryAdd(key, value);
			}

			// 添加玩家的英雄列表
			TryAddWithException(self.availableChampionIdArray, player.Id, new List<int>(), "玩家已存在");

			// 初始化玩家的金币
			TryAdd(self.playersGoldDict, player.Id, GPDefine.InitGold);

			// 初始化玩家的英雄上限
			TryAdd(self.playerCurrentChampionLimit, player.Id, GPDefine.InitChampionLimit);

			// 初始化玩家等级
			TryAdd(self.playerLevelDict, player.Id, 1);

			// 初始化玩家的英雄等级
			TryAdd(self.playerHeroLevelDict, player.Id, 1);
		}

		public static void AddPlayerGold(this ShopComponent self, Player player, int gold)
		{
			if (!self.playersGoldDict.TryGetValue(player.Id, out int count))
			{
				throw new ArgumentException("玩家不存在");
			}

			int res = count + gold;
			self.playersGoldDict[player.Id] = res;

			self.SendRefreshGold(player, count + res);
		}

		public static void SubPlayerGold(this ShopComponent self, Player player, int gold)
		{
			if (self.playersGoldDict.TryGetValue(player.Id, out int count))
			{
				if (count >= gold)
				{
					self.playersGoldDict[player.Id] -= gold;
					self.SendRefreshGold(player, count - gold);
				}
				else
				{
					throw new InvalidOperationException("金币不足");
				}
			}
		}

		public static bool TrySubPlayerGold(this ShopComponent self, Player player, int goldCount)
		{
			if (self.playersGoldDict.TryGetValue(player.Id, out int count))
			{
				if (count >= goldCount)
				{
					self.playersGoldDict[player.Id] -= goldCount;
					self.SendRefreshGold(player, count - goldCount);
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		public static bool IsEnoughGold(this ShopComponent self, long playerId, int gold)
		{
			if (!self.playersGoldDict.TryGetValue(playerId, out int value))
			{
				return false;
			}

			return value >= gold;
		}

		/// <summary>
		/// 测试，获取随机英雄
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static int GetRandomChampionId(this ShopComponent self)
		{
			int rand = RandomHelper.RandomNumber(0, self.championIdsArray.Length);

			return self.championIdsArray[rand];
		}

		/// <summary>
		/// 刷新商店
		/// </summary>
		/// <param name="self"></param>
		/// <param name="player"></param>
		/// <param name="isFree"></param>
		public static bool RefreshShop(this ShopComponent self, Player player, bool isFree = false)
		{
			HeroComponent heroComponent = player.GetComponent<HeroComponent>();
			if (heroComponent == null)
			{
				return false;
			}

			if (self.playersGoldDict.TryGetValue(player.Id, out int gold))
			{
				if (gold < 2 && isFree == false)
				{
					return false;
				}
			}

			// if (self.availableChampionIdArray.ContainsKey(player.Id))
			// {
			//     self.availableChampionIdArray.Remove(player.Id);
			// }

			int playerHeroLevel = self.playerHeroLevelDict[player.Id];
			var selfAvailableChampions = self.availableChampionIdArray[player.Id];
			selfAvailableChampions.Clear();
			// 遍历拥有的英雄，获取英雄等级，对比玩家英雄等级，如果等级小于等于玩家英雄等级，则将英雄id加入到availableChampionIdArray中
			foreach (var id in heroComponent.GetAllHeroIds())
			{
				HeroConfig config = HeroConfigCategory.Instance.Get(id);
				if (config.Lv <= playerHeroLevel)
				{
					selfAvailableChampions.Add(config.ChampionId);
				}
			}

			// 随机打乱并取前5个
			self.availableChampionIdArray[player.Id] = selfAvailableChampions.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

			// TODO 临时
			// const int count = 5;
			// var availableChampionArray = new List<int>(count);
			// self.availableChampionIdArray.Add(player.Id, availableChampionArray);

			// for (int i = 0; i < count; i++)
			// {
			//     int randomChampionId = self.GetRandomChampionId();

			//     availableChampionArray.Add(randomChampionId);
			// }

			if (isFree == false)
			{
				self.SubPlayerGold(player, 2);
			}

			return true;
		}

		public static List<int> GetAvailableChampionIdArray(this ShopComponent self, long id)
		{
			if (!self.availableChampionIdArray.TryGetValue(id, out List<int> value))
			{
				throw new ArgumentException("玩家不存在");
			}

			return value;
		}

		public static void SendRefreshGold(this ShopComponent self, Player player)
		{
			if (self.playersGoldDict.TryGetValue(player.Id, out int gold))
			{
				self.SendRefreshGold(player, gold);
			}
			else
			{
				throw new ArgumentException("玩家不存在");
			}
		}

		public static void SendRefreshGold(this ShopComponent self, Player player, int goldCount)
		{
			player.SendMessage(new G2C_RefreshGold()
			{
				GlodCount = goldCount
			});
		}

		public static int GetIdByIndex(this ShopComponent self, Player player, int index)
		{
			if (!self.availableChampionIdArray.TryGetValue(player.Id, out var availableList))
			{
				return -1;
			}

			if (index >= availableList.Count)
			{
				return -1;
			}

			return availableList[index];
		}

		public static bool TryLevelUp(this ShopComponent self, Player player)
		{
			if (!self.playerLevelDict.TryGetValue(player.Id, out int currentLevel))
			{
				return false;
			}

			// 计算升级费用
			int levelUpCost = GetLevelUpCost(currentLevel);

			// 检查金币是否足够
			if (!self.TrySubPlayerGold(player, levelUpCost))
			{
				return false;
			}

			// 升级并更新英雄上限
			currentLevel += 1;
			self.playerLevelDict[player.Id] = currentLevel;
			self.playerCurrentChampionLimit[player.Id] = GPDefine.InitChampionLimit + currentLevel;

			// 计算下一级所需费用
			int nextLevelCost = GetLevelUpCost(currentLevel);

			// 发送更新消息
			player.SendMessage(new G2C_UpdateLevel()
			{
				Level = currentLevel,
				ChampionLimit = self.playerCurrentChampionLimit[player.Id],
				NextLevelCost = nextLevelCost
			});

			return true;
		}

		/// <summary>
		/// 获取升级到下一级所需的金币
		/// </summary>
		public static int GetLevelUpCost(int currentLevel)
		{
			return currentLevel * 2 + 2; // 例如: 1级→2级需要4金币, 2级→3级需要6金币
		}

		public static int GetPlayerLevel(this ShopComponent self, long playerId)
		{
			return self.playerLevelDict.GetValueOrDefault(playerId, 1);
		}

		// GetPlayerChampionMaxLimit	
		public static int GetPlayerChampionMaxLimit(this ShopComponent self, long playerId)
		{
			return self.playerCurrentChampionLimit.GetValueOrDefault(playerId, GPDefine.InitChampionLimit);
		}

		public static void UpgradePlayerHeroLevel(this ShopComponent self, Player player)
		{
			if (!self.playerHeroLevelDict.TryGetValue(player.Id, out int currentLevel))
			{
				return;
			}

			currentLevel += 1;
			self.playerHeroLevelDict[player.Id] = currentLevel;
			self.RefreshShop(player, true);
			List<int> availableChampionIdArray = self.GetAvailableChampionIdArray(player.Id);
			availableChampionIdArray.ForEach(x => Log.Info($"刷新商店 {x}"));

			G2C_UpdateShppChampion message = new G2C_UpdateShppChampion();
			message.championIds = availableChampionIdArray;
			player.SendMessage(message);
		}
	}
}
