using System.Collections.Generic;

namespace ET
{
	[ObjectSystem]
	public class RoomComponentAwakeSystem: AwakeSystem<RoomComponent>
	{
		public override void Awake(RoomComponent self)
		{
			RoomComponent.Instance = self;
			self.Rooms = new Dictionary<long, Room>();
		}
	}

	[ObjectSystem]
	public class RoomComponentDestroySystem: DestroySystem<RoomComponent>
	{
		public override void Destroy(RoomComponent self)
		{

		}
	}

    [FriendClass(typeof(RoomComponent))]
    [FriendClassAttribute(typeof(ET.Room))]
    public static class RoomComponentSystem
    {
        public static Room CreateRoom(this RoomComponent self, Player player)
        {
            Room room = self.AddChildWithId<Room>(IdGenerater.Instance.GenerateId());
            self.Rooms.Add(room.Id, room);
            room.RoomHolder = player;

            return room;
        }

        public static Room GetWaitingRoom(this RoomComponent self)
        {
            foreach (Room room in self.Rooms.Values)
            {
                if (room.IsWaiting)
                {
                    return room;
                }
            }
            return null;
        }

        public static Room GetRoom(this RoomComponent self, long id)
        {
            //Log.Warning($"请求的Room Id不存在 ： {id}");  干掉报错
            return self.Rooms.GetValueOrDefault(id);
        }

        public static void RemoveRoom(this RoomComponent self, long id)
        {
            if (self.Rooms.TryGetValue(id, out Room room))
            {
                room.Dispose();
                self.Rooms.Remove(id);
            }
        }
    }
}
