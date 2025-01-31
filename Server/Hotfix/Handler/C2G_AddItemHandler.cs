using System;

namespace ET
{
	[FriendClass(typeof (BagComponent))]
	public class C2G_AddItemHandler: AMRpcHandler<C2G_AddItem, G2C_AddItem>
	{
		protected override async ETTask Run(Session session, C2G_AddItem request, G2C_AddItem response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();

			// 获取背包组件
			BagComponent bagComponent = player.GetComponent<BagComponent>();

			// 添加道具
			bagComponent.AddItem(request.ItemId, request.Count);

			// 返回最新数量
			response.ItemId = request.ItemId;
			response.Count = bagComponent.GetItemCount(request.ItemId);
			response.Error = ErrorCode.ERR_Success;
			reply();

			await ETTask.CompletedTask;
		}
	}
}
