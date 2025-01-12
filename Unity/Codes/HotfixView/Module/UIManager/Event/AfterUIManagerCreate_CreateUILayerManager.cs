namespace ET
{
    public class AfterUIManagerCreate_CreateUILayerManager : AEvent<UIEventType.AfterUIManagerCreate>
    {
        protected override void Run(UIEventType.AfterUIManagerCreate args)
        {
            UIManagerComponent.Instance.AddComponent<UILayersComponent>();
        }
    }
}
