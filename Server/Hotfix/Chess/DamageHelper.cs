using MongoDB.Driver.Core.Events;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof (ET.CpCombatComponent))]
    [FriendClassAttribute(typeof (ET.GamePlayComponent))]
    [FriendClassAttribute(typeof (ET.Player))]
    public static class DamageHelper
    {
        public static int Damage(GamePlayComponent gamePlayComponent, Unit attacker, Unit target, int damage, ChampionConfig config)
        {
            int finalDamage = CalculateFinalDamage(gamePlayComponent, attacker, damage);
            target.GetComponent<NumericComponent>()
                    .Set(NumericType.Hp, target.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - finalDamage);
            Log.Info($"attacker: {attacker.Id}, target: {target.Id}, damage: {finalDamage}");
            return finalDamage;
        }

        private static int CalculateFinalDamage(GamePlayComponent gamePlayComponent, Unit attacker, int baseDamage)
        {
            NumericComponent numericComponent = attacker.GetComponent<NumericComponent>();

            // 设置基础伤害
            numericComponent.Set(NumericType.ATKBase, baseDamage);

            // 应用羁绊加成
            ApplyBonusDamage(gamePlayComponent, attacker);

            return numericComponent.GetAsInt(NumericType.ATK);
        }

        private static void ApplyBonusDamage(GamePlayComponent gamePlayComponent, Unit attacker)
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
                    numericComponent.SetNoEvent(NumericType.ATKAdd, numericComponent.GetAsInt(NumericType.ATKAdd) + bonus.damageAddBonus);
                }

                if (bonus.damagePctBonus > 0)
                {
                    numericComponent.SetNoEvent(NumericType.ATKPct, numericComponent.GetAsInt(NumericType.ATKPct) + bonus.damagePctBonus);
                }

                if (bonus.damageFinalAddBonus > 0)
                {
                    numericComponent.SetNoEvent(NumericType.ATKFinalAdd,
                        numericComponent.GetAsInt(NumericType.ATKFinalAdd) + bonus.damageFinalAddBonus);
                }

                numericComponent.Set(NumericType.ATKFinalPct, numericComponent.GetAsInt(NumericType.ATKFinalPct) + bonus.damageFinalPctBonus);
            }

            Log.Info($"add : {numericComponent.GetAsInt(NumericType.ATKAdd)}");
            Log.Info($"pct : {numericComponent.GetAsInt(NumericType.ATKPct)}");
            Log.Info($"finalAdd : {numericComponent.GetAsInt(NumericType.ATKFinalAdd)}");
            Log.Info($"finalPct : {numericComponent.GetAsInt(NumericType.ATKFinalPct)}");
        }
    }
}