﻿using System.Collections.Generic;
using System.Linq;
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
        public static async ETTask CombatLoop(this CpCombatComponent self, GamePlayComponent gamePlayComponent,
        Unit selfUnit, ChampionInfo info, List<Unit> selfUnits, List<Unit> targetUnits)
        {
            ChampionConfig config = info.Config;
            int skillId = selfUnit.GetSkillId(config);
            ActiveSkillConfig activeSkillConfig = ActiveSkillConfigCategory.Instance.Get(skillId);

            Unit nearestTarget = self.FindTarget(targetUnits, activeSkillConfig.attackRange);
            Log.Info($"{selfUnit.Id} 回合");

            // 有攻击目标
            if (self.targetList.Count > 0)
            {
                await self.Attack(gamePlayComponent, config, selfUnits);
            } // 没有攻击目标，移动到最近的目标
            else if (nearestTarget != null)
            {
                Log.Info(
                    $"{selfUnit.Id} 移动到 {nearestTarget.Id} attackRange {activeSkillConfig.attackRange} || dis : {Vector3.Distance(selfUnit.Position, nearestTarget.Position)} ");

                MoveComponent moveComponent = selfUnit.GetComponent<MoveComponent>();
                NumericComponent numericComponent = selfUnit.GetComponent<NumericComponent>();

                // 移动到两个人直线上的攻击范围内
                Vector3 targetPos = CpMoveHelper.GetTargetPos(selfUnit, nearestTarget, activeSkillConfig.attackRange, config.moveRange);
                G2C_SyncUnitPos message = new G2C_SyncUnitPos();
                message.UnitId = selfUnit.Id;
                message.MoveToUnitId = nearestTarget.Id;
                message.SkillId = skillId;
                message.ChampionConfigId = info.Config.Id;
                message.X = targetPos.x;
                message.Y = targetPos.y;
                message.Z = targetPos.z;
                gamePlayComponent.Broadcast(message);
                await moveComponent.MoveToAsync(targetPos, numericComponent.GetAsInt(NumericType.Speed));
            }

            await ETTask.CompletedTask;
        }

        public static Unit FindTarget(this CpCombatComponent self, List<Unit> targets, float attackRange)
        {
            bool IsAlive(Unit unit)
            {
                return !unit.IsDisposed && unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) >= 0;
            }

            Unit selfUnit = self.GetParent<Unit>();
            var validTargets = targets.Where(IsAlive).Select(u => new { Unit = u, Distance = Vector3.Distance(selfUnit.Position, u.Position) });
            var inRangeTargets = validTargets.Where(x => x.Distance <= attackRange).Select(x => x.Unit).ToList();

            self.targetList = inRangeTargets;
            return validTargets.OrderBy(x => x.Distance).FirstOrDefault()?.Unit;
        }

        public static async ETTask Attack(this CpCombatComponent self, GamePlayComponent gamePlayComponent,
        ChampionConfig config, List<Unit> selfUnits)
        {
            Unit attacker = self.GetParent<Unit>();

            int skillId = attacker.GetSkillIdAndHandlerPower(gamePlayComponent, config);
            ActiveSkillConfig skillConfig = ActiveSkillConfigCategory.Instance.Get(skillId);
            SkillType type = (SkillType)skillConfig.SkillType;
            List<Unit> targets = null;

            long attackTime = skillConfig.attacktime * 2;
            if (type == SkillType.Attack)
            {
                targets = new List<Unit>();
                //  攻击 或者 治疗
                G2C_AttackDamage message = new G2C_AttackDamage();
                message.SkillId = skillId;
                message.FromId = attacker.Id;
                message.DamageType = skillConfig.damageType;

                for (int i = 0; i < Mathf.Min(skillConfig.targetNum, self.targetList.Count); i++)
                {
                    Unit target = self.targetList[i];
                    targets.Add(target);
                    NumericComponent targetNumericComponent = target.GetComponent<NumericComponent>();
                    int finalDamage = DamageHelper.Damage(attacker, target, skillConfig.damageType, out bool isCritical);
                    finalDamage *= skillConfig.multiplyValue;

                    targetNumericComponent.Add(NumericType.Hp, -finalDamage);
                    targetNumericComponent.Add(NumericType.Power, ConfigGlobal.AddPowerBeHit);

                    int hp = targetNumericComponent.GetAsInt(NumericType.Hp);
                    message.Damages.Add(finalDamage);
                    message.ToIds.Add(target.Id);
                    message.HPs.Add(hp);
                    message.MaxHPs.Add(targetNumericComponent.GetAsInt(NumericType.MaxHp));
                    message.IsCrits.Add(isCritical);

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
                    Log.Info($"飞行时间 {attackTime}");
                    await TimerComponent.Instance.WaitAsync((long)time / 2 + 700);
                }
            }
            else
            {
                G2C_AttackHeal message = new G2C_AttackHeal();
                message.SkillId = skillId;
                message.FromId = attacker.Id;
                targets = selfUnits;
                for (int i = 0; i < selfUnits.Count; i++)
                {
                    Unit target = selfUnits[i];
                    NumericComponent numericComponent = target.GetComponent<NumericComponent>();

                    if (numericComponent == null || numericComponent.GetAsInt(NumericType.Hp) <= 0)
                    {
                        continue;
                    }

                    int finalDamage = DamageHelper.DamageHeal(attacker, target);
                    finalDamage *= skillConfig.multiplyValue;
                    message.Damages.Add(finalDamage);
                    message.ToIds.Add(target.Id);

                    int hp = numericComponent.GetAsInt(NumericType.Hp);
                    int maxHp = numericComponent.GetAsInt(NumericType.MaxHp);
                    hp = Mathf.Min(hp + finalDamage, maxHp);
                    numericComponent.Set(NumericType.Hp, hp);

                    message.HPs.Add(hp);
                    message.MaxHPs.Add(maxHp);

                    Log.Info($"治疗时间 {attackTime}");
                }

                gamePlayComponent.Broadcast(message);
                await TimerComponent.Instance.WaitAsync(attackTime);
            }

            Log.Info("技能结束");

            if (skillConfig.addBuffs != null)
            {
                Log.Info("开始加BUff");
                for (var i = 0; i < skillConfig.addBuffs.Length; i++)
                {
                    int buffId = skillConfig.addBuffs[i];
                    BuffConfig buffConfig = BuffConfigCategory.Instance.Get(buffId);
                    CpBuffType buffConfigTarget = (CpBuffType)buffConfig.target;
                    if (buffConfigTarget == CpBuffType.Self)
                    {
                        Unit unit = self.GetParent<Unit>();
                        CpBuffComponent cpBuffComponent = unit.GetComponent<CpBuffComponent>();
                        cpBuffComponent.AddBuff(gamePlayComponent, unit, buffId);
                    }
                    else
                    {
                        foreach (Unit target in targets)
                        {
                            CpBuffComponent cpBuffComponent = target.GetComponent<CpBuffComponent>();
                            cpBuffComponent.AddBuff(gamePlayComponent, target, buffId);
                        }
                    }
                }
            }

            await ETTask.CompletedTask;
        }

        private static int GetSkillIdAndHandlerPower(this Unit self, GamePlayComponent gamePlayComponent, ChampionConfig config)
        {
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            int power = numericComponent.GetAsInt(NumericType.Power);
            int skillId = 0;
            if (power >= ConfigGlobal.MaxPower)
            {
                skillId = config.superId;
                power = 0;
                Log.Info($"发动大招技能 {skillId}");
            }
            else
            {
                skillId = config.normId;
                power += ConfigGlobal.AddPower;
                Log.Info($"发动技能 {skillId}");
            }

            numericComponent.Set(NumericType.Power, power);
            G2C_SyncPower message = new G2C_SyncPower();
            message.Power = power;
            message.ToId = self.Id;
            gamePlayComponent.Broadcast(message);

            return skillId;
        }

        public static int GetSkillId(this Unit self, ChampionConfig config)
        {
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            int power = numericComponent.GetAsInt(NumericType.Power);
            int skillId = 0;
            skillId = power >= ConfigGlobal.MaxPower? config.superId : config.normId;

            return skillId;
        }

        private static bool IsDead(this Unit self)
        {
            return self.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) <= 0;
        }
    }
}