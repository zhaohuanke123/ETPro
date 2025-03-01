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
	public static class CpCombatComponentSystem
	{
		public static async ETTask CombatLoop(this CpCombatComponent self, GamePlayComponent gamePlayComponent, Unit selfUnit, ChampionInfo info,
		List<Unit> targets)
		{
			ChampionConfig config = info.Config;
			Unit nearestTarget = self.FindTarget(targets, config);
			Log.Info($"{selfUnit.Id} 回合");

			// 有攻击目标
			if (self.targetList.Count > 0)
			{
				await self.Attack(gamePlayComponent, config);
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
			self.targetList.Clear();

			foreach (Unit unit in targets)
			{
				if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0)
				{
					continue;
				}

				float distance = Vector3.Distance(selfUnit.Position, unit.Position);
				if (distance <= config.attackRange)
				{
					self.targetList.Add(unit);
				}

				if (nearestTarget == null)
				{
					nearestTarget = unit;
					continue;
				}

				if (distance < Vector3.Distance(selfUnit.Position, nearestTarget.Position))
				{
					nearestTarget = unit;
				}
			}

			return nearestTarget;
		}

		public static async ETTask Attack(this CpCombatComponent self, GamePlayComponent gamePlayComponent, ChampionConfig config)
		{
			Unit attacker = self.GetParent<Unit>();

			int skillId = attacker.GetSkillIdAndHandlerPower(config);
			ActiveSkillConfig skillConfig = ActiveSkillConfigCategory.Instance.Get(skillId);
			SkillType type = (SkillType)skillConfig.SkillType;

			//  攻击 或者 治疗
			G2C_AttackDamage message = new G2C_AttackDamage();
			long attackTime = skillConfig.allAttacktime;
			if (type == SkillType.Attack)
			{
				message.FromId = attacker.Id;

				for (int i = 0; i < Mathf.Min(skillConfig.targetNum, self.targetList.Count); i++)
				{
					Unit target = self.targetList[i];
					NumericComponent targetNumericComponent = target.GetComponent<NumericComponent>();
					int finalDamage = DamageHelper.Damage(attacker, target);

					targetNumericComponent.Add(NumericType.Hp, -finalDamage);
					targetNumericComponent.Add(NumericType.Power, ConfigGlobal.AddPowerBeHit);

					int hp = targetNumericComponent.GetAsInt(NumericType.Hp);
					message.Damages.Add(finalDamage);
					message.ToIds.Add(target.Id);
					message.HPs.Add(hp);
					message.MaxHPs.Add(targetNumericComponent.GetAsInt(NumericType.MaxHp));

					Log.Info($" {attacker.Id}, target: {target.Id}, damage: {finalDamage} targeHp : {hp}");
				}

				message.AttackTime = attackTime;
				gamePlayComponent.Broadcast(message);

				Log.Info($"攻击时间 {attackTime}");
				await TimerComponent.Instance.WaitAsync(attackTime);

				if (skillConfig.projectileEffect != "")
				{
					float distance = Vector3.Distance(attacker.Position, self.targetList[0].Position);
					float time = distance / skillConfig.projSpeed * 1000;
					await TimerComponent.Instance.WaitAsync((long)time + 500);
				}

				// gamePlayComponent.Broadcast(new G2C_UnitDead()
				// {
				// 	UnitId = target.Id
				// });
				//
				// Log.Info($"{target.Id} is dead");
				//
				// CpCombatComponent cpCombatComponent = attacker.GetComponent<CpCombatComponent>();
				// cpCombatComponent.nearesrTarget = null;
				// await TimerComponent.Instance.WaitAsync(attackTime);
			}
			else
			{

			}
		}

		private static int GetSkillIdAndHandlerPower(this Unit self, ChampionConfig config)
		{
			NumericComponent numericComponent = self.GetComponent<NumericComponent>();
			int power = numericComponent.GetAsInt(NumericType.Power);
			int skillId = 0;
			if (power >= ConfigGlobal.MaxPower)
			{
				skillId = config.superId;
				power -= ConfigGlobal.MaxPower;
				Log.Info($"发动大招技能 {skillId}");
			}
			else
			{
				skillId = config.normId;
				power += ConfigGlobal.AddPower;
				Log.Info($"发动技能 {skillId}");
			}
			numericComponent.Set(NumericType.Speed, power);

			return skillId;
		}

		public static bool IsDead(this CpCombatComponent self)
		{
			return self.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0;
		}

		private static bool IsDead(this Unit self)
		{
			return self.GetComponent<CpCombatComponent>().GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0;
		}
	}
}
