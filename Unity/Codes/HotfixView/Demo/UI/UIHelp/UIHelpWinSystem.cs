namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIHelpWin))]
    public class UIHelpWinOnCreateSystem: OnCreateSystem<UIHelpWin>
    {
        public override void OnCreate(UIHelpWin self)
        {
            self.GalBtn = self.AddUIComponent<UIButton>("Button");
            self.GalBtn.SetOnClick(self.OnGalBtnClick);
            self.GalBtn.SetActive(false);
            self.SettingBtn = self.AddUIComponent<UIButton>("Setting");
            self.SettingBtn.SetOnClick(self.OnSettingBtnClick);
            self.SettingBtn.SetActive(false);
            self.ReturnBtn = self.AddUIComponent<UIButton>("Return");
            self.ReturnBtn.SetOnClick(self.OnReturnBtnClick);
        }
    }

    // [UISystem]
    // [FriendClass(typeof (UIHelpWin))]
    // public class UIHelpViewOnEnableSystem: OnEnableSystem<UIHelpWin, Scene>
    // {
    //     public override void OnEnable(UIHelpWin self, Scene scene)
    //     {
    //         self.zoneScene = scene;
    //     }
    // }

    [FriendClass(typeof (UIHelpWin))]
    public static class UIHelpWinSystem
    {
        public static void OnGalBtnClick(this UIHelpWin self)
        {
            GalGameEngineComponent.Instance.PlayChapterByName("StartChapter").Coroutine();
        }

        public static void OnSettingBtnClick(this UIHelpWin self)
        {
            UIManagerComponent.Instance.OpenWindow<UISettingView>(UISettingView.PrefabPath).Coroutine();
        }

        public static void OnReturnBtnClick(this UIHelpWin self)
        {
            EnterMapHelper.ExitMapAsync(self.ZoneScene()).Coroutine();
        }
    }
}