using System;

namespace ET
{
	public class G2C_RemoveBuffHandler: AMHandler<G2C_RemoveBuff>
	{
		protected override void Run(Session session, G2C_RemoveBuff message)
		{
			Scene currentScene = session.ZoneScene().CurrentScene();
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			long id = message.ToId;
			Unit unit = unitComponent.Get(id);

			if (unit == null || unit.IsDisposed)
			{
				return;
			}

			CharacterControlComponent characterControlComponent = unit.GetComponent<CharacterControlComponent>();
			characterControlComponent.SetControlled(false).Coroutine();
		}
	}
}
