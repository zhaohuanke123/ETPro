using UnityEngine;

namespace ET
{
	[FriendClass(typeof (ToastComponent))]
	[FriendClassAttribute(typeof (ET.UIToast))]
	public class ShowToastEvent_CreateToastUI: AEventAsync<UIEventType.ShowToast>
	{
		protected override async ETTask Run(UIEventType.ShowToast args)
		{
			if (args.showTime == 0)
			{
				args.showTime = 2;
			}
			await Show(args.Text, args.showTime);
		}

		private async ETTask Show(string Content, int seconds = 2)
		{
			GameObject gameObject = await GameObjectPoolComponent.Instance.GetGameObjectAsync("UI/UIToast/Prefabs/UIToast.prefab");
			UIToast ui = ToastComponent.Instance.AddChild<UIToast>();
			var transform = gameObject.transform;
			ui.AddUIComponent<UITransform, Transform>("", transform);
			transform = gameObject.transform;
			transform.SetParent(ToastComponent.Instance.root, false);
			// transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = new Vector3(1, 1, 1);
			UIWatcherComponent.Instance.OnCreate(ui);
			UIWatcherComponent.Instance.OnEnable(ui, Content);
			// 等待animator动画结束
			await TimerComponent.Instance.WaitAsync((long)(ui.animator.GetCurrentAnimatorStateInfo(0).length * 1000));
			await TimerComponent.Instance.WaitAsync(seconds * 1000);
			ui.BeforeOnDestroy();
			UIWatcherComponent.Instance.OnDisable(ui);
			UIWatcherComponent.Instance.OnDestroy(ui);
			await TimerComponent.Instance.WaitAsync(100);
			await TimerComponent.Instance.WaitAsync((long)(ui.animator.GetCurrentAnimatorStateInfo(0).length * 1000));
			GameObjectPoolComponent.Instance.RecycleGameObject(gameObject);
		}
	}
}
