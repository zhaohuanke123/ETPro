using UnityEngine;

namespace ET
{
	public class CPChangeRotate_RefreshChampionView: AEvent<EventType.CPChangeRotate>
	{
		protected override void Run(EventType.CPChangeRotate args)
		{
			GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
			if (gameObjectComponent == null)
			{
				return;
			}

			Transform transform = gameObjectComponent.GameObject.transform;
			transform.rotation = args.Unit.ViewRotation;
		}
	}
}
