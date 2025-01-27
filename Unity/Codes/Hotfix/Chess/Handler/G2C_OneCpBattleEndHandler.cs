using System;

namespace ET
{
	public class G2C_OneCpBattleEndHandler: AMHandler<G2C_OneCpBattleEnd>
	{
		protected override void Run(Session session, G2C_OneCpBattleEnd message)
		{
			Game.EventSystem.PublishAsync(new UIEventType.ShowToast()
			{
				Text = "战斗" + (message.Result == 1? "胜利" : "失败")
			}).Coroutine();
			Scene currentScene = session.ZoneScene().CurrentScene();
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			unitComponent.RemoveAll();
			Game.EventSystem.Publish(new EventType.OneCpBattleEnd());
		}
	}
}
