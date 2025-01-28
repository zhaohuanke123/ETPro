namespace ET
{
    [ObjectSystem]
    public class UIIconNameCreateSystem: OnCreateSystem<UIIconName>
    {
        public override void OnCreate(UIIconName self)
        {
            self.Icon = self.AddUIComponent<UIImage>("Icon");
            self.Name = self.AddUIComponent<UITextmesh>("Name");
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

        public static void SetIconAndName(this UIIconName self, string spritePath, string Name)
        {
            self.SetName(Name);
            self.SetIcon(spritePath).Coroutine();
        }
    }
}