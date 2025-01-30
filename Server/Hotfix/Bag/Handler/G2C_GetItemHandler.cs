using System;

namespace ET
{
	public class G2C_GetItemHandler: AMRpcHandler<C2G_GetItem, G2C_GetItem>
	{
		protected override async ETTask Run(Session session, C2G_GetItem request, G2C_GetItem response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			BagComponent bagComponent = player.GetComponent<BagComponent>();

			int itemCount = default;
			try
			{
				itemCount = bagComponent.GetItemCount(request.ItemId);
			}
			catch (ArgumentException)
			{
				response.Error = ErrorCode.ERR_ItemNotFound;
				reply();
				return;
			}
			
			response.ItemId = request.ItemId;
			response.Count = itemCount;

			reply();
			await ETTask.CompletedTask;
		}
	}
}
