namespace ET
{
    public class AppStartInitFinish_CreateLoginUI: AEventAsync<EventType.AppStartInitFinish>
    {
        protected override async ETTask Run(EventType.AppStartInitFinish args)
        {
            await SceneManagerComponent.Instance.SwitchScene(SceneNames.Login, true);
            await UIManagerComponent.Instance.OpenWindow<UILoginView>(UILoginView.PrefabPath);
            await UIManagerComponent.Instance.CloseWindow<UILoadingView>();
        }
    }
}