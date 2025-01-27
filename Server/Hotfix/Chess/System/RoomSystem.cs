using System;
using System.Collections.Generic;

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

			self.ContainsPlayers.Remove(player.Id);
			player.RoomId = 0;
			player.SetCamp(Camp.None);

			if (self.ContainsPlayers.Count == 0)
			{
				self.Dispose();
			}
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

			player.RoomId = self.Id;
			player.SetCamp(self.RoomHolder == player? Camp.Player1 : Camp.Player2);

			if (self.ContainsPlayers.Count == 2)
			{
				self.gamePlayComponent = self.AddComponent<GamePlayComponent>();
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
