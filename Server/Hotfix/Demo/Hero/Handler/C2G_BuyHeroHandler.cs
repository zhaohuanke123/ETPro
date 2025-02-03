using System;

namespace ET
{
	[FriendClass(typeof (Player))]
	[FriendClass(typeof (HeroComponent))]
	public class C2G_BuyHeroHandler: AMRpcHandler<C2G_BuyHero, G2C_BuyHero>
	{
		protected override async ETTask Run(Session session, C2G_BuyHero request, G2C_BuyHero response, Action reply)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			HeroComponent heroComponent = player.GetComponent<HeroComponent>();
			BagComponent bagComponent = player.GetComponent<BagComponent>();

			// 防止多次点击购买
			if (session.GetComponent<SessionLockingComponent>() != null)
			{
				response.Error = ErrorCode.ERR_RequestRepeatedly;
				reply();
				return;
			}

			using (session.AddComponent<SessionLockingComponent>())
			{
				// 使用协程锁确保同一玩家的购买操作串行执行
				using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.BuyHero, player.Id.GetHashCode()))
				{
					// 检查是否已拥有
					if (heroComponent.HasHero(request.HeroConfigId))
					{
						response.Error = ErrorCode.ERR_HeroAlreadyOwned;
						reply();
						return;
					}

					int point = bagComponent.GetItemCount(ItemDefine.PointId);
					// 检查积分是否足够
					HeroConfig heroConfig = HeroConfigCategory.Instance.Get(request.HeroConfigId);
					if (point < heroConfig.ItemCount)
					{
						response.Error = ErrorCode.ERR_PointNotEnough;
						reply();
						return;
					}

					// 扣除积分
					point -= heroConfig.ItemCount;
					await bagComponent.SetItemCount(ItemDefine.PointId, point);
					// 添加英雄
					await heroComponent.AddHero(request.HeroConfigId);

					response.HeroConfigId = request.HeroConfigId;
					reply();
				}
			}
		}
	}
}
