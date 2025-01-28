using UnityEngine;

namespace ET
{
	[ChildOf]
	[ComponentOf]
	public class CharacterControlComponent: Entity, IAwake<GameObject>, IDestroy
	{
		public Transform transform;
		public Transform rotateTransform;
	}
}
