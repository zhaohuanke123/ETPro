using System;

namespace ET
{
	public class G2C_StartMatchHandler: AMRpcHandler<C2G_StartMatch, G2C_StartMatch>
	{
		protected override async ETTask Run(Session session, C2G_StartMatch request, G2C_StartMatch response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

			Room room = RoomComponent.Instance.GetWaitingRoom();
			if (room == null)
			{
				room = RoomComponent.Instance.CreateRoom(player);
			}

			room.JoinRoom(player);

			reply();
			await ETTask.CompletedTask;
		}
	}
}
