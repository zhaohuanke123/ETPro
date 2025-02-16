using System;

namespace ET
{
	[MessageHandler]
	public class G2C_UpdateItemHandler: AMHandler<G2C_UpdateItem>
	{
		protected override void Run(Session session, G2C_UpdateItem message)
		{
			UILobbyView ui = UIManagerComponent.Instance.GetWindow<UILobbyView>();
			if (ui == null)
			{
				return;
			}

			ui.SetPoint(message.ItemCount);
		}
	}
}
