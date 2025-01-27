using System;

namespace ET
{
	public class G2C_OneCpBattleEndHandler: AMHandler<G2C_OneCpBattleEnd>
	{
		protected override void Run(Session session, G2C_OneCpBattleEnd message)
		{
			Scene currentScene = session.ZoneScene().CurrentScene();
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			unitComponent.RemoveAll();
			Game.EventSystem.Publish(new EventType.OneCpBattleEnd());
		}
	}
}
