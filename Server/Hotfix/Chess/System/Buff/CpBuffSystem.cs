using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace ET
{
    public class CpBuffAwakeSystem: AwakeSystem<CpBuff, int>
    {
        public override void Awake(CpBuff self, int configId)
        {
            self.ConfigId = configId;
            self.time = self.Config.duration;
        }
    }

    public class CpBuffDestroySystem: DestroySystem<CpBuff>
    {
        public override void Destroy(CpBuff self)
        {
        }
    }

    [FriendClass(typeof (CpBuff))]
    public static class CpBuffSystem
    {
        public static void OnAddBuff(this CpBuff self, GamePlayComponent gamePlayComponent, Unit target)
        {
            Log.Info($"添加Buff {target.Id}, cofigId : {self.ConfigId}");
            NumericComponent numericComponent = target.GetComponent<NumericComponent>();

            BuffConfig config = self.Config;
            BuffType buffType = self.BuffType;

            if (buffType == BuffType.Attr)
            {
                numericComponent.AddAll(config.attrNames, config.attrValues);
            }
            else if (buffType == BuffType.Control)
            {
                G2C_AddBuff message = new G2C_AddBuff();
                message.ToId = target.Id;
                message.BuffId = self.ConfigId;
                gamePlayComponent.Broadcast(message);
            }
        }

        public static void OnRemoveBuff(this CpBuff self, GamePlayComponent gamePlayComponent, Unit target)
        {
            NumericComponent numericComponent = target.GetComponent<NumericComponent>();

            BuffConfig config = self.Config;
            BuffType buffType = self.BuffType;
            if (buffType == BuffType.Attr)
            {
                numericComponent.SubAll(config.attrNames, config.attrValues);
            }
            else if (buffType == BuffType.Control)
            {
                G2C_RemoveBuff message = new G2C_RemoveBuff();
                message.ToId = target.Id;
                message.BuffId = self.ConfigId;
                gamePlayComponent.Broadcast(message);
            }
        }

        public static void DurationTick(this CpBuff self, GamePlayComponent gamePlayComponent, Unit target)
        {
            BuffConfig config = self.Config;
            BuffType buffType = self.BuffType;
            Log.Info($"BuffTick {target.Id} :buffId: {self.ConfigId}");

            if (buffType == BuffType.Damage)
            {
                // TODO damage 
                NumericComponent numericComponent = target.GetComponent<NumericComponent>();
                int hp = numericComponent.GetAsInt(NumericType.Hp);
                int damage = self.Config.attrValues[0];
                hp -= damage;
                numericComponent.Set(NumericType.Hp, hp);

                G2C_AttackBuff message = new G2C_AttackBuff();
                message.Damage = damage;
                message.ToId = target.Id;
                message.BuffId = self.ConfigId;
                message.HP = hp;
                message.MaxHP = numericComponent.GetAsInt(NumericType.MaxHp);

                gamePlayComponent.Broadcast(message);
            }
        }
    }
}