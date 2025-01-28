using System;

namespace ET
{
	public class G2C_MatchSuccessHandler: AMHandler<G2C_MatchSuccess>
	{
		protected override void Run(Session session, G2C_MatchSuccess message)
		{
			Game.EventSystem.PublishAsync(new UIEventType.CloseDialog()).Coroutine();
			Game.EventSystem.PublishAsync(new UIEventType.ShowDialog()
			{
				Text = "匹配成功. 是否进入匹配棋盘?",
				OnConfirm = () =>
				{
					EnterMapHelper.EnterMatchChessMapAsync(session.ZoneScene()).Coroutine();
				},
				OnCancel = () =>
				{
					MatchHelper.LevelMatch(session.ZoneScene()).Coroutine();
				}
			}).Coroutine();
		}
	}
}
