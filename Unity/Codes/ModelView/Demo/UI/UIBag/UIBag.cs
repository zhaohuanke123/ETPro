using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UIComponent]
	public class UIBag : Entity, IAwake, ILoad, IOnCreate, IOnEnable
	{
		public static string PrefabPath => "UI/UIBag/Prefabs/UIBag.prefab";
		public UILoopGridView BagView;
		public UIButton BtnClose;
		public List<ItemInfo> itemInfos;
	}
}
