using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (UIWindow))]
    public class InnerOpenWindow_GetGameObject: AEventAsync<UIEventType.InnerOpenWindow>
    {
        protected override async ETTask Run(UIEventType.InnerOpenWindow args)
        {
            UIWindow target = args.window;
            Entity view = target.GetComponent(target.ViewType);

            await UIWatcherComponent.Instance.OnViewInitializationSystem(view);

            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(args.path);
            if (go == null)
            {
                Log.Error(string.Format("UIManager InnerOpenWindow {0} faild", target.PrefabPath));
                return;
            }

            Transform trans = go.transform;
            trans.SetParent(UIManagerComponent.Instance.GetLayer(target.Layer).transform, false);
            trans.name = target.Name;

            view.AddUIComponent<UITransform, Transform>("", trans);
            UIWatcherComponent.Instance.OnCreate(view);
        }
    }
}