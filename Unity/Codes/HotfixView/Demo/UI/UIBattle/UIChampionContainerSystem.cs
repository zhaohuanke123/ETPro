namespace ET
{
    public class UIChampionContainerAwakeSystem: AwakeSystem<UIChampionContainer>
    {
        public override void Awake(UIChampionContainer self)
        {
            // champion
            self.championBtn = self.AddUIComponent<UIButton>("champion");
            // champion/top/SK1
            self.Sk1 = self.championBtn.AddUIComponent<UIIconName>("top/SK1");
            // champion/top/SK2
            self.Sk2 = self.championBtn.AddUIComponent<UIIconName>("top/SK2");
            // champion/bottom/CostGo
            self.cost = self.championBtn.AddUIComponent<UICostIN>("bottom/CostGo");
        }
    }

    [FriendClass(typeof (UIChampionContainer))]
    public static class UIChampionContainerSystem
    {
        public static void Test(this UIChampionContainer self)
        {
        }
    }
}