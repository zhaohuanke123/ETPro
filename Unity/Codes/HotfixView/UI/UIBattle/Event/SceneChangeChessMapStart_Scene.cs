namespace ET
{
    public class SceneChangeChessMapStart_Scene: AEventAsync<EventType.SceneChangeChessMapStart>
    {
        protected override async ETTask Run(EventType.SceneChangeChessMapStart args)
        {
            await SceneManagerComponent.Instance.SwitchScene(SceneNames.Main, true);
            await ETTask.CompletedTask;
        }
    }
}