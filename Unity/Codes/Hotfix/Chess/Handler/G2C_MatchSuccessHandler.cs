using System;
using ET.LoginUI;

namespace ET
{
	public class G2C_MatchSuccessHandler: AMHandler<G2C_MatchSuccess>
	{
		protected override async void Run(Session session, G2C_MatchSuccess message)
		{
			await Game.EventSystem.PublishAsync(new UIEventType.CloseDialog());
			await EnterMapHelper.EnterMatchChessMapAsync(session.ZoneScene());

			// MessageBoxRes messageBoxRes = await MessageBox.Show(session.ZoneScene(), "匹配成功");
			// if (messageBoxRes == MessageBoxRes.Cancel)
			// {
			// 	// MatchHelper.LevelMatch(session.ZoneScene()).Coroutine();
			// }
			// else
			// {
			// 	MatchHelper.LevelMatch(session.ZoneScene()).Coroutine();
			// }
			// Game.EventSystem.PublishAsync(new UIEventType.ShowDialog()
			// {
			// 	Text = "匹配成功. 是否进入匹配棋盘?",
			// 	OnConfirm = () =>
			// 	{
			// 	},
			// 	OnCancel = () =>
			// 	{
			// 	}
			// }).Coroutine();
		}
	}
}
