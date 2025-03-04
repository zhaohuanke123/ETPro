using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class CharacterControlComponentAwakeSystem: AwakeSystem<CharacterControlComponent, GameObject>
	{
		public override void Awake(CharacterControlComponent self, GameObject gameObject)
		{
			self.transform = gameObject.transform;
			self.rotateTransform = gameObject.transform.Find("Model");
			self.hpBar = self.AddChild<HealBarComponent, Transform>(gameObject.transform.Find("HealthBar"));
			self.pwBar = self.AddChild<HealBarComponent, Transform>(gameObject.transform.Find("PowerBar"));

			ReferenceCollector referenceCollector = gameObject.GetComponent<ReferenceCollector>();
			self.attackPointTs = referenceCollector.Get<GameObject>("attackPoint").transform;
			self.hitPointTs = referenceCollector.Get<GameObject>("hitPoint").transform;
			self.beControlledGo = gameObject.transform.Find("icecube").gameObject;
			self.beControlledGo.SetActive(false);

			self.Init();
			self.PlayAnim(AnimDefine.Idle);
		}
	}

	[ObjectSystem]
	public class CharacterControlComponentUpdateSystem: UpdateSystem<CharacterControlComponent>
	{
		public override void Update(CharacterControlComponent self)
		{
			if (self.IsDisposed)
			{
				return;
			}

			self.playableController.Update(Time.deltaTime);
		}
	}

	[ObjectSystem]
	public class CharacterControlComponentDestroySystem: DestroySystem<CharacterControlComponent>
	{
		public override void Destroy(CharacterControlComponent self)
		{
			self.beControlledGo.SetActive(false);
			self.playableController.OnDestroy();
		}
	}

	[FriendClass(typeof (CharacterControlComponent))]
	public static class CharacterControlComponentSystem
	{
		public static void Init(this CharacterControlComponent self)
		{
			if (self.isInit)
			{
				return;
			}

			self.playableControllerData = self.transform.gameObject.GetComponent<UnityPlayableController>();
			self.playableController = new PlayableController();
			PlayableControllerMgr.Instance.LoadPlayableData(self.playableControllerData.aniaClipLoadDatas,
			self.playableControllerData.animationCollector,
			data => { self.playableController.Init(self.playableControllerData.animator, data); });

			self.isInit = true;
		}

		public static void SetPosition(this CharacterControlComponent self, Vector3 position)
		{
			self.transform.position = position;
		}

		public static void SetRotation(this CharacterControlComponent self, Quaternion rotation)
		{
			self.rotateTransform.rotation = rotation;
		}

		public static void SetScale(this CharacterControlComponent self, Vector3 scale)
		{
			self.rotateTransform.localScale = scale;
		}

		public static void SetScale(this CharacterControlComponent self, float scale)
		{
			self.rotateTransform.localScale = new Vector3(scale, scale, scale);
		}

		public static void PlayAnim(this CharacterControlComponent self, string animName, float fadeTime = 0f)
		{
			if (self.curAnimName == AnimDefine.Dead)
			{
				return;
			}

			self.curAnimName = animName;
			self.Init();
			self.playableController.Play(animName, fadeTime);
		}

		public static float GetAnimTime(this CharacterControlComponent self, string animName)
		{
			self.Init();
			return self.playableController.GetAnimTime(animName);
		}

		public static async ETTask SetControlled(this CharacterControlComponent self, bool isControlled)
		{
			// if (!isControlled)
			// {
			// 	if (self.taks != null)
			// 	{
			// 		self.beControlledGo = await self.taks;
			// 		self.taks = null;
			// 	}
			// 	if (self.beControlledGo == null)
			// 	{
			// 		return;
			// 	}
			//
			// 	GameObjectPoolComponent.Instance.RecycleGameObject(self.beControlledGo);
			// 	self.beControlledGo = null;
			// }
			// else
			// {
			// 	self.taks ??= GameObjectPoolComponent.Instance.GetGameObjectAsync("GameAssets/Chess/Effects/icecube.prefab");
			// 	
			// 	GameObject go = await self.taks;
			// 	self.beControlledGo = go;
			// 	self.taks = null;
			// 	
			// 	go.transform.position = self.transform.position;
			// }
			self.beControlledGo.SetActive(isControlled);
			await ETTask.CompletedTask;
		}
	}
}
