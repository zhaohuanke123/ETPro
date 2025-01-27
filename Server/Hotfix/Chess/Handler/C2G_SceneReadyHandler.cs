using System;

namespace ET
{
	[MessageHandler]
	public class C2G_SceneReadyHandler: AMHandler<C2G_SceneReady>
	{
		protected override void Run(Session session, C2G_SceneReady message)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
			gamePlayComponent.SetReady(player, true);
		}
	}
}
