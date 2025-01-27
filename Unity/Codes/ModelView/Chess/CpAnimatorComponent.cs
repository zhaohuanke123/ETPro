using UnityEngine;

namespace ET
{
	[ComponentOf(typeof (GameObjectComponent))]
	public class CpAnimatorComponent: Entity, IAwake, IDestroy, IUpdate
	{
		public static readonly int MovementSpeedHash = Animator.StringToHash("movementSpeed");
		public static readonly int IsAttackingHash = Animator.StringToHash("isAttacking");
		public Animator animator;
		public Vector3 lastFramePosition;
		public Transform transform;
	}
}
