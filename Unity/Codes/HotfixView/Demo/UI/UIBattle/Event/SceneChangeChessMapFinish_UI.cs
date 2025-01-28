namespace ET
{
	public class SceneChangeChessMapFinish_UI: AEventAsync<EventType.SceneChangeChessMapFinish>
	{
		protected override async ETTask Run(EventType.SceneChangeChessMapFinish args)
		{
			await UIManagerComponent.Instance.OpenWindow<UIBattle>(UIBattle.PrefabPath);
			await UIManagerComponent.Instance.DestroyWindow<UILoadingView>();

			await ETTask.CompletedTask;
		}
	}
}
