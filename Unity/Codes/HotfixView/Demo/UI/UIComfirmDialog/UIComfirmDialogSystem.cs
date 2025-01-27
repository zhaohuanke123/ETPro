using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
namespace ET
{
	[UISystem]
	[FriendClass(typeof(UIComfirmDialog))]
	public class UIComfirmDialogOnCreateSystem : OnCreateSystem<UIComfirmDialog>
	{

		public override void OnCreate(UIComfirmDialog self)
		{
			self.Title = self.AddUIComponent<UITextmesh>("Panel/Title");
			self.ComfirmBtn = self.AddUIComponent<UIButton>("Panel/Comfirm");
			self.CancelBtn = self.AddUIComponent<UIButton>("Panel/Cancel");
			self.ComfirmBtn.SetOnClick(self.OnClickComfirmBtn);
			self.CancelBtn.SetOnClick(self.OnClickCancelBtn);
		}

	}
	[ObjectSystem]
	[FriendClass(typeof(UIComfirmDialog))]
	public class UIComfirmDialogLoadSystem : LoadSystem<UIComfirmDialog>
	{

		public override void Load(UIComfirmDialog self)
		{
			self.ComfirmBtn.SetOnClick(self.OnClickComfirmBtn);
			self.CancelBtn.SetOnClick(self.OnClickCancelBtn);
		}

	}
	[FriendClass(typeof(UIComfirmDialog))]
	public static class UIComfirmDialogSystem
	{
		public static void OnClickComfirmBtn(this UIComfirmDialog self)
		{

		}
		public static void OnClickCancelBtn(this UIComfirmDialog self)
		{

		}
	}

}
