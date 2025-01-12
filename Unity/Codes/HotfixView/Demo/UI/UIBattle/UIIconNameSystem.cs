namespace ET
{
    [ObjectSystem]
    public class UIIconNameAwakeSystem: AwakeSystem<UIIconName>
    {
        public override void Awake(UIIconName self)
        {
            self.Icon = self.AddUIComponent<UIImage>("Icon");
            self.Name = self.AddUIComponent<UIText>("Name");
        }
    }

    [ObjectSystem]
    public class UIIconNameDestroySystem: DestroySystem<UIIconName>
    {
        public override void Destroy(UIIconName self)
        {
        }
    }

    [FriendClass(typeof (UIIconName))]
    public static partial class UIIconNameSystem
    {
        public static void SetName(this UIIconName self, string Name)
        {
            self.Name.SetText(Name);
        }

        public static async ETTask SetIcon(this UIIconName self, string spritePath)
        {
            await self.Icon.SetSpritePath(spritePath);
        }

        public static async ETTask SetIconAndName(this UIIconName self, string spritePath, string Name)
        {
            await self.SetIcon(spritePath);
            self.SetName(Name);
        }
    }
}