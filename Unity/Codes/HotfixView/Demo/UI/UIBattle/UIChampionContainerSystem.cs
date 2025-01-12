namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIChampionContainer))]
    public class UIChampionContainerOnCreateSystem: OnCreateSystem<UIChampionContainer>
    {
        public override void OnCreate(UIChampionContainer self)
        {
            self.championBtn = self.AddUIComponent<UIButton>("champion");
            self.Image = self.AddUIComponent<UIImage>("champion/top/Image");
            self.type1 = self.AddUIComponent<UIText>("champion/top/type1");
            self.type2 = self.AddUIComponent<UIText>("champion/top/type2");
            self.icon1 = self.AddUIComponent<UIImage>("champion/top/icon1");
            self.icon2 = self.AddUIComponent<UIImage>("champion/top/icon2");
            self.Cost = self.AddUIComponent<UIText>("champion/bottom/Cost");
            self.Name = self.AddUIComponent<UIText>("champion/bottom/Name");
            self.championBtn.SetOnClick(self.OnClickchampionBtn);
        }
    }

    [ObjectSystem]
    [FriendClass(typeof (UIChampionContainer))]
    public class UIChampionContainerLoadSystem: LoadSystem<UIChampionContainer>
    {
        public override void Load(UIChampionContainer self)
        {
            self.championBtn.SetOnClick(self.OnClickchampionBtn);
        }
    }

    [FriendClass(typeof (UIChampionContainer))]
    public static class UIChampionContainerSystem
    {
        public static void OnClickchampionBtn(this UIChampionContainer self)
        {
        }
    }
}