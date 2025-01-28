using System;

namespace ET
{
	[MessageHandler]
	public class C2G_SceneReadyHandler: AMHandler<C2G_SceneReady>
	{
		protected override void Run(Session session, C2G_SceneReady message)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

			if (player == null)
			{
				throw new ArgumentException("player is null");
			}
			GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();

			if (gamePlayComponent == null)
			{

				Room room = RoomComponent.Instance.GetRoom(player.RoomId);
				if (room == null)
				{
					throw new ArgumentException("room is null");
				}
				gamePlayComponent = room.GetComponent<GamePlayComponent>();
			}

			gamePlayComponent.SetReady(player, true);
			Log.Info($"玩家 {player.Id} 准备好了");
		}
	}
}
