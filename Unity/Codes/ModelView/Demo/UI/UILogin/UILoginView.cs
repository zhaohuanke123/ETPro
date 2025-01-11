using UnityEngine;

namespace ET
{
	[UIComponent]
	public class UILoginView: Entity,IAwake,IOnCreate,IOnEnable<Scene>
	{
		public UIButton loginBtn;
		public UIInputTextmesh password;
		public UIInputTextmesh account;
		public UIInputTextmesh ipaddr;
		public UIButton registerBtn;
		public UILoopListView2 settingView;
		public Scene scene;
		public GameObject GMPanel; 

        public static string PrefabPath => "UI/UILogin/Prefabs/UILoginView.prefab";

	}
}
