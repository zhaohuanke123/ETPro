using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIBonus: Entity, IAwake, ILoad, IOnCreate<int>, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBonus.prefab";
        public UIImage activeBg;
        public UIImage icon;
        public UIText name;
        public UIText count;
        public int index;
    }
}