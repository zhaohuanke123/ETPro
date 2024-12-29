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
            // self.EnterBtn.SetOnClick(self.OnEnterBtnClick);
            self.ReturnLoginBtn = self.AddUIComponent<UIButton>("Panel/ReturnLogin");
            self.ReturnLoginBtn.SetOnClickAsync(self.OnReturnLoginBtnClick);
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

        public static async ETTask OnReturnLoginBtnClick(this UILobbyView self)
        {
            int errorCode = await LoginHelper.Logout(self.zoneScene);

            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error("Logout error");
                return;
            }

            UIManagerComponent.Instance.OpenWindow<UILoginView, Scene>(UILoginView.PrefabPath, self.zoneScene).Coroutine();
            UIManagerComponent.Instance.CloseWindow<UILobbyView>().Coroutine();
        }
    }
}