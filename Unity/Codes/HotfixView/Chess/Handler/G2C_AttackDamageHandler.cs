using System;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.CpAnimatorComponent))]
	[FriendClassAttribute(typeof (ET.CharacterControlComponent))]
	[FriendClassAttribute(typeof (ET.GamePlayComponent))]
	public class G2C_AttackDamageHandler: AMHandler<G2C_AttackDamage>
	{
		protected override async void Run(Session session, G2C_AttackDamage message)
		{
			await GamePlayComponent.Instance.isViewReadyTask;

			long toId = message.ToId;
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

			CharacterControlComponent controlComponent = fromUnit.GetComponent<CharacterControlComponent>();
			PlayAttack(fromUnit, controlComponent, config).Coroutine();
			await TimerComponent.Instance.WaitAsync(config.attacktime);

			Unit toUnit = unitComponent.Get(toId);
			if (toUnit == null)
			{
				return;
			}

			GameObjectComponent projectileGameObjectComponent = null;
			ProjectileComponent projectileComponent = null;
			if (config.attackProjectile != "")
			{
				GameObject projectileGO = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.attackProjectile);
				projectileGameObjectComponent = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(projectileGO,
				() =>
				{
					GameObjectPoolComponent.Instance.RecycleGameObject(projectileGO);
				});
				projectileComponent = projectileGameObjectComponent.AddComponent<ProjectileComponent, Unit, float>(toUnit, config.projSpeed);
				projectileGO.transform.position = controlComponent.attackPointTs.position;
				
				projectileComponent.StartTrack().Coroutine();
			}

			GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
			CharacterControlComponent characterControlComponent = toUnit.GetComponent<CharacterControlComponent>();

			if (config.attackProjectile != "")
			{
				await projectileComponent.WaitAsync();
				projectileGameObjectComponent.Dispose();
			}

			if (toUnit.IsDisposed)
			{
				return;
			}

			HealBarComponent healBarComponent = characterControlComponent.GetComponent<HealBarComponent>();
			healBarComponent.SetHpRatio(1.0f * message.HP / message.MaxHP);
			FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
			floatTextComponent.Init(toUnit.ViewPosition + new Vector3(0, 2.5f, 0), message.Damage);
			characterControlComponent = toUnit.GetComponent<CharacterControlComponent>();
			characterControlComponent.PlayAnim(AnimDefine.BeHit);
			await TimerComponent.Instance.WaitAsync((long)(characterControlComponent.GetAnimTime(AnimDefine.BeHit) * 1000));
			characterControlComponent.PlayAnim(AnimDefine.Idle, 0.3f);
		}

		public static async ETTask PlayAttack(Unit unit, CharacterControlComponent controlComponent, ChampionConfig config)
		{
			controlComponent.PlayAnim(AnimDefine.Attack);
			await TimerComponent.Instance.WaitAsync(config.attacktime);

			if (!unit.IsDisposed)
			{
				await TimerComponent.Instance.WaitAsync((long)(controlComponent.GetAnimTime(AnimDefine.Attack) * 1000) - config.attacktime);
				controlComponent.PlayAnim(AnimDefine.Idle, 0.3f);
			}
		}
	}
}
