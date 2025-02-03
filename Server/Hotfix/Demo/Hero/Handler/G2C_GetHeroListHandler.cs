using System;

namespace ET
{
	[FriendClass(typeof (Player))]
	[FriendClass(typeof (HeroComponent))]
	public class C2G_GetHeroListHandler: AMRpcHandler<C2G_GetHeroList, G2C_GetHeroList>
	{
		protected override async ETTask Run(Session session, C2G_GetHeroList request, G2C_GetHeroList response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			HeroComponent heroComponent = player.GetComponent<HeroComponent>();

			// 获取玩家拥有的英雄ID列表
			response.HeroIds = heroComponent.GetAllHeroes();
			reply();

			await ETTask.CompletedTask;
		}
	}
}
