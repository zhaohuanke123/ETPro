using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIBagItem))]
	public class UIBagItemOnCreateSystem: OnCreateSystem<UIBagItem>
	{
		public override void OnCreate(UIBagItem self)
		{
			self.ItemIcon = self.AddUIComponent<UIImage>("ICON");
			// self.ItemName = self.AddUIComponent<UIText>("Name");
			self.ItemCount = self.AddUIComponent<UITextmesh>("Label_AmmoCount");
			self.Btn = self.AddUIComponent<UIButton>("SPR_Background");
			self.Btn.SetOnClickAsync(self.OnClick);
		}
	}

	[FriendClass(typeof (UIBagItem))]
	public static class UIBagItemSystem
	{
		public static void SetData(this UIBagItem self, ItemInfo itemInfo, int index, Action<int> onClick)
		{
			self.index = index;
			self.itemId = itemInfo.ItemId;
			self.OnClickCallback = onClick;

			ItemConfig config = ItemConfigCategory.Instance.Get(itemInfo.ItemId);

			self.ItemIcon.SetSpritePath(config.Icon).Coroutine();
			self.ItemCount.SetText(itemInfo.Count.ToString());
		}

		public static async ETTask OnClick(this UIBagItem self)
		{
			self.OnClickCallback?.Invoke(self.index);
			UIItemTip ui = await UIManagerComponent.Instance.OpenWindow<UIItemTip>(UIItemTip.PrefabPath);
			ui.SetItemTip(self.itemId);
		}
	}
}
