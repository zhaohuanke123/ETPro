using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (CharacterControlComponent))]
    public class G2C_AttackHealHandler: AMHandler<G2C_AttackHeal>
    {
        protected override async void Run(Session session, G2C_AttackHeal message)
        {
            Scene currentScene = session.ZoneScene().CurrentScene();
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            long fromId = message.FromId;
            Unit fromUnit = unitComponent.Get(fromId);
            if (fromUnit == null)
            {
                return;
            }

            ActiveSkillConfig activeSkillConfig = ActiveSkillConfigCategory.Instance.Get(message.SkillId);
            CharacterControlComponent controlComponent = fromUnit.GetComponent<CharacterControlComponent>();
            PlayerAnimHelper.PlayAttack(controlComponent, activeSkillConfig.attacktime, activeSkillConfig.isSuper).Coroutine();
            await TimerComponent.Instance.WaitAsync(activeSkillConfig.attacktime);

            for (int i = 0; i < message.ToIds.Count; i++)
            {
                long toId = message.ToIds[i];
                Unit toUnit = unitComponent.Get(toId);
                if (toUnit == null)
                {
                    continue;
                }

                Log.Info($"治疗效果 ： {message.ToIds[i]}");
                BeHeal(currentScene, toUnit, message, i).Coroutine();
            }

            await ETTask.CompletedTask;
        }

        // public static async ETTask PlayAttack(Unit unit, CharacterControlComponent controlComponent, long attackTime, int isSuper)
        // {
        //     Log.Info("治疗动画播放");
        //     string anim = isSuper == 1? AnimDefine.SAttack : AnimDefine.Attack;
        //     controlComponent.PlayAnim(anim);
        //
        //     await TimerComponent.Instance.WaitAsync(attackTime);
        //
        //     if (!unit.IsDisposed)
        //     {
        //         await TimerComponent.Instance.WaitAsync((long)(controlComponent.GetAnimTime(anim) * 1000) - attackTime);
        //         controlComponent.PlayAnim(AnimDefine.Idle, 0.5f);
        //     }
        // }

        public static async ETTask BeHeal(Scene currentScene, Unit unit, G2C_AttackHeal message, int index)
        {
            int hp = message.HPs[index];
            int maxHp = message.MaxHPs[index];
            int Damage = message.Damages[index];
            int skillId = message.SkillId;
            ActiveSkillConfig skillConfig = ActiveSkillConfigCategory.Instance.Get(skillId);

            // 回血特效
            var healEffectAsync = GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/Effects/Heal Effect.prefab");
            ETTask<GameObject> floatTextGoAsync = GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");

            CharacterControlComponent beHitController = unit.GetComponent<CharacterControlComponent>();
            GameObject floatTextGo = await floatTextGoAsync;
            GameObject HealEffectGo = await healEffectAsync;
            HealEffectGo.transform.position = beHitController.transform.position;

            // 血条
            HealBarComponent healBarComponent = beHitController.hpBar;
            healBarComponent.SetRatio(1.0f * hp / maxHp, true);

            // 飘字
            FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(floatTextGo);
            floatTextComponent.Init(unit.ViewPosition + new Vector3(0, 2.5f, 0), Damage, DamageType.Heal);

            await TimerComponent.Instance.WaitAsync(1000);
            GameObjectPoolComponent.Instance.RecycleGameObject(HealEffectGo);
        }
    }
}