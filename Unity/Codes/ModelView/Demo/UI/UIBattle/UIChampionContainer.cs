using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIChampionContainer: Entity, IAwake, ILoad, IOnCreate, IOnEnable
    {
        public static string PrefabPath => "UI/UIBattle/Prefabs/UIChampionContainer.prefab";
        public UIButton championBtn;
        public UIImage Image;
        public UIText type1;
        public UIText type2;
        public UIImage icon1;
        public UIImage icon2;
        public UIText Cost;
        public UIText Name;
    }
}