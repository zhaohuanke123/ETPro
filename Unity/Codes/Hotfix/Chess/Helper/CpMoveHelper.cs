using UnityEngine;

namespace ET
{
    public static class CpMoveHelper
    {
        public static Vector3 GetTargetPos(Unit moveUnit, Unit targetUnit, ChampionConfig config)
        {
            Vector3 direction = targetUnit.Position - moveUnit.Position;
            float distance = direction.magnitude;
            float moveDistance = Mathf.Min(distance - config.attackRange * 2 / 3, config.moveRange);

            // 如果当前距离小于等于攻击范围，不需要移动
            if (distance < config.attackRange)
            {
                return moveUnit.Position + direction.normalized * moveDistance;
            }

            // 如果移动距离可以覆盖剩余距离
            if (moveDistance + config.attackRange >= distance)
            {
                return targetUnit.Position - direction.normalized * (config.attackRange * 2 / 3);
            }

            // 按最大移动范围移动
            return moveUnit.Position + direction.normalized * config.moveRange;
        }
    }
}