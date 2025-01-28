using System;

namespace ET
{
	public class G2C_ChessGameOverHandler: AMHandler<G2C_ChessGameOver>
	{
		protected override void Run(Session session, G2C_ChessGameOver message)
		{
			Scene currentScene = session.ZoneScene().CurrentScene();
			if (currentScene == null)
			{
				return;
			}

			Game.EventSystem.PublishAsync(new UIEventType.ShowDialog()
			{
				Text = $"游戏结束，{(message.Result? "对局胜利" : "对局失败")} 。点击确定返回主界面。",
				OnConfirm = () =>
				{
					SceneChangeHelper.SceneChangeToLogin(session.ZoneScene()).Coroutine();
				}
			}).Coroutine();
		}
	}
}
