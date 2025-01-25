using System;
using System.Collections.Generic;
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

                self.SendSyncTimerToAllPlayer();
            }
            else if (self.currentGameStage == GameStage.Preparation)
            {
                long now = TimeHelper.ServerNow();
                self.timer -= now - self.preTime;
                self.preTime = now;
                // Log.Warning((now - self.preTime).ToString());

                self.SendSyncTimerToAllPlayer();

                if (self.timer < 0)
                {
                    self.currentGameStage = GameStage.Combat;
                }

                self.StartNewBattle();
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
                player.SendMessage(new G2C_SyncTimer() { timer = self.timer });
            }
        }

        public static void StartNewBattle(this GamePlayComponent self)
        {
            ChampionMapArrayComponent championMapArrayComponent = self.GetComponent<ChampionMapArrayComponent>();
            foreach (var kv in championMapArrayComponent.playersGridArray)
            {
                ChampionInfo[,] championInfoMap = kv.Value;
                Player player = PlayerComponent.Instance.Get(kv.Key);
                var units = self.playerChampionDict[player];
                foreach (ChampionInfo championInfo in championInfoMap)
                {
                    if (championInfo == null)
                    {
                        continue;
                    }

                    Unit unit = UnitFactory.CreateChampionUnit(player);
                    units.Add(unit);
                    unit.Position = MapComponent.Instance.GetMapPosition(championInfo.X, championInfo.Z);
                }
            }

            foreach (Player player in self.playerChampionDict.Keys)
            {
                foreach (var units in self.playerChampionDict.Values)
                {
                    
                }
            }
        }
    }
#endif
}