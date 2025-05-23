﻿using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class HealBarComponentAwakeSystem: AwakeSystem<HealBarComponent, Transform>
    {
        public override void Awake(HealBarComponent self, Transform transform)
        {
            self.canvasGroup = transform.GetComponent<CanvasGroup>();
            self.hpFill = transform.Find("HP/Fill").GetComponent<Image>();
            // self.Lv
            // self.Name
            self.SetRatio(1f, true);
            // Camera camera = Camera.main;
            // Canvas canvas = transform.GetComponent<Canvas>();
            // canvas.worldCamera = camera;
        }
    }

    [ObjectSystem]
    public class HealBarComponentDestroySystem: DestroySystem<HealBarComponent>
    {
        public override void Destroy(HealBarComponent self)
        {
        }
    }

    [FriendClass(typeof (HealBarComponent))]
    public static class HealBarComponentSystem
    {
        public static void SetRatio(this HealBarComponent self, float hpRatio, bool isInit = false)
        {
            if (isInit)
            {
                self.hpFill.fillAmount = hpRatio;
            }
            else
            {
                self.hpFill.fillAmount = Mathf.Min(self.hpFill.fillAmount, hpRatio);
            }
        }

        public static void SetVisible(this HealBarComponent self, bool visible)
        {
            self.canvasGroup.alpha = visible? 1 : 0;
        }
    }
}