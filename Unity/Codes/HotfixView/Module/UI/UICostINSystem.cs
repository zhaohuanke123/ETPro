namespace ET
{
    public class UICostINSystemAwakeSystem: AwakeSystem<UICostIN>
    {
        public override void Awake(UICostIN self)
        {
            self.costIcon = self.AddUIComponent<UIImage>("coin");
            self.costText = self.AddUIComponent<UIText>("cost");
        }
    }

    [FriendClass(typeof (UICostIN))]
    public static class UICostINSystemSystem
    {
        public static void SetNumber(this UICostIN self, int number)
        {
            self.costText.SetText(number.ToString());
        }
    }
}