using System;
using UnityEngine;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UILobbyView))]
    public class UILobbyViewOnCreateSystem: OnCreateSystem<UILobbyView>
    {
        public override void OnCreate(UILobbyView self)
        {
            self.EnterBtn = self.AddUIComponent<UIButton>("Panel/EnterMap");
            self.EnterBtn.SetOnClick(self.OnEnterBtnClick);
            self.ReturnLoginBtn = self.AddUIComponent<UIButton>("Panel/ReturnLogin");
            self.ReturnLoginBtn.SetOnClick(self.OnReturnLoginBtnClick);
        }
    }

    [UISystem]
    [FriendClass(typeof (UILobbyView))]
    public class UILobbyViewOnEnableSystem: OnEnableSystem<UILobbyView, Scene>
    {
        public override void OnEnable(UILobbyView self, Scene scene)
        {
            self.zoneScene = scene;
            GuidanceComponent.Instance.NoticeEvent("Click_Login");
        }
    }

    [FriendClass(typeof (UILobbyView))]
    public static class UILobbyViewSystem
    {
        public static void OnEnterBtnClick(this UILobbyView self)
        {
            EnterMapHelper.EnterMapAsync(self.zoneScene).Coroutine();
            // GuidanceComponent.Instance.NoticeEvent("Click_EnterMap");
        }

        public static void OnReturnLoginBtnClick(this UILobbyView self)
        {
            LoginHelper.Logout(self.zoneScene, (success) =>
            {
                if (success)
                {
                    UIManagerComponent.Instance.OpenWindow<UILoginView, Scene>(UILoginView.PrefabPath, self.zoneScene).Coroutine();
                    UIManagerComponent.Instance.CloseWindow<UILobbyView>().Coroutine();
                }
            }).Coroutine();
        }
    }
}