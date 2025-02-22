using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [UIComponent]
	public class UILobbyView : Entity,IAwake,IOnCreate,IOnEnable
    {
        public static string PrefabPath => "UI/UILobby/Prefabs/UILobbyView.prefab";
        public UIButton EnterBtn;
        public UIButton ReturnLoginBtn;
        public UIButton EnterChessMapBtn;
        public UIButton StartMatchBtn;
        public UIItem PointItem;
        public UIButton BtnBag;

        public List<UIButton> galBtns;
        public int nextGalId;
        public Transform galLevelTs;
        public UIButton ExercisesBtn;
    }
}