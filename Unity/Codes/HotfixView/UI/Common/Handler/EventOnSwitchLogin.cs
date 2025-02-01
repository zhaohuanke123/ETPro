namespace ET
{
	[FriendClassAttribute(typeof (ET.SceneManagerComponent))]
	public class EventOnSwitchLogin: AEventAsync<UIEventType.SwitchLogin>
	{
		protected override async ETTask Run(UIEventType.SwitchLogin args)
		{
			if (SceneManagerComponent.Instance.CurrentScene != SceneNames.Login)
			{
				Scene currentScene = args.ZoneScene.CurrentScene();
				currentScene?.Dispose();
				await SceneManagerComponent.Instance.SwitchScene(SceneNames.Login, true);
			}
			await UIManagerComponent.Instance.CloseWindow<UILoadingView>();
			await UIManagerComponent.Instance.CloseWindow<UILobbyView>();
			await UIManagerComponent.Instance.OpenWindow<UILoginView>(UILoginView.PrefabPath);

			await ETTask.CompletedTask;
		}
	}
}
