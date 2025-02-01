using System;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIItem))]
	public class UIItemOnCreateSystem: OnCreateSystem<UIItem, int>
	{
		public override async void OnCreate(UIItem self, int itemId)
		{
			ItemConfig config = ItemConfigCategory.Instance.Get(itemId);
			self.icon = self.AddUIComponent<UIImage>("Icon");
			self.icon.SetSpritePath(config.Icon).Coroutine();
			self.tryAddBtn = self.AddUIComponent<UIButton>("TryAddBtn");
			self.numText = self.AddUIComponent<UITextmesh>("numText");

			self.itemId = itemId;
			int itemCount = await ItemHelper.GetItemCount(self.ZoneScene(), itemId);
			self.SetItemCount(itemCount);

			self.tryAddBtn.SetOnClickAsync(self.TryAddItem);
		}
	}

	[UISystem]
	[FriendClass(typeof (UIItem))]
	public class UIItemOnEnableSystem: OnEnableSystem<UIItem>
	{
		public override void OnEnable(UIItem self)
		{

		}
	}

	[UISystem]
	public class UIItemDestroySystem: DestroySystem<UIItem>
	{
		public override void Destroy(UIItem self)
		{

		}
	}

	[FriendClass(typeof (UIItem))]
	public static class UIItemSystem
	{
		public static void SetItemCount(this UIItem self, int count)
		{
			self.numText.SetText(count.ToString());
		}

		public static void SetIcon(this UIItem self, string icon)
		{
			self.icon.SetSpritePath(icon).Coroutine();
		}

		public static async ETTask TryAddItem(this UIItem self)
		{
			try
			{
				int newCount = await ItemHelper.AddItem(self.ZoneScene(), self.itemId, 1);
				self.SetItemCount(newCount);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
