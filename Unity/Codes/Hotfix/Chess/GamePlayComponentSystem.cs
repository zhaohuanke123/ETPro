using System;
using System.Collections.Generic;
using UnityEngine;
#if SERVER
using MongoDB.Driver.Core.Events;
#endif

namespace ET.Chess
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
				}
			}
			else if (self.currentGameStage == GameStage.Combat)
			{
				List<Unit> unit1 = null;
				List<Unit> unit2 = null;
				foreach (KeyValuePair<Player, List<Unit>> kv in self.playerChampionDict)
				{
					Player player = kv.Key;
					if (player.camp == Camp.Player1)
					{
						unit1 = kv.Value;
					}
					else
					{
						unit2 = kv.Value;
					}
				}

				foreach (Unit unit in unit1)
				{
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					cpCombatComponent.FindTarget(unit2);
				}
				foreach (Unit unit in unit2)
				{
					CpCombatComponent cpCombatComponent = unit.GetComponent<CpCombatComponent>();
					cpCombatComponent.FindTarget(unit1);
				}
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
		}
	}

	[FriendClass(typeof (GamePlayComponent))]
	[FriendClass(typeof (ET.Player))]
	[FriendClassAttribute(typeof (ET.ChampionMapArrayComponent))]
	public static class GamePlayComponentSystemSystem
	{
		public static void AddPlayer(this GamePlayComponent self, Player player)
		{
			ShopComponent shopComponent = self.GetComponent<ShopComponent>();
			ChampionArrayComponent championArrayComponent = self.GetComponent<ChampionArrayComponent>();
			ChampionMapArrayComponent championMapArrayComponent = self.GetComponent<ChampionMapArrayComponent>();
			shopComponent.AddPlayer(player);
			championMapArrayComponent.AddPlayer(player);
			championArrayComponent.AddPlayer(player);

			player.gamePlayRoom = self;
			self.playerChampionDict.Add(player, new List<Unit>());
			// TODO 先默认准备好
			self.playerReadyDict.Add(player.Id, true);
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
					unit.AddComponent<SendUniPosComponent, Player>(player);
					ChampionInfoPB championInfoPb = championInfo.GetChampionInfoPb();
					championInfoPb.Lv = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Lv);
					championInfoPbs.Add(championInfoPb);

					unit.Position = MapComponent.Instance.GetMapPosition(championInfo.X, championInfo.Z);
					unit.Rotation = championMapArrayComponent.GetPlayerChampionRotate(player);
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
	}
#endif
}
