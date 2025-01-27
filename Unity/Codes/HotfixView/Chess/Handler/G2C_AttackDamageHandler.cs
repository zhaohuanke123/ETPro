using System;

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
			Unit unit = unitComponent.Get(toId);

			Unit fromUnit = unitComponent.Get(fromId);

			if (message.IsDead)
			{
				unit.Dispose();
			}

			GameObjectComponent gameObjectComponent = fromUnit.GetComponent<GameObjectComponent>();
			CpAnimatorComponent cpAnimatorComponent = gameObjectComponent.GetComponent<CpAnimatorComponent>();
			cpAnimatorComponent.DoAttack(true);
			await TimerComponent.Instance.WaitAsync(message.attackTime);
			cpAnimatorComponent?.DoAttack(false);
		}
	}
}
