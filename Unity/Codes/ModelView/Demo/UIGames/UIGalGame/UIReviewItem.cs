namespace ET
{
	[UIComponent]
	public class UIReviewItem : Entity,IOnCreate,IOnEnable,IAwake
	{
		public static string PrefabPath => "UIGames/UIGalGame/Prefabs/UIReviewItem.prefab";
		public UITextmesh Title;
		public UITextmesh Content;
		 

	}
}
