namespace ET
{
	[UIComponent]
	public class UIHelpWin : Entity, IAwake,IOnCreate,IOnEnable
	{
		public UIButton GalBtn;
		public UIButton SettingBtn;
		public UIButton ReturnBtn;
		// public Scene zoneScene;
		public static string PrefabPath => "UI/UIHelp/Prefabs/UIHelpWin.prefab";
	}
}
