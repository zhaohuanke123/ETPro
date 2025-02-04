using ET.LoginUI;
using ET.UIEventType;

namespace ET
{
	public static class MessageBox
	{
		public static ETTask<MessageBoxRes> Show(Scene scene, string Ttile)
		{
			ETTask<MessageBoxRes> task = ETTask<MessageBoxRes>.Create();
			Game.EventSystem.PublishAsync(new ShowDialog()
			{
				Scene = scene,
				Text = Ttile,
				OnConfirm = () =>
				{
					task.SetResult(MessageBoxRes.OK);
				},
				OnCancel = () =>
				{
					task.SetResult(MessageBoxRes.Cancel);
				}
			}).Coroutine();

			return task;
		}
	}
}
