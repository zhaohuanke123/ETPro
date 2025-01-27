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

			C2G_SceneReady c2GSceneReady = new C2G_SceneReady();
			args.ZoneScene.GetComponent<SessionComponent>().Session.Send(c2GSceneReady);

			await ETTask.CompletedTask;
		}
	}
}
