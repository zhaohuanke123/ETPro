using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof (Scene))]
	public class RoomComponent: Entity, IAwake, IDestroy
	{
		public Dictionary<long, Room> Rooms;
		public int RoomIdNum;
		public static RoomComponent Instance;
	}
}
