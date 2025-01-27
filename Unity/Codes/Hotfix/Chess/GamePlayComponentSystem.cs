using System;
using System.Collections.Generic;
using UnityEngine;
#if SERVER
using MongoDB.Driver.Core.Events;
#endif

namespace ET
{
	[ObjectSystem]
	public class GamePlayAwakeSystem: AwakeSystem<GamePlayComponent>
	{
		public override void Awake(GamePlayComponent self)
		{
			self.currentGameStage = GameStage.BeforeGame;
#if SERVER
			self.playerChampionDict = new Dictionary<Player, List<Unit>>();
			self.playerReadyDict = new Dictionary<long, bool>();
#endif
		}
	}

#if SERVER
	[ObjectSystem]
	[FriendClassAttribute(typeof (ET.Player))]
	public class GamePlayFixedUpdateSystem: FixedUpdateSystem<GamePlayComponent>
	{
		public override void FixedUpdate(GamePlayComponent self)
		{
			if (self.currentGameStage == GameStage.BeforeGame)
			{
				if (self.playerReadyDict.Count < 2)
				{
					return;
				}
				
				foreach (bool isReady in self.playerReadyDict.Values)
				{
					if (isReady == false)
					{
						return;
					}
				}

				self.currentGameStage = GameStage.Preparation;
				self.preTime = TimeHelper.ServerNow();
				self.timer = self.PreparationStageDuration;

				self.StartPrepareTimer().Coroutine();
				Log.Info("所有玩家准备完毕，开始准备阶段");
			}
			else if (self.currentGameStage == GameStage.Preparation)
			{
				long now = TimeHelper.ServerNow();
				self.timer -= now - self.preTime;
				self.preTime = now;
				// Log.Warning((now - self.preTime).ToString());

				// self.SendSyncTimerToAllPlayer();

				if (self.timer < 0)
				{
					self.currentGameStage = GameStage.Combat;
					self.StartNewBattle();
					self.CombatLoop().Coroutine();
				}
			}
			else if (self.currentGameStage == GameStage.Combat)
			{

			}
		}
	}

	[ObjectSystem]
	[FriendClass(typeof (ET.Player))]
	public class GamePlayDestroySystem: DestroySystem<GamePlayComponent>
	{
		public override void Destroy(GamePlayComponent self)
		{
			foreach (var player in self.playerChampionDict.Keys)
			{
				player.gamePlayRoom = null;
			}
			self.playerChampionDict.Clear();
			self.playerReadyDict.Clear();
		}
	}

	[FriendClass(typeof (GamePlayComponent))]
	[FriendClass(typeof (ET.Player))]
	[FriendClassAttribute(typeof (ET.ChampionMapArrayComponent))]
	public static class GamePlayComponentSystemSystem
	{
		public static void AddPlayer(this GamePlayComponent self, Player player, bool isReady = false)
		{
			ShopComponent shopComponent = self.GetComponent<ShopComponent>();
			ChampionArrayComponent championArrayComponent = self.GetComponent<ChampionArrayComponent>();
			ChampionMapArrayComponent championMapArrayComponent = self.GetComponent<ChampionMapArrayComponent>();
			shopComponent.AddPlayer(player);
			championMapArrayComponent.AddPlayer(player);
			championArrayComponent.AddPlayer(player);

			player.gamePlayRoom = self;
			self.playerChampionDict.Add(player, new List<Unit>());
			self.playerReadyDict.Add(player.Id, isReady);
		}

		public static void SetReady(this GamePlayComponent self, Player player, bool isReady)
		{
			if (!self.playerChampionDict.ContainsKey(player))
			{
				throw new ArgumentException("player not in room");
			}

			self.playerReadyDict[player.Id] = isReady;
		}

		public static void SendSyncTimerToAllPlayer(this GamePlayComponent self)
		{
			foreach (Player player in self.playerChampionDict.Keys)
			{
				player.SendMessage(new G2C_SyncTimer()
				{
					timer = self.timer
				});
			}
		}

		public static async ETTask StartPrepareTimer(this GamePlayComponent self)
		{
			while (self.timer >= 0)
			{
				self.SendSyncTimerToAllPlayer();
				await TimerComponent.Instance.WaitAsync(300);
			}
		}

		public static void StartNewBattle(this GamePlayComponent self)
		{
			ChampionMapArrayComponent championMapArrayComponent = self.GetComponent<ChampionMapArrayComponent>();

			// 临时AI
			TmpAiChampionComponentComponent tmpAiChampionComponentComponent = self.GetComponent<TmpAiChampionComponentComponent>();
			if (tmpAiChampionComponentComponent != null)
			{
				tmpAiChampionComponentComponent.GenAnAI();
			}

			UnitComponent unitComponent = self.GetComponent<UnitComponent>();

			List<ChampionInfoPB> championInfoPbs = new List<ChampionInfoPB>();

			foreach (KeyValuePair<Player, List<Unit>> kv in self.playerChampionDict)
			{
				Player player = kv.Key;
				List<Unit> units = kv.Value;
				foreach (Unit unit in units)
				{
					unitComponent.Remove(unit.Id);
				}
				units.Clear();
				List<ChampionInfo> championInfos = championMapArrayComponent.GetChampionInfos(player);

				foreach (ChampionInfo championInfo in championInfos)
				{
					Unit unit = UnitFactory.CreateChampionUnit(unitComponent, championInfo);
					units.Add(unit);
					// unit.AddComponent<SendUniPosComponent, Player>(player);
					ChampionInfoPB championInfoPb = championInfo.GetChampionInfoPb();
					championInfoPb.Lv = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Lv);
					championInfoPbs.Add(championInfoPb);

					unit.Position = MapComponent.Instance.GetMapPosition(championInfo.X, championInfo.Z);
					unit.Rotation = championMapArrayComponent.GetPlayerChampionRotate(player);
				}

				if (player.camp == Camp.Player1)
				{
					self.player1Units = units;
					self.player1ChampionInfos = championInfos;
				}
				else
				{
					self.player2Units = units;
					self.player2ChampionInfos = championInfos;
				}
			}

			G2C_CreateCpUnits message = new G2C_CreateCpUnits();
			message.ChampionInfoPBList = championInfoPbs;
			foreach (var units in self.playerChampionDict.Values)
			{
				foreach (Unit unit in units)
				{
					UnitInfo unitInfo = UnitHelper.CreateUnitInfo(unit);
					message.Units.Add(unitInfo);
				}
			}

			foreach (Player player in self.playerChampionDict.Keys)
			{
				player.SendMessage(message);
			}
		}

		public static bool CheckBattleEnd(this GamePlayComponent self)
		{
			List<Unit> unit1 = self.player1Units;
			List<Unit> unit2 = self.player2Units;
			for (int i = unit1.Count - 1; i >= 0; i--)
			{
				NumericComponent numericComponent = unit1[i].GetComponent<NumericComponent>();
				if (numericComponent.GetAsInt(NumericType.Hp) <= 0)
				{
					Log.Info($"{unit1[i].Id} 死亡");
					unit1.RemoveAt(i);
				}
			}
			for (int i = unit2.Count - 1; i >= 0; i--)
			{
				NumericComponent numericComponent = unit2[i].GetComponent<NumericComponent>();
				if (numericComponent.GetAsInt(NumericType.Hp) <= 0)
				{
					Log.Info($"{unit2[i].Id} 死亡");
					unit2.RemoveAt(i);
				}
			}

			return unit1.Count == 0 || unit2.Count == 0;
		}

		public static async ETTask CombatLoop(this GamePlayComponent self)
		{
			List<Unit> unit1 = self.player1Units;
			List<Unit> unit2 = self.player2Units;

			while (true)
			{
				if (self.CheckBattleEnd())
				{
					await TimerComponent.Instance.WaitAsync(1000);
					foreach (var kv in self.playerChampionDict)
					{
						Player player = kv.Key;
						List<Unit> units = kv.Value;
						player.SendMessage(new G2C_OneCpBattleEnd()
						{
							Result = units.Count > 0? 1 : 0
						});
					}
					Log.Info("战斗结束");
					self.currentGameStage = GameStage.BeforeGame;
					return;
				}

				for (var i = 0; i < unit1.Count; i++)
				{
					Unit unit = unit1[i];
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					await cpCombatComponent.CombatLoop(self, unit, self.player1ChampionInfos[i], unit2);
				}
				for (var i = 0; i < unit2.Count; i++)
				{
					Unit unit = unit2[i];
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					await cpCombatComponent.CombatLoop(self, unit, self.player2ChampionInfos[i], unit1);
				}

				await ETTask.CompletedTask;
			}
		}

		public static void Broadcast(this GamePlayComponent self, IMessage message)
		{
			foreach (var player in self.playerChampionDict.Keys)
			{
				player.SendMessage(message);
			}
		}
	}
#endif
}
