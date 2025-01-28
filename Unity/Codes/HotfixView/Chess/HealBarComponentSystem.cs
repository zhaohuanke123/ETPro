using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[ObjectSystem]
	public class HealBarComponentAwakeSystem: AwakeSystem<HealBarComponent, Transform>
	{
		public override void Awake(HealBarComponent self, Transform transform)
		{
			self.canvasGroup = transform.GetComponentInChildren<CanvasGroup>();
			self.hpFill = transform.Find("BG/fill").GetComponent<Image>();
			self.SetHpRatio(1f);
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
		public static void SetHpRatio(this HealBarComponent self, float hpRatio)
		{
			self.hpFill.fillAmount = hpRatio;
		}

		public static void SetVisible(this HealBarComponent self, bool visible)
		{
			self.canvasGroup.alpha = visible? 1 : 0;
		}
	}
}
