using System;
using System.Collections.Generic;

namespace ET
{
    public class CpBuffComponentAwakeSystem: AwakeSystem<CpBuffComponent>
    {
        public override void Awake(CpBuffComponent self)
        {
            self.CpBuffMap = new Dictionary<int, CpBuff>();
        }
    }

    public class CpBuffComponentDestroySystem: DestroySystem<CpBuffComponent>
    {
        public override void Destroy(CpBuffComponent self)
        {
            self.CpBuffMap.Clear();
        }
    }

    [FriendClass(typeof (CpBuffComponent))]
    [FriendClassAttribute(typeof (ET.CpBuff))]
    public static class CpBuffComponentSystem
    {
        public static void AddBuff(this CpBuffComponent self, GamePlayComponent gamePlayComponent, Unit target, int buffId)
        {
            if (target == null)
            {
                throw new ArgumentException("target is null");
            }

            if (target.IsDisposed)
            {
                throw new ArgumentException("target is isDisposed");
            }

            CpBuff buff = self.AddChild<CpBuff, int>(buffId);
            bool res = self.CpBuffMap.TryAdd(buffId, buff);
            if (res)
            {
                buff.OnAddBuff(gamePlayComponent, target);
            }
        }

        public static void RemoveBuff(this CpBuffComponent self, GamePlayComponent gamePlayComponent, Unit target, CpBuff buff)
        {
            if (target == null)
            {
                throw new ArgumentException("target is null");
            }

            if (target.IsDisposed)
            {
                throw new ArgumentException("target is isDisposed");
            }

            buff.OnRemoveBuff(gamePlayComponent, target);
            self.CpBuffMap.Remove(buff.ConfigId);
        }

        public static void Tick(this CpBuffComponent self, GamePlayComponent gamePlayComponent)
        {
            foreach (var pair in self.CpBuffMap)
            {
                CpBuff buff = pair.Value;
                buff.time--;
                Unit target = self.GetParent<Unit>();
                buff.DurationTick(gamePlayComponent, target);

                if (buff.time <= 0)
                {
                    self.RemoveBuff(gamePlayComponent, self.GetParent<Unit>(), buff);
                }
            }
        }

        public static bool HasBuff(this CpBuffComponent self, int buffId)
        {
            return self.CpBuffMap.ContainsKey(buffId);
        }
    }
}