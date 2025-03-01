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
	[FriendClass(typeof (GamePlayComponent))]
	[FriendClass(typeof (Player))]
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
				await self.Attack(gamePlayComponent, self.GetParent<Unit>().GetComponent<NumericComponent>().GetAsInt(NumericType.ATK), config);
				self.target = null;
			} // 没有攻击目标，移动到最近的目标
			else if (nearestTarget != null)
			{
				Log.Info($"{selfUnit.Id} 移动到 {nearestTarget.Id}");
				MoveComponent moveComponent = selfUnit.GetComponent<MoveComponent>();
				NumericComponent numericComponent = selfUnit.GetComponent<NumericComponent>();

				// 移动到两个人直线上的攻击范围内
				Vector3 targetPos = CpMoveHelper.GetTargetPos(selfUnit, nearestTarget, config);
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

				if (self.target == null)
				{
					if (Vector3.Distance(selfUnit.Position, unit.Position) <= config.attackRange)
					{
						self.target = unit;
					}
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

			if (self.target != null)
			{
				self.target = nearestTarget;
			}

			return nearestTarget;
		}

		public static async ETTask Attack(this CpCombatComponent self, GamePlayComponent gamePlayComponent, int baseDamage, ChampionConfig config)
		{
			Unit attacker = self.GetParent<Unit>();

			Unit target = self.target;
			int finalDamage = DamageHelper.Damage(attacker, target);

			NumericComponent targetNumericComponent = target.GetComponent<NumericComponent>();
			int hp = targetNumericComponent.GetAsInt(NumericType.Hp);
			long attackTime = config.allAttacktime;

			G2C_AttackDamage message = new G2C_AttackDamage();
			message.FromId = attacker.Id;
			message.ToId = target.Id;
			message.Damage = finalDamage;
			message.AttackTime = attackTime;
			message.HP = hp;
			message.MaxHP = targetNumericComponent.GetAsInt(NumericType.MaxHp);
			gamePlayComponent.Broadcast(message);

			await TimerComponent.Instance.WaitAsync(attackTime);

			if (config.attackProjectile != "")
			{
				float distance = Vector3.Distance(attacker.Position, target.Position);
				float time = distance / config.projSpeed * 1000;
				await TimerComponent.Instance.WaitAsync((long)time);
			}

			if (hp <= 0)
			{
				gamePlayComponent.Broadcast(new G2C_UnitDead()
				{
					UnitId = target.Id
				});
				Log.Info($"{target.Id} is dead");
				CpCombatComponent cpCombatComponent = attacker.GetComponent<CpCombatComponent>();
				cpCombatComponent.target = null;
				await TimerComponent.Instance.WaitAsync(attackTime);
			}
			else
			{
				if (!attacker.IsDisposed)
				{
					Log.Info($"{attacker.Id} attack {target.Id} damage {finalDamage}");
					Log.Info($"{target.Id} hp is {hp}");
				}
			}
		}
	}
}
