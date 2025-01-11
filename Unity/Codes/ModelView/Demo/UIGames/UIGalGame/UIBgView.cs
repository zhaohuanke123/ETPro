namespace ET
{
	[UIComponent]
	public class UIBgView : Entity,IAwake,IOnCreate,IOnEnable<string>
	{
		public static string PrefabPath => "UIGames/UIGalGame/Prefabs/UIBgView.prefab";

		public UIImage bg;
	}
}
