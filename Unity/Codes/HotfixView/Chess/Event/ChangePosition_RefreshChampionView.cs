using ET.EventType;
using UnityEngine;

namespace ET
{
	public class ChangePosition_RefreshChampionView: AEvent<CPChangePosition>
	{
		protected override void Run(CPChangePosition args)
		{
			CharacterControlComponent characterControlComponent = args.Unit.GetComponent<CharacterControlComponent>();
			if (characterControlComponent != null)
			{
				characterControlComponent.SetPosition(args.Unit.ViewPosition);
				return;
			}

			GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();

			if (gameObjectComponent != null)
			{
				Transform transform = gameObjectComponent.GameObject.transform;
				transform.position = args.Unit.ViewPosition;
			}
		}
	}
}
