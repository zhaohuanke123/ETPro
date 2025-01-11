namespace ET
{
    public class SceneChangeLoginFinish_CloseUI: AEventAsync<EventType.SceneChangeLoginFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeLoginFinish args)
        {
            await UIManagerComponent.Instance.OpenWindow<UILobbyView>(UILobbyView.PrefabPath);
            await UIManagerComponent.Instance.DestroyWindow<UIHelpWin>();
            await UIManagerComponent.Instance.DestroyWindow<UILoadingView>();
            await ETTask.CompletedTask;
        }
    }
}