﻿namespace ET
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
            self.ReturnLoginBtn.SetOnClickAsync(self.OnReturnLoginBtnClick);
            // Panel/EnterChessMap
            self.EnterChessMapBtn = self.AddUIComponent<UIButton>("Panel/EnterChessMap");
            self.EnterChessMapBtn.SetOnClickAsync(self.OnEnterChessMapBtnClick);
        }
    }

    [FriendClass(typeof (UILobbyView))]
    public static class UILobbyViewSystem
    {
        public static void OnEnterBtnClick(this UILobbyView self)
        {
            EnterMapHelper.EnterMapAsync(self.ZoneScene()).Coroutine();
            // GuidanceComponent.Instance.NoticeEvent("Click_EnterMap");
        }

        public static async ETTask OnReturnLoginBtnClick(this UILobbyView self)
        {
            int errorCode = await LoginHelper.Logout(self.ZoneScene());

            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error("Logout error");
                return;
            }

            UIManagerComponent.Instance.OpenWindow<UILoginView>(UILoginView.PrefabPath).Coroutine();
            UIManagerComponent.Instance.DestroyWindow<UILobbyView>().Coroutine();
        }

        public static async ETTask OnEnterChessMapBtnClick(this UILobbyView self)
        {
            await EnterMapHelper.EnterChessMapAsync(self.ZoneScene());
        }
    }
}