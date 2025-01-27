using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[ChildOf(typeof (ToastComponent))]
	public class UIComfirmDialog: Entity, IAwake, IOnCreate
	{
		public static string PrefabPath => "UI/UIComfirmDialog/Prefabs/UIComfirmDialog.prefab";
		public UITextmesh Title;
		public UIButton ComfirmBtn;
		public UIButton CancelBtn;
		public Action OnConfirm;
		public Action OnCancel;
	}
}
