﻿using System;

namespace ET
{
	[UIComponent]
	public class UIMsgBoxWin: Entity, IAwake, IOnCreate, IOnEnable<UIMsgBoxWin.MsgBoxPara>, IOnDisable
	{
		public static string PrefabPath => "UI/UIUpdate/Prefabs/UIMsgBoxWin.prefab";
		public UIText Text;
		public UIButton btn_cancel;
		public UIText CancelText;
		public UIButton btn_confirm;
		public UIText ConfirmText;

		public class MsgBoxPara
		{
			public string Content;
			public string CancelText;
			public string ConfirmText;
			public Action CancelCallback;
			public Action ConfirmCallback;
		}
	}
}
