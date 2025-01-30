namespace ET
{
	[UIComponent]
	public class UIItem: Entity, IAwake, IDestroy, IOnCreate<int>, IOnEnable
	{
		public UIImage icon;
		public UITextmesh numText;
		public UIButton tryAddBtn;
	}
}
