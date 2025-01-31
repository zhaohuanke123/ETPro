namespace ET
{
	[UIComponent]
	public partial class UIItem: Entity, IAwake, IDestroy, IOnCreate<int>, IOnEnable
	{
		public int itemId;
		public UIImage icon;
		public UITextmesh numText;
		public UIButton tryAddBtn;
	}
}
