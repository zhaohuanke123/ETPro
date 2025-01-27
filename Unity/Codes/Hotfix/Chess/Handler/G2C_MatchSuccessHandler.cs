using System;

namespace ET
{
	public class G2C_MatchSuccessHandler: AMHandler<G2C_MatchSuccess>
	{
		protected override async void Run(Session session, G2C_MatchSuccess message)
		{
			await EnterMapHelper.EnterMatchChessMapAsync(session.ZoneScene());

			await ETTask.CompletedTask;
		}
	}
}
