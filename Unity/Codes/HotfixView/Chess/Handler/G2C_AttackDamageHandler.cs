using System;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpAnimatorComponent))]
	public class G2C_AttackDamageHandler: AMHandler<G2C_AttackDamage>
	{
		protected override void Run(Session session, G2C_AttackDamage message)
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
			// await TimerComponent.Instance.WaitAsync(message.attackTime);
			// cpAnimatorComponent?.DoAttack(false);
		}
	}
}
