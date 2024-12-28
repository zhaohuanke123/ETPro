using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIBattle: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIBattle.prefab";
        public UIText GoldText;
        public UIText CountDownText;
        public UIText HpText;
    }
}