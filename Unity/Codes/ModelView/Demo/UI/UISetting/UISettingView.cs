namespace ET
{
	[UIComponent]
	public class UISettingView : Entity, IAwake, ILoad, IOnCreate, IOnEnable
	{
		public static string PrefabPath => "UI/UISetting/Prefabs/UISettingView.prefab";
		public UIToggle Chinese;
		public UIToggle English;
		public UIButton BackBtn;
		 

	}
}
