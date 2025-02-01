using ET.UIEventType;

namespace ET
{
	[MessageHandler]
	public class A2C_DisconnectHandler: AMHandler<A2C_Disconnect>
	{
		protected override async void Run(Session session, A2C_Disconnect message)
		{
			Scene zoneScene = session.ZoneScene();
			Log.Info($"断开连接");
			//TODO 断开连接后的处理

			await Game.EventSystem.PublishAsync(new SwitchLogin()
			{
				ZoneScene = session.ZoneScene()
			});

			Game.EventSystem.PublishAsync(new ShowToast()
			{
				Text = "与服务器断开连接"
			}).Coroutine();

			await ETTask.CompletedTask;
		}
	}
}
