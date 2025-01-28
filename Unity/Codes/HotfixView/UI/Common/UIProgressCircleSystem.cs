namespace ET
{
	[UISystem]
	public class UIProgressCircleCreateSystem: OnCreateSystem<UIProgressCircle>
	{
		public override void OnCreate(UIProgressCircle self)
		{
			self.ValueText = self.AddUIComponent<UIText>("Text");
			self.Slider = self.AddUIComponent<UIImage>("Image");
		}
	}

	[FriendClass(typeof (UIProgressCircle))]
	public static class UIProgressCircleSystem
	{
		public static void SetProcess(this UIProgressCircle self, float value)
		{
			self.Slider.SetFillAmount(value);
			self.ValueText.SetText($"{(value * 100):F0}%");
		}
	}
}
