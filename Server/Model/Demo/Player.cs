using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	[ChildOf(typeof (PlayerComponent))]
	public sealed class Player: Entity, IAwake<string>
	{
		[BsonIgnore]
		public GamePlayComponent gamePlayRoom;

		[BsonIgnore]
		public Camp camp;

		public string Account { get; set; }
		public long accountId { get; set; }

		[BsonIgnore]
		public long UnitId { get; set; }

		public long RoomId { get; set; }
		// public long BagId { get; set; }

		[BsonIgnore]
		public Session Session { get; set; }
	}
}
