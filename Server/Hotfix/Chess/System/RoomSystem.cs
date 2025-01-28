using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
	public class RoomAwakeSystem: AwakeSystem<Room>
	{
		public override void Awake(Room self)
		{
			self.ContainsPlayers = new Dictionary<long, Player>();
		}
	}

	public class RoomDestroySystem: DestroySystem<Room>
	{
		public override void Destroy(Room self)
		{

		}
	}

	[FriendClass(typeof (Room))]
	public static class RoomSystem
	{
		public static void LeaveRoom(this Room self, Player player)
		{
			if (!self.ContainsPlayers.ContainsKey(player.Id))
			{
				throw new ArgumentException("玩家不在房间中");
			}

			Log.Info($"玩家 {player.Id} 离开房间 {self.Id}");
			self.ContainsPlayers.Remove(player.Id);
			player.RoomId = 0;
			player.SetCamp(Camp.None);

			if (self.ContainsPlayers.Count != 0)
			{
				Player first = self.ContainsPlayers.Values.First();
				first.SendMessage(new G2C_MatchFail());
			}

			RoomComponent.Instance.RemoveRoom(self.Id);
		}

		public static void JoinRoom(this Room self, Player player)
		{
			if (!self.IsWaiting)
			{
				throw new ArgumentException("房间已满");
			}

			if (!self.ContainsPlayers.TryAdd(player.Id, player))
			{
				throw new ArgumentException("玩家已经在房间中");
			}

			Log.Info($"玩家 {player.Id} 加入房间 {self.Id}");

			player.RoomId = self.Id;
			player.SetCamp(self.RoomHolder == player? Camp.Player1 : Camp.Player2);

			if (self.ContainsPlayers.Count == 2)
			{
				Log.Info($" 房间 {self.Id}  匹配成功");
				GamePlayComponent gamePlayComponent = self.AddComponent<GamePlayComponent>();
				self.gamePlayComponent = gamePlayComponent;

				gamePlayComponent.AddComponent<MapComponent>();
				gamePlayComponent.AddComponent<ShopComponent>();
				gamePlayComponent.AddComponent<ChampionArrayComponent>();
				gamePlayComponent.AddComponent<UnitComponent>();

				ChampionMapArrayComponent championMapArrayComponent = gamePlayComponent.AddComponent<ChampionMapArrayComponent>();
				championMapArrayComponent.AddComponent<BattleChampionBonusComponent>();
				gamePlayComponent.AddComponent<SendUniPosComponent>();
				foreach (Player p in self.ContainsPlayers.Values)
				{
					self.gamePlayComponent.AddPlayer(p);
				}

				self.Broadcast(new G2C_MatchSuccess());
			}
		}

		public static void Broadcast(this Room self, IMessage message)
		{
			foreach (Player player in self.ContainsPlayers.Values)
			{
				player.SendMessage(message);
			}
		}
	}
}
