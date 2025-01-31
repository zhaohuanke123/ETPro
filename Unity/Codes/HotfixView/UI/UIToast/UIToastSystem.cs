using UnityEngine;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIToast))]
	public class UIToastOnCreateSystem: OnCreateSystem<UIToast>
	{
		public override void OnCreate(UIToast self)
		{
			self.Text = self.AddUIComponent<UITextmesh>("Content/Text/Label_Action");
			self.Content = self.AddUIComponent<UITransform>("Content");
			self.animator = self.GetMonoComponent<Animator>("");
		}
	}

	[UISystem]
	[FriendClass(typeof (UIToast))]
	public class UIToastOnEnableSystem: OnEnableSystem<UIToast, string>
	{
		public override void OnEnable(UIToast self, string param1)
		{
			self.Text.SetText(param1);
			self.animator.SetBool(UIToast.Active, true);
		}
	}

	[UISystem]
	[FriendClass(typeof (UIToast))]
	public class UIToastOnDisableSystem: OnDisableSystem<UIToast>
	{
		public override void OnDisable(UIToast self)
		{
			self.animator.SetBool(UIToast.Active, false);
		}
	}
}
