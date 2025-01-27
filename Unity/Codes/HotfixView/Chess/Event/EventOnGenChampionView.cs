using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class EventOnGenChampionView: AEventAsync<EventType.GenChampionView>
	{
		protected override async ETTask Run(EventType.GenChampionView args)
		{
			ChessBattleViewComponent.Instance.HideAllInMap();

			ChampionInfoPB infoPb = args.ChampionInfoPb;

			ChampionConfig config = ChampionConfigCategory.Instance.Get(infoPb.ConfigId);
			GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.prefab);
			GameObjectComponent showView = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(go,
			() =>
			{
				GameObjectPoolComponent.Instance.RecycleGameObject(go);
			});

			Unit unit = args.unit;
			unit.ViewPosition = unit.Position;
			unit.ViewRotation = unit.Rotation;
			unit.AddComponent(showView);
			unit.AddComponent<MoveViewComponent>();
			showView.AddComponent<CpAnimatorComponent>();
			// unit.AddComponent<AnimatorComponent>();

			await ETTask.CompletedTask;
		}
	}
}
