using System.Collections.Generic;

namespace ET
{
	[ChildOf(typeof (RoomComponent))]
	public class Room: Entity, IAwake, IDestroy
	{
		/// <summary>
		/// 房主
		/// </summary>
		public Player RoomHolder;

		public string RoomName;

		public GamePlayComponent gamePlayComponent;

		/// <summary>
		/// 这个房间当前包含的玩家，包括房主
		/// </summary>
		public Dictionary<long, Player> ContainsPlayers;

		public bool IsWaiting => this.ContainsPlayers.Count < 2;
	}
}
