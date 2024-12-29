using System;
using ET.EventType;
using ET.UIEventType;
using UnityEngine;
using SuperScrollView;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UILoginView))]
    public class UILoginViewOnCreateSystem: OnCreateSystem<UILoginView>
    {
        public override void OnCreate(UILoginView self)
        {
            self.loginBtn = self.AddUIComponent<UIButton>("Panel/LoginBtn");
            self.registerBtn = self.AddUIComponent<UIButton>("Panel/RegisterBtn");
            self.loginBtn.SetOnClickAsync(self.OnLoginClickHandler);
            self.registerBtn.SetOnClickAsync(self.OnRegisterClockHandler);
            self.account = self.AddUIComponent<UIInputTextmesh>("Panel/Account");
            self.password = self.AddUIComponent<UIInputTextmesh>("Panel/Password");
            self.ipaddr = self.AddUIComponent<UIInputTextmesh>("Panel/GM/InputField");
            self.loginBtn.AddUIComponent<UIRedDotComponent, string>("", "Test");

            self.settingView = self.AddUIComponent<UILoopListView2>("Panel/GM/Setting");
            self.settingView.InitListView(ServerConfigCategory.Instance.GetAll().Count, self.GetItemByIndex);

            self.account.SetOnEndEdit(() =>
            {
                if (!string.IsNullOrEmpty(self.account.GetText()))
                    GuidanceComponent.Instance.NoticeEvent("Enter_Account");
            });
        }
    }

    [UISystem]
    [FriendClass(typeof (UILoginView))]
    public class UILoginViewOnEnableSystem: OnEnableSystem<UILoginView, Scene>
    {
        public override void OnEnable(UILoginView self, Scene scene)
        {
            self.scene = scene;
            self.ipaddr.SetText(ServerConfigComponent.Instance.GetCurConfig().RealmIp);
            self.account.SetText(PlayerPrefs.GetString(CacheKeys.Account, ""));
            self.password.SetText(PlayerPrefs.GetString(CacheKeys.Password, ""));
        }
    }

    [FriendClass(typeof (UILoginView))]
    [FriendClass(typeof (GlobalComponent))]
    public static class UILoginViewSystem
    {
        public static async ETTask OnLoginClickHandler(this UILoginView self)
        {
            RedDotComponent.Instance.RefreshRedDotViewCount("Test1", 0);
            string account = self.account.GetText();
            string password = self.password.GetText();

            // self.loginBtn.SetInteractable(false);
            PlayerPrefs.SetString(CacheKeys.Account, account);
            PlayerPrefs.SetString(CacheKeys.Password, self.password.GetText());

            try
            {
                int errorCode = await LoginHelper.Login(self.scene, self.ipaddr.GetText(), account, password);

                if (errorCode != ErrorCode.ERR_Success)
                {
                    EventSystem.Instance.PublishAsync(new ShowErrorToast() { Scene = self.scene, ErrorCode = errorCode }).Coroutine();
                    return;
                }

                EventSystem.Instance.PublishAsync(new LoginFinish() { ZoneScene = self.scene, Account = account }).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void OnBtnClick(this UILoginView self, int id)
        {
            self.ipaddr.SetText(ServerConfigComponent.Instance.ChangeEnv(id).RealmIp);
        }

        public static async ETTask OnRegisterClockHandler(this UILoginView self)
        {
            string account = self.account.GetText();
            string password = self.password.GetText();

            PlayerPrefs.SetString(CacheKeys.Account, account);
            PlayerPrefs.SetString(CacheKeys.Password, self.password.GetText());

            int errorCode = await LoginHelper.Register(self.scene, self.ipaddr.GetText(), account, password);

            if (errorCode != ErrorCode.ERR_Success)
            {
                EventSystem.Instance.PublishAsync(new ShowErrorToast() { Scene = self.scene, ErrorCode = errorCode }).Coroutine();
                return;
            }

            EventSystem.Instance.PublishAsync(new ShowToast() { Text = "注册成功" }).Coroutine();
        }

        public static LoopListViewItem2 GetItemByIndex(this UILoginView self, LoopListView2 listView, int index)
        {
            if (index < 0 || index >= ServerConfigCategory.Instance.GetAll().Count)
                return null;

            ServerConfig data = ServerConfigCategory.Instance.Get(index + 1); //配置表从1开始的
            LoopListViewItem2 item = listView.NewListViewItem("SettingItem");
            if (!item.IsInitHandlerCalled)
            {
                item.IsInitHandlerCalled = true;
                self.settingView.AddItemViewComponent<UISettingItem>(item);
            }

            UISettingItem uiItemView = self.settingView.GetUIItemView<UISettingItem>(item);
            uiItemView.SetData(data, self.OnBtnClick);
            return item;
        }

        private static bool IsValidInput(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                Game.EventSystem.PublishAsync(new ShowToast() { Text = I18NComponent.Instance.I18NGetText("Text_Enter_Account") })
                        .Coroutine();
                return false;
            }

            return true;
        }
    }
}