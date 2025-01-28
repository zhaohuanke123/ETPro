using UnityEngine;

namespace ET
{
	public class CPChangeRotate_RefreshChampionView: AEvent<EventType.CPChangeRotate>
	{
		protected override void Run(EventType.CPChangeRotate args)
		{
			CharacterControlComponent characterControlComponent = args.Unit.GetComponent<CharacterControlComponent>();
			if (characterControlComponent != null)
			{
				characterControlComponent.SetRotation(args.Unit.ViewRotation);
				return;
			}

			GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
			if (gameObjectComponent != null)
			{
				Transform transform = gameObjectComponent.GameObject.transform;
				transform.rotation = args.Unit.ViewRotation;
			}
		}
	}
}
