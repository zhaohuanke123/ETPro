﻿using System;

namespace ET
{
	[FriendClassAttribute(typeof (ET.GamePlayComponent))]
	public class G2C_OneCpBattleEndHandler: AMHandler<G2C_OneCpBattleEnd>
	{
		protected override void Run(Session session, G2C_OneCpBattleEnd message)
		{
			Game.EventSystem.PublishAsync(new UIEventType.ShowToast()
			{
				Text = "战斗" + (message.Result == 1? "胜利" : "失败"),
				showTime = 1,
			}).Coroutine();

			Scene currentScene = session.ZoneScene().CurrentScene();
			GamePlayComponent gamePlayComponent = currentScene.GetComponent<GamePlayComponent>();
			gamePlayComponent.currentGameStage = GameStage.Preparation;

			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			unitComponent.RemoveAll();
			Game.EventSystem.Publish(new EventType.OneCpBattleEnd());
		}
	}
}
