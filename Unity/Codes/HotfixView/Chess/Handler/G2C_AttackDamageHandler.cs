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

			CharacterControlComponent controlComponent = fromUnit.GetComponent<CharacterControlComponent>();
			PlayAttack(fromUnit, controlComponent, config).Coroutine();
			await TimerComponent.Instance.WaitAsync(config.attacktime);

			for (int i = 0; i < message.ToIds.Count; i++)
			{
				long toId = message.ToIds[i];
				Unit toUnit = unitComponent.Get(toId);
				if (toUnit == null)
				{
					continue;
				}

				BeHit(currentScene,
				toUnit,
				controlComponent,
				config,
				message.HPs[i],
				message.MaxHPs[i],
				message.Damages[i]).Coroutine();
			}
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

		public static async ETTask BeHit(Scene currentScene, Unit unit, CharacterControlComponent controlComponent, ChampionConfig config, int hp,
		int maxHp, int Damage)
		{
			GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/UI/FloatText.prefab");
			CharacterControlComponent characterControlComponent = unit.GetComponent<CharacterControlComponent>();

			// 子弹
			GameObjectComponent projectileGameObjectComponent = null;
			ProjectileComponent projectileComponent = null;
			if (config.attackProjectile != "")
			{
				GameObject projectileGO = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.attackProjectile);
				projectileGameObjectComponent = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(projectileGO,
				() => { GameObjectPoolComponent.Instance.RecycleGameObject(projectileGO); });
				projectileComponent = projectileGameObjectComponent.AddComponent<ProjectileComponent, Unit, float>(unit, config.projSpeed);
				projectileGO.transform.position = controlComponent.attackPointTs.position;

				projectileComponent.StartTrack().Coroutine();
			}
			if (config.attackProjectile != "")
			{
				await projectileComponent.WaitAsync();
				projectileGameObjectComponent.Dispose();
			}

			if (unit.IsDisposed)
			{
				return;
			}

			// 血条
			HealBarComponent healBarComponent = characterControlComponent.GetComponent<HealBarComponent>();
			healBarComponent.SetHpRatio(1.0f * hp / maxHp);
			if (hp <= 0)
			{
				healBarComponent.SetVisible(false);
			}

			// 飘字
			FloatTextComponent floatTextComponent = currentScene.AddChild<FloatTextComponent, GameObject>(go);
			floatTextComponent.Init(unit.ViewPosition + new Vector3(0, 2.5f, 0), Damage);

			// 受机或者死亡
			characterControlComponent = unit.GetComponent<CharacterControlComponent>();
			if (hp <= 0)
			{
				characterControlComponent.PlayAnim(AnimDefine.Dead, 0.5f);
				await TimerComponent.Instance.WaitAsync((long)(characterControlComponent.GetAnimTime(AnimDefine.Dead) * 1000));

				UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
				unitComponent.Remove(unit.Id);
			}
			else
			{
				characterControlComponent.PlayAnim(AnimDefine.BeHit);
				await TimerComponent.Instance.WaitAsync((long)(characterControlComponent.GetAnimTime(AnimDefine.BeHit) * 1000));
				characterControlComponent.PlayAnim(AnimDefine.Idle, 0.3f);
			}
		}
	}
}
