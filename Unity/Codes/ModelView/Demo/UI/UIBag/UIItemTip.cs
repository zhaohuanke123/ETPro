using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UIComponent]
	public class UIItemTip: Entity, IAwake, ILoad, IOnCreate, IOnEnable, IUpdate
	{
		public static string PrefabPath => "UI/UIBag/Prefabs/UIItemTip.prefab";
		public UITextmesh Desc;
		public UIImage Icon;
		public UITextmesh ItemName;
	}
}
