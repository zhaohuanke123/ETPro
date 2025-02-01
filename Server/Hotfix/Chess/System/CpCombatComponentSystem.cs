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

		public static async ETTask Attack(this CpCombatComponent self, GamePlayComponent gamePlayComponent, int baseDamage, ChampionConfig config)
		{
			Unit attacker = self.GetParent<Unit>();

			// 计算最终伤害
			int finalDamage = self.CalculateFinalDamage(gamePlayComponent, attacker, baseDamage);

			await DamageHelper.Damage(gamePlayComponent, attacker, self.target, finalDamage, config);
		}

		private static int CalculateFinalDamage(this CpCombatComponent self, GamePlayComponent gamePlayComponent, Unit attacker, int baseDamage)
		{
			NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();

			// 设置基础伤害
			numericComponent.Set(NumericType.ATKBase, baseDamage);

			// 应用羁绊加成
			self.ApplyBonusDamage(gamePlayComponent, attacker);

			return numericComponent.GetAsInt(NumericType.ATK);
		}

		private static void ApplyBonusDamage(this CpCombatComponent self, GamePlayComponent gamePlayComponent, Unit attacker)
		{
			// 获取单位所属阵营
			if (!gamePlayComponent.unitStateDict.TryGetValue(attacker, out UnitState unitState))
			{
				return;
			}

			// 获取对应玩家
			Player attackerPlayer = null;
			foreach (var kv in gamePlayComponent.playerChampionDict)
			{
				if (kv.Key.camp == unitState.camp)
				{
					attackerPlayer = kv.Key;
					break;
				}
			}

			if (attackerPlayer == null)
			{
				return;
			}

			// 获取羁绊组件
			ChampionMapArrayComponent championMapArrayComponent = gamePlayComponent.GetComponent<ChampionMapArrayComponent>();
			BattleChampionBonusComponent battleChampionBonusComponent = championMapArrayComponent.GetComponent<BattleChampionBonusComponent>();

			NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();

			// 重置所有加成值
			numericComponent.SetNoEvent(NumericType.ATKAdd, 0);
			numericComponent.SetNoEvent(NumericType.ATKPct, 0);
			numericComponent.SetNoEvent(NumericType.ATKFinalAdd, 0);
			numericComponent.SetNoEvent(NumericType.ATKFinalPct, 0);

			// 获取激活的羁绊列表并应用加成
			var activeBonusList = battleChampionBonusComponent.GetPlayerActiveBonus(attackerPlayer);
			foreach (var bonus in activeBonusList)
			{
				// 根据羁绊配置应用不同类型的加成
				if (bonus.damageAddBonus > 0)
				{
					numericComponent.SetNoEvent(NumericType.ATKAdd, bonus.damageAddBonus);
				}
				if (bonus.damagePctBonus > 0)
				{
					numericComponent.SetNoEvent(NumericType.ATKPct, bonus.damagePctBonus);
				}
				if (bonus.damageFinalAddBonus > 0)
				{
					numericComponent.SetNoEvent(NumericType.ATKFinalAdd, bonus.damageFinalAddBonus);
				}
				// if (bonus.damageFinalPctBonus > 0)
				// {
				// }
				numericComponent.Set(NumericType.ATKFinalPct, bonus.damageFinalPctBonus);
			}
		}
	}
}
