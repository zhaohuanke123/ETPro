using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class CpCombatComponentAwakeSystem: AwakeSystem<CpCombatComponent>
	{
		public override void Awake(CpCombatComponent self)
		{
		}
	}

	[ObjectSystem]
	public class CpCombatComponentFixedUpdateSystem: FixedUpdateSystem<CpCombatComponent>
	{
		public override void FixedUpdate(CpCombatComponent self)
		{
		}
	}

	[ObjectSystem]
	public class CpCombatComponentDestroySystem: DestroySystem<CpCombatComponent>
	{
		public override void Destroy(CpCombatComponent self)
		{
		}
	}

	[FriendClass(typeof (CpCombatComponent))]
	public static partial class CpCombatComponentSystem
	{
		public static async ETTask CombatLoop(this CpCombatComponent self, GamePlayComponent gamePlayComponent, Unit selfUnit, ChampionInfo info,
		List<Unit> targets)
		{
			ChampionConfig config = info.Config;
			Unit nearestTarget = self.FindTarget(targets, config);
			Log.Info($"{selfUnit.Id} 回合");

			// 有攻击目标
			if (self.target != null)
			{
				Log.Info($"{selfUnit.Id} 攻击 {self.target.Id}");
				await self.Attack(gamePlayComponent, config.attacktime);
				self.target = null;
			} // 没有攻击目标，移动到最近的目标
			else if (nearestTarget != null)
			{
				Log.Info($"{selfUnit.Id} 移动到 {nearestTarget.Id}");
				MoveComponent moveComponent = selfUnit.GetComponent<MoveComponent>();
				NumericComponent numericComponent = selfUnit.GetComponent<NumericComponent>();

				// 移动到两个人直线上的攻击范围内
				Vector3 targetPos = nearestTarget.Position - (nearestTarget.Position - selfUnit.Position).normalized * (0.5f * config.attackRange);
				G2C_SyncUnitPos message = new G2C_SyncUnitPos();
				message.UnitId = selfUnit.Id;
				message.MoveToUnitId = nearestTarget.Id;
				message.ChampionConfigId = info.Config.Id;
				message.X = targetPos.x;
				message.Y = targetPos.y;
				message.Z = targetPos.z;
				gamePlayComponent.Broadcast(message);
				await moveComponent.MoveToAsync(targetPos, numericComponent.GetAsInt(NumericType.Speed));
			}

			await ETTask.CompletedTask;
		}

		public static Unit FindTarget(this CpCombatComponent self, List<Unit> targets, ChampionConfig config)
		{
			Unit selfUnit = self.GetParent<Unit>();
			Unit nearestTarget = null;
			foreach (Unit unit in targets)
			{
				if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				{
					continue;
				}

				if (Vector3.Distance(selfUnit.Position, unit.Position) < config.attackRange)
				{
					self.target = unit;
					break;
				}

				if (nearestTarget == null)
				{
					nearestTarget = unit;
					continue;
				}

				if (Vector3.Distance(selfUnit.Position, unit.Position) < Vector3.Distance(selfUnit.Position, nearestTarget.Position))
				{
					nearestTarget = unit;
				}
			}

			return nearestTarget;
		}

		public static async ETTask Attack(this CpCombatComponent self, GamePlayComponent gamePlayComponent, long attackTime)
		{
			await DamageHelper.Damage(gamePlayComponent, self.GetParent<Unit>(), self.target, 5, attackTime);
		}
	}
}
