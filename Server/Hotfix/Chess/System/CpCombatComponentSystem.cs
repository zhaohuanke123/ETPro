using System.Collections.Generic;
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
            if (self.target == null)
            {
                return;
            }

            float distance = Vector3.Distance(self.GetParent<Unit>().Position, self.target.Position);
            if (distance < 1f)
            {
                self.GetParent<Unit>().GetComponent<MoveComponent>().Stop(true);
                return;
            }
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
        public static void FindTarget(this CpCombatComponent self, List<Unit> targets)
        {
            if (self.target != null)
            {
                return;
            }
            
            // foreach 对比距离，找最近的目标
            Unit target = null;
            Unit parent = self.GetParent<Unit>();
            foreach (Unit unit in targets)
            {
                if (target == null)
                {
                    target = unit;
                    continue;
                }

                if (Vector3.Distance(parent.Position, unit.Position) < Vector3.Distance(parent.Position, target.Position))
                {
                    target = unit;
                }
            }

            self.target = target;
            MoveComponent moveComponent = parent.GetComponent<MoveComponent>();
            NumericComponent numericComponent = parent.GetComponent<NumericComponent>();
            moveComponent.MoveToAsync(target.Position, numericComponent.GetAsInt(NumericType.Speed)).Coroutine();
        }
    }
}