namespace ET
{
    [ObjectSystem]
    public class ToastComponentAwakeSystem : AwakeSystem<ToastComponent>
    {
        public override void Awake(ToastComponent self)
        {
            ToastComponent.Instance = self;
            self.root = UIManagerComponent.Instance.GetLayer(UILayerNames.TipLayer).transform;
        }
    }
    [ObjectSystem]
    public class ToastComponentDestroySystem : DestroySystem<ToastComponent>
    {
        public override void Destroy(ToastComponent self)
        {
            ToastComponent.Instance = null;
            self.root = null;
        }
    }
}
