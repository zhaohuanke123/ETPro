using System;

namespace ET
{
	[MessageHandler]
	public class G2C_ExitChessMapHandler: AMHandler<G2C_ExitChessMap>
	{
		protected override async void Run(Session session, G2C_ExitChessMap message)
		{
			await SceneChangeHelper.SceneChangeToLogin(session.ZoneScene());
		}
	}
}
