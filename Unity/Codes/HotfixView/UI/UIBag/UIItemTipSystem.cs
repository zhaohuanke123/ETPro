using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIItemTip))]
	public class UIItemTipOnCreateSystem: OnCreateSystem<UIItemTip>
	{
		public override void OnCreate(UIItemTip self)
		{
			self.Desc = self.AddUIComponent<UITextmesh>("Content/Desc");
			self.Icon = self.AddUIComponent<UIImage>("Content/Icon");
			self.ItemName = self.AddUIComponent<UITextmesh>("Content/ItemName");
		}
	}

	[ObjectSystem]
	[FriendClass(typeof (UIItemTip))]
	public class UIItemTipUpdateSystem: UpdateSystem<UIItemTip>
	{
		public override void Update(UIItemTip self)
		{
			if (Input.GetMouseButtonDown(0))
			{
				self.CloseSelf().Coroutine();
			}
		}
	}

	[FriendClass(typeof (UIItemTip))]
	public static class UIItemTipSystem
	{
		public static void SetItemTip(this UIItemTip self, int itemId)
		{
			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
			self.ItemName.SetText(itemConfig.Name);
			self.Desc.SetText(itemConfig.Description);
			self.Icon.SetSpritePath(itemConfig.Icon).Coroutine();
		}
	}
}
