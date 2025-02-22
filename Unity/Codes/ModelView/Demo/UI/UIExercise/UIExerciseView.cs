using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[UIComponent]
	public class UIExerciseView : Entity, IAwake, ILoad, IOnCreate, IOnEnable
	{
		public static string PrefabPath => "UI/UIExercise/Prefabs/UIExerciseView.prefab";
		public UIButton RetBtn;
		public UIEmptyGameobject TopList;
		public UILoopListView2 ExcList;

		public UITextmesh ShowExecText;
		public UITextmesh PointText;
		public UIButton SubmitBtn;
	}
}
