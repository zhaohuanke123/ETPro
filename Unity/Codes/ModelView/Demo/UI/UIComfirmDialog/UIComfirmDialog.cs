using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class UIComfirmDialog : Entity, IAwake, ILoad, IOnCreate, IOnEnable
	{
		public static string PrefabPath => "UI/UIComfirmDialog/Prefabs/UIComfirmDialog.prefab";
		public UITextmesh Title;
		public UIButton ComfirmBtn;
		public UIButton CancelBtn;
		 

	}
}
