using MongoDB.Bson.Serialization.Conventions;

namespace ET
{
	[FriendClass(typeof (Player))]
	public static class PlayerSystem
	{
		[ObjectSystem]
		public class PlayerAwakeSystem: AwakeSystem<Player, string>
		{
			public override void Awake(Player self, string a)
			{
				self.Account = a;
			}
		}
		
		public static void SetCamp(this Player self, Camp camp)
		{
			self.camp = camp;
		}

		public static void SendMessage(this Player self, IMessage message)
		{
			if (self.Session != null)
			{
				self.Session.Send(message);
			}
		}
	}
}
