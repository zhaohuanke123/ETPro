namespace ET
{
    public class SceneChangeLoginBegin_ToLogin: AEventAsync<EventType.SceneChangeLoginBegin>
    {
        protected override async ETTask Run(EventType.SceneChangeLoginBegin args)
        {
            await SceneManagerComponent.Instance.SwitchScene(SceneNames.Login, true);
        }
    }
}