using System;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpAnimatorComponent))]
	public class G2C_AttackDamageHandler: AMHandler<G2C_AttackDamage>
	{
		protected override async void Run(Session session, G2C_AttackDamage message)
		{
			long toId = message.ToId;
			long fromId = message.FromId;
			Scene currentScene = session.ZoneScene().CurrentScene();
			UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
			Unit fromUnit = unitComponent.Get(fromId);
			// 如果目标单位已经死亡，不执行攻击动画
			if (fromUnit == null)
			{
				return;
			}

			GameObjectComponent gameObjectComponent = fromUnit.GetComponent<GameObjectComponent>();
			CpAnimatorComponent cpAnimatorComponent = gameObjectComponent.GetComponent<CpAnimatorComponent>();
			cpAnimatorComponent.DoAttack(true);

			await TimerComponent.Instance.WaitAsync(message.AttackTime);

			if (!fromUnit.IsDisposed)
			{
				cpAnimatorComponent.DoAttack(false);
			}

			GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
			Unit toUnit = unitComponent.Get(toId);
			if (toUnit == null)
			{
				return;
			}
			CharacterControlComponent characterControlComponent = toUnit.GetComponent<CharacterControlComponent>();
			HealBarComponent healBarComponent = characterControlComponent.GetComponent<HealBarComponent>();
			healBarComponent.SetHpRatio(1.0f * message.HP / message.MaxHP);
			FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
			floatTextComponent.Init(toUnit.ViewPosition + new Vector3(0, 2.5f, 0), message.Damage);
		}
	}
}
