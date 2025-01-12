namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIBattle))]
    public class UIBattleOnCreateSystem: OnCreateSystem<UIBattle>
    {
        public override void OnCreate(UIBattle self)
        {
            self.GoldText = self.AddUIComponent<UIText>("/Gold/Text");
            self.CountDownText = self.AddUIComponent<UIText>("Placement/Timer/Text");
            self.HpText = self.AddUIComponent<UIText>("Hp/Text");
            
            // ReturnBtn
            self.ReturnBtn = self.AddUIComponent<UIButton>("ReturnBtn");
            self.ReturnBtn.SetOnClickAsync(self.OnReturnBtnClick);
        }
    }

    [ObjectSystem]
    [FriendClass(typeof (UIBattle))]
    public class UIBattleLoadSystem: LoadSystem<UIBattle>
    {
        public override void Load(UIBattle self)
        {
        }
    }

    [FriendClass(typeof (UIBattle))]
    public static class UIBattleSystem
    {
        public static async ETTask OnReturnBtnClick(this UIBattle self)
        {
            await SceneManagerComponent.Instance.SwitchScene(SceneNames.Login);
            await UIManagerComponent.Instance.DestroyWindow<UIBattle>();
            await UIManagerComponent.Instance.OpenWindow<UILobbyView>(UILobbyView.PrefabPath);
            await UIManagerComponent.Instance.CloseWindow<UILoadingView>();
        }
    }
}