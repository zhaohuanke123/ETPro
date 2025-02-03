using UnityEngine;

namespace ET
{
	public static class AnimDefine
	{
		public const string Idle = "Idle";
		public const string Run = "Run";
		public const string Dead= "Dead";
		public const string BeHit = "BeHit";
		public const string Attack = "Attack";
	}

	[ChildOf]
	[ComponentOf]
	public class CharacterControlComponent: Entity, IAwake<GameObject>, IDestroy, IUpdate
	{
		public Transform transform;
		public Transform rotateTransform;
		public Transform attackPointTs;
		public Transform hitPointTs;
		public UnityPlayableController playableControllerData;
		public PlayableController playableController;
	}
}
