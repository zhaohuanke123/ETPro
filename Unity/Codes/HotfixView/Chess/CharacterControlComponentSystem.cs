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
            self.AddComponent<HealBarComponent, Transform>(gameObject.transform.Find("HealthBar"));

            ReferenceCollector referenceCollector = gameObject.GetComponent<ReferenceCollector>();
            self.attackPointTs = referenceCollector.Get<GameObject>("attackPoint").transform;
            self.hitPointTs = referenceCollector.Get<GameObject>("hitPoint").transform;

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
    }
}