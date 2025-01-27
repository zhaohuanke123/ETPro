using System;
using System.Collections.Generic;
using ET.EventType;

namespace ET
{
	public class G2C_CreateCpUnitsHandler: AMHandler<G2C_CreateCpUnits>
	{
		protected override void Run(Session session, G2C_CreateCpUnits message)
		{
			Scene currentScene = session.ZoneScene().CurrentScene();
			List<UnitInfo> units = message.Units;
			for (int i = 0; i < units.Count; i++)
			{
				UnitInfo unitInfo = units[i];
				Unit unit = UnitFactory.Create(currentScene, unitInfo);
				EventSystem.Instance.PublishAsync(new GenChampionView()
				{
					unit = unit,
					ChampionInfoPb = message.ChampionInfoPBList[i],
				}).Coroutine();
			}
		}
	}
}
