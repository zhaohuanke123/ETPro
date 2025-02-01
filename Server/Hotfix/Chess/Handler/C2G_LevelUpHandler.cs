using System;
using System.Collections.Generic;
using ET;

public class C2G_LevelUpHandler: AMRpcHandler<C2G_LevelUp, G2C_LevelUp>
{
	protected override async ETTask Run(Session session, C2G_LevelUp request, G2C_LevelUp response, Action reply)
	{
		Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

		GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();

		if (gamePlayComponent == null)
		{
			Room room = RoomComponent.Instance.GetRoom(player.RoomId);
			if (room == null)
			{
				response.Error = ErrorCode.ERR_RoomNotFound;
				reply();
				return;
			}

			gamePlayComponent = room.GetComponent<GamePlayComponent>();

		}
		ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();

		if (!shopComponent.TryLevelUp(player))
		{
			response.Error = ErrorCode.GoldNotEnough;
			reply();
			return;
		}

		reply();
		await ETTask.CompletedTask;
	}
}
