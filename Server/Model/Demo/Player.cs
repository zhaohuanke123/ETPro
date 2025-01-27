namespace ET
{
	[ChildOf(typeof (PlayerComponent))]
	public sealed class Player: Entity, IAwake<string>
	{
		public GamePlayComponent gamePlayRoom;
		public Camp camp;
		public string Account { get; set; }

		public long UnitId { get; set; }
		public long RoomId { get; set; }

		public Session Session { get; set; }
	}
}
