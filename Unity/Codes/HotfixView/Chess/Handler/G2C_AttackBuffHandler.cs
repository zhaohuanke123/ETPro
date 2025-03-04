using System;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof(ET.CharacterControlComponent))]
    public class G2C_AttackBuffHandler : AMHandler<G2C_AttackBuff>
    {
        protected override async void Run(Session session, G2C_AttackBuff message)
        {
            long unitId = message.ToId;
            int hp = message.HP;
            int maxHp = message.MaxHP;
            int Damage = message.Damage;

            Scene currentScene = session.ZoneScene().CurrentScene();
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.Get(unitId);

            if (unit == null)
            {
                return;
            }

            CharacterControlComponent beHitController = unit.GetComponent<CharacterControlComponent>();
            // 血条
            HealBarComponent healBarComponent = beHitController.hpBar;
            healBarComponent.SetRatio(1.0f * hp / maxHp);
            if (hp <= 0)
            {
                healBarComponent.SetVisible(false);
            }

            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
            // 飘字
            FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
            floatTextComponent.Init(unit.ViewPosition + new Vector3(0, 2.5f, 0), Damage, DamageType.BuffDamage);

            // 受机或者死亡
            if (hp <= 0)
            {
                beHitController.PlayAnim(AnimDefine.Dead, 0.1f);
                await TimerComponent.Instance.WaitAsync((long)(beHitController.GetAnimTime(AnimDefine.Dead) * 1000));

                unitComponent.Remove(unit.Id);
            }
        }
    }
}