namespace ET
{
	[UIComponent]
	public class UIGalGameHelper : Entity,IAwake,IOnCreate,IOnEnable,IOnDisable
	{
		public static string PrefabPath => "UIGames/UIGalGame/Prefabs/UIGalGameHelper.prefab";

		public KeyListener inputer;
	}
}
