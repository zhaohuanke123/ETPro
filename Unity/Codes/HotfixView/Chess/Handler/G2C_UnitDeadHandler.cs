using System;

namespace ET
{
	public class G2C_UnitDeadHandler: AMHandler<G2C_UnitDead>
	{
		protected override void Run(Session session, G2C_UnitDead message)
		{
			long unitId = message.UnitId;
			Scene currentScene = session.ZoneScene().CurrentScene();
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			unitComponent.Remove(unitId);
		}
	}
}
