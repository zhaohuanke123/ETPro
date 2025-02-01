using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIBag))]
	public class UIBagOnCreateSystem: OnCreateSystem<UIBag>
	{
		public override async void OnCreate(UIBag self)
		{
			self.BagView = self.AddUIComponent<UILoopGridView>("Panel/Bag");
			self.BtnClose = self.AddUIComponent<UIButton>("Panel/BtnClose");
			self.BtnClose.SetOnClick(self.OnClickBtnClose);

			List<ItemInfo> itemInfos = await ItemHelper.GetItemInfo(self.ZoneScene());
			self.itemInfos = itemInfos;

			self.BagView.InitGridView(itemInfos.Count,
			self.GetItemByIndex,
			new LoopGridViewSettingParam()
			{
				mPadding = new RectOffset()
				{
					left = 20,
					top = 20,
				},
				mItemPadding = new Vector2(20, 20),
				mFixedRowOrColumnCount = 10,
			});
		}
	}

	[ObjectSystem]
	[FriendClass(typeof (UIBag))]
	public class UIBagLoadSystem: LoadSystem<UIBag>
	{
		public override void Load(UIBag self)
		{
			self.BtnClose.SetOnClick(self.OnClickBtnClose);
		}
	}

	[FriendClass(typeof (UIBag))]
	public static class UIBagSystem
	{
		public static void OnClickBtnClose(this UIBag self)
		{
			UIManagerComponent.Instance.CloseWindow(self).Coroutine();
		}

		public static async ETTask OnShow(this UIBag self)
		{
			try
			{
				// 向服务器请求背包数据
				G2C_BagInfo bagInfo = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(new C2G_BagInfo()) as G2C_BagInfo;
				self.RefreshBagItems(bagInfo.Items);
			}
			catch (Exception e)
			{
				Log.Error($"获取背包数据失败: {e}");
			}
		}

		public static void RefreshBagItems(this UIBag self, List<ItemInfo> items)
		{
			self.BagView.SetListItemCount(items.Count);
			self.BagView.RefreshAllShownItem();
		}

		public static LoopGridViewItem GetItemByIndex(this UIBag self, LoopGridView gridView, int index, int row, int col)
		{
			if (index < 0 || index >= self.itemInfos.Count)
			{
				return null;
			}
			if (row < 0)
				return null;

			LoopGridViewItem item = gridView.NewListViewItem("UIBagItem");

			if (!item.IsInitHandlerCalled)
			{
				item.IsInitHandlerCalled = true;
				self.BagView.AddItemViewComponent<UIBagItem>(item);
			}

			UIBagItem uiBagItem = self.BagView.GetUIItemView<UIBagItem>(item);
			ItemInfo itemData = self.itemInfos[index];
			if (itemData != null)
			{
				uiBagItem.SetData(itemData,index, self.OnItemClick);
			}

			return item;
		}

		public static void OnItemClick(this UIBag self, int index)
		{
			// 处理物品点击事件
			Log.Info($"点击物品: {index}");
		}
	}
}
