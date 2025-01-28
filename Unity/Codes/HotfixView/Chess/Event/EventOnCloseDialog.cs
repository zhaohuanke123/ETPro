namespace ET
{
	public class EventOnCloseDialog: AEventAsync<UIEventType.CloseDialog>
	{
		protected override async ETTask Run(UIEventType.CloseDialog args)
		{
			UIComfirmDialog uiComfirmDialog = UIComfirmDialog.Instance;
			if (uiComfirmDialog != null)
			{
				uiComfirmDialog.BeforeOnDestroy();
				UIWatcherComponent.Instance.OnDestroy(uiComfirmDialog);
				GameObjectPoolComponent.Instance.RecycleGameObject(UIComfirmDialog.go);
				UIComfirmDialog.go = null;
				UIComfirmDialog.Instance = null;
			}

			await ETTask.CompletedTask;
		}
	}
}
