using System;
using UnityEngine;

namespace ET
{
	[FriendClassAttribute(typeof (ET.ToastComponent))]
	[FriendClassAttribute(typeof (ET.UIComfirmDialog))]
	public class ShowDialog_CreateDialogUI: AEventAsync<UIEventType.ShowDialog>
	{
		protected override async ETTask Run(UIEventType.ShowDialog args)
		{
			string Content = args.Text;
			GameObject gameObject = await GameObjectPoolComponent.Instance.GetGameObjectAsync(UIComfirmDialog.PrefabPath);
			UIComfirmDialog ui = ToastComponent.Instance.AddChild<UIComfirmDialog>();
			UIComfirmDialog.go = gameObject;
			UIComfirmDialog.Instance = ui;
			var transform = gameObject.transform;
			ui.AddUIComponent<UITransform, Transform>("", transform);
			transform = gameObject.transform;
			transform.SetParent(ToastComponent.Instance.root);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = new Vector3(1, 1, 1);
			UIWatcherComponent.Instance.OnCreate(ui);
			UIWatcherComponent.Instance.OnEnable(ui, Content);

			if (args.OnConfirm == null)
			{
				ui.ComfirmBtn.SetActive(false);
			}
			else
			{
				ui.ComfirmBtn.SetActive(true);
			}

			ui.SetTitle(args.Text);
			ui.OnCancel = () =>
			{
				args.OnCancel?.Invoke();
				ui.BeforeOnDestroy();
				UIWatcherComponent.Instance.OnDestroy(ui);
				GameObjectPoolComponent.Instance.RecycleGameObject(gameObject);
			};
			ui.OnConfirm = () =>
			{
				args.OnConfirm?.Invoke();
				ui.BeforeOnDestroy();
				UIWatcherComponent.Instance.OnDestroy(ui);
				GameObjectPoolComponent.Instance.RecycleGameObject(gameObject);
			};

		}
	}
}
