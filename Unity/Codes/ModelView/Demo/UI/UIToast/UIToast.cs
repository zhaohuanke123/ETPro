namespace ET
{
	[ChildOf(typeof(ToastComponent))]
    public class UIToast : Entity,IAwake,IOnCreate,IOnEnable<string>
	{
		public UITextmesh Text;
    }
}
