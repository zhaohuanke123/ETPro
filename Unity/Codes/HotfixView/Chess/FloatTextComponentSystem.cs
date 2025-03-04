using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ET
{
    [ObjectSystem]
    public class FloatTextComponentAwakeSystem: AwakeSystem<FloatTextComponent, GameObject>
    {
        public override void Awake(FloatTextComponent self, GameObject gameObject)
        {
            self.tmp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            self.transform = gameObject.transform;
            self.canvasGroup = gameObject.GetComponent<CanvasGroup>();
            self.timer = 0;
        }
    }

    [ObjectSystem]
    public class FloatTextComponentDestroySystem: DestroySystem<FloatTextComponent>
    {
        public override void Destroy(FloatTextComponent self)
        {
            TimerComponent.Instance.Remove(ref self.MoveTimer);
            GameObjectPoolComponent.Instance.RecycleGameObject(self.transform.gameObject);
        }
    }

    [FriendClass(typeof (FloatTextComponent))]
    public static class FloatTextComponentSystem
    {
        [Timer(TimerType.FloatTextMoveTimer)]
        [FriendClass(typeof (FloatTextComponent))]
        public class MoveTimer: ATimer<FloatTextComponent>
        {
            public override void Run(FloatTextComponent self)
            {
                try
                {
                    if (self.IsDisposed)
                    {
                        return;
                    }

                    long lastUpdateTime = self.UpdateTime;
                    self.UpdateTime = TimeHelper.ClientNow();
                    float deltaTime = self.UpdateTime - lastUpdateTime;

                    deltaTime /= 1000f;
                    self.transform.position += self.moveDirection * (self.speed * deltaTime);

                    self.timer += deltaTime;
                    // Log.Info($"timer {self.timer}");
                    float fade = (self.fadeOutTime - self.timer) / self.fadeOutTime;

                    self.canvasGroup.alpha = fade;

                    if (fade <= 0)
                    {
                        self.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"move timer error: {self.Id}\n{e}");
                }
            }
        }

        public static void Init(this FloatTextComponent self, Vector3 startPosition, float v, DamageType damageType, bool isCrit = false)
        {
            self.transform.position = startPosition;
            self.tmp.text = Mathf.Round(v).ToString();
            self.moveDirection = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f)).normalized;
            self.canvasGroup.alpha = 1;

            Color color;
            string colorStr = "";

            if (damageType == DamageType.Heal)
            {
                // 治疗类型无视暴击判断
                colorStr = "#98FB98";
            }
            else if (damageType == DamageType.Normal)
            {
                colorStr = isCrit? "#FF4500" : "#FFFFFF";
            }
            else if (damageType == DamageType.Magic)
            {
                colorStr = isCrit? "#9370DB" : "#6495ED";
            }
            else if (damageType == DamageType.BuffDamage)
            {
                colorStr = "#FFA500";
            }
            else
            {
                Debug.LogWarning($"未知伤害类型：{damageType}");
            }
            
            if (!ColorUtility.TryParseHtmlString(colorStr, out color))
            {
                Debug.LogError($"颜色解析失败：{colorStr}");
                color = Color.magenta; // 用醒目颜色提示错误
            }

            self.tmp.color = color;

            self.MoveTimer = TimerComponent.Instance.NewFrameTimer(TimerType.FloatTextMoveTimer, self);
            self.UpdateTime = TimeHelper.ClientNow();
        }
    }
}