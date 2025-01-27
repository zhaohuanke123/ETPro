using System;

namespace ET
{
	public class C2G_LeaveMatchHandler: AMHandler<C2G_LeaveMatch>
	{
		protected override void Run(Session session, C2G_LeaveMatch message)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			Room room = RoomComponent.Instance.GetRoom(player.RoomId);
			
			if (room == null)
			{
				return;
			}
			
			room.LeaveRoom(player);
		}
	}
}
