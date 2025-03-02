using UnityEngine;
using System;

namespace ET
{
	[ObjectSystem]
	public class ProjectileComponentAwakeSystem: AwakeSystem<ProjectileComponent, Unit, float>
	{
		public override void Awake(ProjectileComponent self, Unit target, float speed)
		{
			self.Target = target;
			self.Speed = speed;
			self.Tcs = ETTask.Create(true);
		}
	}

	[ObjectSystem]
	public class ProjectileComponentDestroySystem: DestroySystem<ProjectileComponent>
	{
		public override void Destroy(ProjectileComponent self)
		{
			self.Target = null;
			self.Tcs = null;
		}
	}

	[FriendClass(typeof (ProjectileComponent))]
	[FriendClass(typeof (CharacterControlComponent))]
	public static class ProjectileComponentSystem
	{
		public static async ETTask StartTrack(this ProjectileComponent self)
		{
			try
			{
				GameObjectComponent gameObjectComponent = self.GetParent<GameObjectComponent>();
				long lastUpdateTime = TimeHelper.ClientNow();
				if (gameObjectComponent == null || self.Target == null)
				{
					self.Tcs.SetResult();
					return;
				}

				Transform ts = gameObjectComponent.GameObject.transform;
				Transform target = self.Target.GetComponent<CharacterControlComponent>().hitPointTs;

				while (true)
				{
					if (self.Target == null || self.Target.IsDisposed)
					{
						break;
					}

					// 获取当前位置和目标位置
					Vector3 currentPos = ts.position;
					Vector3 targetPos = target.position;

					// 计算方向和距离
					Vector3 direction = (targetPos - currentPos).normalized;
					float distance = Vector3.Distance(currentPos, targetPos);

					// 如果距离很近，认为命中
					if (distance <= 0.4f)
					{
						break;
					}

					long now = TimeHelper.ClientNow();
					long delta = now - lastUpdateTime;
					lastUpdateTime = now;
					
					float deltaTime = delta * 0.001f;

					// 更新位置
					float moveDistance = self.Speed * deltaTime; // 假设每帧移动速度
					ts.position = currentPos + direction * moveDistance;

					// 更新朝向
					ts.forward = direction;

					await TimerComponent.Instance.WaitFrameAsync();
				}
			}
			finally
			{
				self.Tcs.SetResult();
			}
		}

		public static ETTask WaitAsync(this ProjectileComponent self)
		{
			return self.Tcs;
		}
	}
}
