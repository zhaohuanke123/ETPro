using System;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpAnimatorComponent))]
	[FriendClassAttribute(typeof (ET.CharacterControlComponent))]
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

			CharacterControlComponent controlComponent = fromUnit.GetComponent<CharacterControlComponent>();

			ChampionConfig config = GamePlayComponent.Instance.GetChampionConfig(fromUnit);
			if (config == null)
			{
				return;
			}

			controlComponent.PlayAnim(AnimDefine.Attack);
			await TimerComponent.Instance.WaitAsync(config.attacktime);

			if (!fromUnit.IsDisposed)
			{
				controlComponent.PlayAnim(AnimDefine.Idle, 0.3f);
			}

			Unit toUnit = unitComponent.Get(toId);
			if (toUnit == null)
			{
				return;
			}

			ETTask trackTask = null;
			GameObjectComponent projectileGameObjectComponent = null;
			if (config.attackProjectile != "")
			{
				GameObject projectileGO = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.attackProjectile);
				projectileGameObjectComponent = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(projectileGO,
				() =>
				{
					GameObjectPoolComponent.Instance.RecycleGameObject(projectileGO);
				});
				ProjectileComponent projectileComponent =
						projectileGameObjectComponent.AddComponent<ProjectileComponent, Unit, float>(toUnit, config.projSpeed);
				projectileGO.transform.position = controlComponent.attackPointTs.position;
				trackTask = projectileComponent.StartTrack();
			}

			GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
			CharacterControlComponent characterControlComponent = toUnit.GetComponent<CharacterControlComponent>();
			HealBarComponent healBarComponent = characterControlComponent.GetComponent<HealBarComponent>();

			if (config.attackProjectile != "")
			{
				await trackTask;
				projectileGameObjectComponent.Dispose();
			}

			healBarComponent.SetHpRatio(1.0f * message.HP / message.MaxHP);

			FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
			floatTextComponent.Init(toUnit.ViewPosition + new Vector3(0, 2.5f, 0), message.Damage);

			characterControlComponent = toUnit.GetComponent<CharacterControlComponent>();
			characterControlComponent.PlayAnim(AnimDefine.BeHit);
			await TimerComponent.Instance.WaitAsync((long)(characterControlComponent.GetAnimTime(AnimDefine.BeHit) * 1000));
			characterControlComponent.PlayAnim(AnimDefine.Idle, 0.3f);
		}
	}
}
