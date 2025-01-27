using System;

namespace ET
{
	public class C2G_ExitChessMapHandler: AMHandler<C2G_ExitChessMap>
	{
		protected override void Run(Session session, C2G_ExitChessMap message)
		{
			session.RemoveComponent<GamePlayComponent>();
		}
	}
}
