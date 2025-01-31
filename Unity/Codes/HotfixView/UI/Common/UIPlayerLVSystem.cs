using UnityEngine;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIPlayerLV))]
	public class UIPlayerLVOnCreateSystem: OnCreateSystem<UIPlayerLV>
	{
		public override void OnCreate(UIPlayerLV self)
		{
			self.animator = self.GetMonoComponent<Animator>("");
			self.lvText = self.AddUIComponent<UITextmesh>("Content/Label_PlayerLevel");
		}
	}

	[UISystem]
	[FriendClass(typeof (UIPlayerLV))]
	public class UIPlayerLVOnEnableSystem: OnEnableSystem<UIPlayerLV>
	{
		public override void OnEnable(UIPlayerLV self)
		{

		}
	}

	[UISystem]
	public class UIPlayerLVDestroySystem: DestroySystem<UIPlayerLV>
	{
		public override void Destroy(UIPlayerLV self)
		{

		}
	}

	[FriendClass(typeof (UIPlayerLV))]
	public static class UIPlayerLVSystem
	{
		public static void SetLevel(this UIPlayerLV self, int level)
		{
			self.lvText.SetText($"LV{level.ToString()}");
			self.animator.SetTrigger(UIPlayerLV.LevelUp);
		}
	}
}
