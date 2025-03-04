using UnityEngine;

namespace ET
{
	public static class AnimDefine
	{
		public const string Idle = "Idle";
		public const string Run = "Run";
		public const string Dead = "Dead";
		public const string BeHit = "BeHit";
		public const string Attack = "Attack";
		public const string SAttack = "SAttack";
	}

	[ComponentOf()]
	public class CharacterControlComponent: Entity, IAwake<GameObject>, IDestroy, IUpdate
	{
		public bool isInit = false;
		public Transform transform;
		public Transform rotateTransform;
		public Transform attackPointTs;
		public Transform hitPointTs;
		public UnityPlayableController playableControllerData;
		public PlayableController playableController;
		public string curAnimName;
		public GameObject beControlledGo;
		public HealBarComponent hpBar;
		public HealBarComponent pwBar;
		public ETTask<GameObject> taks;
	}
}
