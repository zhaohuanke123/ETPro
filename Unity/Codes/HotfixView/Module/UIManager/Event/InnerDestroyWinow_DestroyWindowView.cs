using UnityEngine;

namespace ET
{
	[FriendClass(typeof(UIWindow))]
    public class InnerDestroyWinow_DestroyWindowView : AEvent<UIEventType.InnerDestroyWindow>
	{
		protected override void Run(UIEventType.InnerDestroyWindow args)
		{
			var target = args.target;
			Entity view = target.GetComponent(target.ViewType);
			if (view != null)
			{
				var obj = view.GetGameObject();
				if (obj)
				{
					if (GameObjectPoolComponent.Instance == null)
						GameObject.Destroy(obj);
					else
						GameObjectPoolComponent.Instance.RecycleGameObject(obj);
				}
				view.BeforeOnDestroy();
				UIWatcherComponent.Instance.OnDestroy(view);
			}
		}
	}
}
