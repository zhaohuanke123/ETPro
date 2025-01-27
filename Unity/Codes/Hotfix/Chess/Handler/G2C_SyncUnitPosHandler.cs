using System;
using UnityEngine;

namespace ET
{
	[MessageHandler]
	public class G2C_SyncUnitPosHandler: AMHandler<G2C_SyncUnitPos>
	{
		protected override async void Run(Session session, G2C_SyncUnitPos message)
		{
			Scene zoneScene = session.ZoneScene();
			CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
			if (currentScenesComponent.Scene == null)
			{
				Log.Warning("收到同步位置消息, 当前场景为空");
				return;
			}

			UnitComponent unitComponent = currentScenesComponent.Scene.GetComponent<UnitComponent>();
			Unit unit = unitComponent.Get(message.UnitId);
			if (unit == null)
			{
				Log.Warning("收到同步位置消息, unit为空");
				return;
			}

			MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
			Log.Warning("收到同步位置消息:{0}", unit.Position);
			unit.Forward = new Vector3(message.ForwardX, message.ForwardY, message.ForwardZ);
			await moveComponent.MoveToAsync(new Vector3(message.X, message.Y, message.Z), 10);
		}
	}
}
