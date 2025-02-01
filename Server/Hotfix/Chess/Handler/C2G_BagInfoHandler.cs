using System;
using System.Collections.Generic;

namespace ET
{
	public class C2G_BagInfoHandler: AMRpcHandler<C2G_BagInfo, G2C_BagInfo>
	{
		protected override async ETTask Run(Session session, C2G_BagInfo request, G2C_BagInfo response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			BagComponent bagComponent = player.GetComponent<BagComponent>();

			response.Items = new List<ItemInfo>();
			bagComponent.GetItems(response.Items);

			reply();
			await ETTask.CompletedTask;
		}
	}
}
