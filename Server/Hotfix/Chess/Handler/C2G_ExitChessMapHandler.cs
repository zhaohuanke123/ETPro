using System;

namespace ET
{
	[MessageHandler]
	public class C2G_ExitChessMapHandler: AMHandler<C2G_ExitChessMap>
	{
		protected override async void Run(Session session, C2G_ExitChessMap message)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			session.RemoveComponent<GamePlayComponent>();

			Room room = RoomComponent.Instance.GetRoom(player.RoomId);
			if (room == null)
			{
				session.Send(new G2C_ExitChessMap());
				return;
			}

			room.Broadcast(new G2C_ExitChessMap());

			await TimerComponent.Instance.WaitAsync(500);

			RoomComponent.Instance.RemoveRoom(room.Id);
		}
	}
}
