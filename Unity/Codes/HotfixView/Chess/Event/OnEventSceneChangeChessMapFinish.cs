namespace ET
{
    public class OnEventSceneChangeChessMapFinish: AEventAsync<EventType.SceneChangeChessMapFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeChessMapFinish args)
        {
            GamePlayComponent gamePlayComponent = args.CurrentScene.GetComponent<GamePlayComponent>();
            gamePlayComponent.AddComponent<ChessBattleViewComponent>();
            gamePlayComponent.AddComponent<InputControlComponent>();
            gamePlayComponent.AddComponent<MapComponent>();
            await ETTask.CompletedTask;
        }
    }
}