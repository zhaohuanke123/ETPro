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
			self.combatQueue = new LinkedList<Unit>();
			self.unitStateDict = new Dictionary<Unit, UnitState>();
#else
			GamePlayComponent.Instance = self;
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
			else if (self.currentGameStage == GameStage.GameOver)
			{
				G2C_ChessGameOver message = new G2C_ChessGameOver();

				long roomId = 0;
				foreach (var kv in self.playerChampionDict)
				{
					Player player = kv.Key;
					List<Unit> units = kv.Value;
					bool isWin = units.Count > 0;

					message.Result = isWin;
					player.SendMessage(message);
					roomId = player.RoomId;
				}

				RoomComponent.Instance.RemoveRoom(roomId);
				self.currentGameStage = GameStage.None;
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

			self.currentGameStage = GameStage.None;
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

			player.RemoveComponent<NumericComponent>();
			NumericComponent numericComponent = player.AddComponent<NumericComponent>();
			numericComponent.Set(NumericType.Hp, 50);
		}

		public static void SetReady(this GamePlayComponent self, Player player, bool isReady)
		{
			if (!self.playerChampionDict.ContainsKey(player))
			{
				throw new ArgumentException("player not in room");
			}

			self.playerReadyDict[player.Id] = isReady;

			NumericComponent numericComponent = player.GetComponent<NumericComponent>();
			player.SendMessage(new G2C_SyncPlayerHp()
			{
				Hp = numericComponent.GetAsInt(NumericType.Hp)
			});
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
			self.ResetCombatGame();
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

				List<ChampionInfo> championInfos = championMapArrayComponent.GetChampionInfos(player);
				foreach (ChampionInfo championInfo in championInfos)
				{
					Unit unit = UnitFactory.CreateChampionUnit(unitComponent, championInfo);
					units.Add(unit);
					self.unitStateDict.Add(unit,
					new UnitState()
					{
						camp = player.camp,
						championInfo = championInfo
					});

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
				if (player.camp == Camp.Player1)
				{
					message.IsPlayer1 = true;
				}
				else
				{
					message.IsPlayer1 = false;
				}

				player.SendMessage(message);
			}
		}

		public static bool CheckBattleEnd(this GamePlayComponent self)
		{
			List<Unit> unit1 = self.player1Units;
			List<Unit> unit2 = self.player2Units;
			UnitComponent unitComponent = self.GetComponent<UnitComponent>();
			for (int i = unit1.Count - 1; i >= 0; i--)
			{
				NumericComponent numericComponent = unit1[i].GetComponent<NumericComponent>();
				if (numericComponent.GetAsInt(NumericType.Hp) <= 0)
				{
					Log.Info($"{unit1[i].Id} 死亡");

					unitComponent.Remove(unit1[i].Id);
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
					Log.Info("战斗结束");

					await TimerComponent.Instance.WaitAsync(1000);

					self.CalAndSendResult();

					return;
				}

				List<ETTask> tasks = new List<ETTask>();
				for (var i = 0; i < unit1.Count; i++)
				{
					Unit unit = unit1[i];
					if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
					{
						continue;
					}
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					tasks.Add(cpCombatComponent.CombatLoop(self, unit, self.player1ChampionInfos[i], unit2));
				}

				await ETTaskHelper.WaitAll(tasks);
				tasks.Clear();

				for (var i = 0; i < unit2.Count; i++)
				{
					Unit unit = unit2[i];
					if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
					{
						continue;
					}
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					tasks.Add(cpCombatComponent.CombatLoop(self, unit, self.player2ChampionInfos[i], unit1));
				}
				await ETTaskHelper.WaitAll(tasks);
				tasks.Clear();

				#region 双方轮流出手的模式

				// var combatQ = self.combatQueue;
				// combatQ.Clear();
				// int i = 0;
				// int j = 0;
				//
				// while (i < unit1.Count && j < unit2.Count)
				// {
				// 	if (RandomHelper.RandomNumber(0, 2) >= 1)
				// 	{
				// 		if (unit1[i].GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				// 		{
				// 			i++;
				// 			continue;
				// 		}
				// 		combatQ.AddLast(unit1[i]);
				// 		i++;
				// 	}
				// 	else
				// 	{
				// 		if (unit2[j].GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				// 		{
				// 			j++;
				// 			continue;
				// 		}
				// 		combatQ.AddLast(unit2[j]);
				// 		j++;
				// 	}
				// }
				//
				// while (i < unit1.Count)
				// {
				// 	if (unit1[i].GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				// 	{
				// 		i++;
				// 		continue;
				// 	}
				// 	combatQ.AddLast(unit1[i]);
				// 	i++;
				// }
				//
				// while (j < unit2.Count)
				// {
				// 	if (unit2[j].GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				// 	{
				// 		j++;
				// 		continue;
				// 	}
				// 	combatQ.AddLast(unit2[j]);
				// 	j++;
				// }
				//
				// foreach (var unit in combatQ)
				// {
				// 	CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				// 	UnitState unitState = self.unitStateDict[unit];
				// 	List<Unit> tarList = unitState.camp == Camp.Player1? unit2 : unit1;
				// 	// await cpCombatComponent.CombatLoop(self, unit, unitState.championInfo, tarList);
				// 	await cpCombatComponent.CombatLoop(self, unit, unitState.championInfo, tarList);
				// }
				//
				// for (var i = 0; i < unit1.Count; i++)
				// {
				// 	Unit unit = unit1[i];
				// 	CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				// 	await cpCombatComponent.CombatLoop(self, unit, self.player1ChampionInfos[i], unit2);
				// }
				// for (var i = 0; i < unit2.Count; i++)
				// {
				// 	Unit unit = unit2[i];
				// 	CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
				// 	await cpCombatComponent.CombatLoop(self, unit, self.player2ChampionInfos[i], unit1);
				// }

				#endregion

				await ETTask.CompletedTask;
			}
		}

		public static void CalAndSendResult(this GamePlayComponent self)
		{
			ShopComponent shopComponent = self.GetComponent<ShopComponent>();
			G2C_OneCpBattleEnd message = new G2C_OneCpBattleEnd();

			self.currentGameStage = GameStage.BeforeGame;
			foreach (var kv in self.playerChampionDict)
			{
				Player player = kv.Key;
				List<Unit> units = kv.Value;

				if (units.Count == 0)
				{
					message.Result = 0;

					NumericComponent numericComponent = player.GetComponent<NumericComponent>();
					int hp = numericComponent.GetAsInt(NumericType.Hp);
					hp -= 3 * self.GetOpponentUnits(player).Count;
					hp = hp < 0? 0 : hp;

					// 根据对面剩余的单位数量减少血量
					numericComponent.Set(NumericType.Hp, hp);

					player.SendMessage(new G2C_SyncPlayerHp()
					{
						Hp = hp
					});
					//  一个玩家的血量小于等于0，游戏结束
					if (hp <= 0)
					{
						self.currentGameStage = GameStage.GameOver;
					}
				}
				// 胜利 的 Player
				else
				{
					message.Result = 1;

					shopComponent.AddPlayerGold(player, 5);
				}

				player.SendMessage(message);
			}
		}

		public static void ResetCombatGame(this GamePlayComponent self)
		{
			self.unitStateDict.Clear();

			UnitComponent unitComponent = self.GetComponent<UnitComponent>();
			foreach (KeyValuePair<Player, List<Unit>> kv in self.playerChampionDict)
			{
				List<Unit> units = kv.Value;
				foreach (Unit unit in units)
				{
					unitComponent.Remove(unit.Id);
				}
				units.Clear();
			}
		}

		public static void Broadcast(this GamePlayComponent self, IMessage message)
		{
			foreach (var player in self.playerChampionDict.Keys)
			{
				player.SendMessage(message);
			}
		}

		public static List<Unit> GetPlayerUnits(this GamePlayComponent self, Player player)
		{
			if (!self.playerChampionDict.TryGetValue(player, out List<Unit> value))
			{
				throw new ArgumentException("玩家不存在");
			}

			return value;
		}

		public static List<Unit> GetOpponentUnits(this GamePlayComponent self, Player player)
		{
			foreach (var kv in self.playerChampionDict)
			{
				if (kv.Key != player)
				{
					return kv.Value;
				}
			}

			throw new ArgumentException("玩家不存在");
		}
	}
#endif
}
