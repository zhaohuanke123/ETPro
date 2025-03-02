using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (CpAnimatorComponent))]
    [FriendClass(typeof (CharacterControlComponent))]
    [FriendClass(typeof (GamePlayComponent))]
    public class G2C_AttackDamageHandler: AMHandler<G2C_AttackDamage>
    {
        protected override async void Run(Session session, G2C_AttackDamage message)
        {
            long fromId = message.FromId;
            Scene currentScene = session.ZoneScene().CurrentScene();
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit fromUnit = unitComponent.Get(fromId);
            if (fromUnit == null)
            {
                return;
            }

            ChampionConfig config = GamePlayComponent.Instance.GetChampionConfig(fromUnit);
            if (config == null)
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

                BeHit(currentScene, toUnit, controlComponent, message, i).Coroutine();
            }
        }

        public static async ETTask PlayAttack(Unit unit, CharacterControlComponent controlComponent, long attackTime, int isSuper)
        {
            string anim = isSuper == 1? AnimDefine.SAttack : AnimDefine.Attack;
            controlComponent.PlayAnim(anim);

            await TimerComponent.Instance.WaitAsync(attackTime);

            if (!unit.IsDisposed)
            {
                await TimerComponent.Instance.WaitAsync((long)(controlComponent.GetAnimTime(anim) * 1000) - attackTime);
                controlComponent.PlayAnim(AnimDefine.Idle, 0.5f);
            }
        }

        private static async ETTask BeHit(Scene currentScene, Unit unit,
        CharacterControlComponent attackController, G2C_AttackDamage message, int index)
        {
            int hp = message.HPs[index];
            int maxHp = message.MaxHPs[index];
            int Damage = message.Damages[index];
            int skillId = message.SkillId;
            ActiveSkillConfig skillConfig = ActiveSkillConfigCategory.Instance.Get(skillId);

            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
            CharacterControlComponent beHitController = unit.GetComponent<CharacterControlComponent>();

            // 子弹
            if (skillConfig.projectileEffect != "")
            {
                GameObject projectileGO = await GameObjectPoolComponent.Instance.GetGameObjectAsync(skillConfig.projectileEffect);
                GameObjectComponent projectileGameObjectComponent =
                        ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(projectileGO,
                            () => { GameObjectPoolComponent.Instance.RecycleGameObject(projectileGO); });
                ProjectileComponent projectileComponent =
                        projectileGameObjectComponent.AddComponent<ProjectileComponent, Unit, float>(unit, skillConfig.projSpeed);
                projectileGO.transform.position = attackController.attackPointTs.position;

                await projectileComponent.StartTrack();
                projectileGameObjectComponent.Dispose();
            }

            if (skillConfig.hitEffect != "")
            {
                BeHitEffectWait(skillConfig.hitEffect, beHitController.attackPointTs).Coroutine();
            }

            if (unit.IsDisposed)
            {
                return;
            }

            // 血条
            HealBarComponent healBarComponent = beHitController.GetComponent<HealBarComponent>();
            healBarComponent.SetHpRatio(1.0f * hp / maxHp);
            if (hp <= 0)
            {
                healBarComponent.SetVisible(false);
            }

            // 飘字
            FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
            floatTextComponent.Init(unit.ViewPosition + new Vector3(0, 2.5f, 0), Damage, (DamageType)message.DamageTypes[index]);

            // 受机或者死亡
            if (hp <= 0)
            {
                beHitController.PlayAnim(AnimDefine.Dead, 0.5f);
                await TimerComponent.Instance.WaitAsync((long)(beHitController.GetAnimTime(AnimDefine.Dead) * 1000));

                UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
                unitComponent.Remove(unit.Id);
            }
            else
            {
                beHitController.PlayAnim(AnimDefine.BeHit);
                await TimerComponent.Instance.WaitAsync((long)(beHitController.GetAnimTime(AnimDefine.BeHit) * 1000));
                beHitController.PlayAnim(AnimDefine.Idle, 0.3f);
            }
        }

        private static async ETTask BeHitEffectWait(string hitEffect, Transform attackPointTs)
        {
            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(hitEffect);
            go.transform.position = attackPointTs.position;
            await TimerComponent.Instance.WaitAsync(1000);
            GameObjectPoolComponent.Instance.RecycleGameObject(go);
        }
    }
}