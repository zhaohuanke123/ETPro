using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class CpAnimatorComponentAwakeSystem: AwakeSystem<CpAnimatorComponent>
	{
		public override void Awake(CpAnimatorComponent self)
		{
			GameObjectComponent gameObjectComponent = self.GetParent<GameObjectComponent>();
			self.animator = gameObjectComponent.GameObject.GetComponentInChildren<Animator>();
			self.transform = gameObjectComponent.GameObject.transform;
		}
	}

	[ObjectSystem]
	public class CpAnimatorComponentDestroySystem: DestroySystem<CpAnimatorComponent>
	{
		public override void Destroy(CpAnimatorComponent self)
		{

		}
	}

	[ObjectSystem]
	public class CpAnimatorComponentUpdateSystem: UpdateSystem<CpAnimatorComponent>
	{
		public override void Update(CpAnimatorComponent self)
		{
			float movementSpeed = (self.transform.position - self.lastFramePosition).magnitude / Time.deltaTime;

			self.animator.SetFloat(CpAnimatorComponent.MovementSpeedHash, movementSpeed);

			if (Mathf.Approximately(movementSpeed, 0))
			{
				self.animator.speed = 1;
			}
			else
			{
				self.animator.speed = movementSpeed / 2;
			}

			self.lastFramePosition = self.transform.position;
		}
	}

	[FriendClass(typeof (CpAnimatorComponent))]
	public static class CpAnimatorComponentSystem
	{
		public static void DoAttack(this CpAnimatorComponent self, bool b)
		{
			self.animator.SetBool(CpAnimatorComponent.IsAttackingHash, b);
		}
	}
}
