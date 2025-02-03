using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET
{
	public class C2A_RegisterAccountHandler: AMRpcHandler<C2A_RegisterAccount, A2C_RegisterAccount>
	{
		protected override async ETTask Run(Session session, C2A_RegisterAccount request, A2C_RegisterAccount response, Action reply)
		{
			if (session.DomainScene().SceneType != SceneType.Account)
			{
				Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
				session.Dispose();
				return;
			}

			session.RemoveComponent<SessionAcceptTimeoutComponent>();

			// 防止多次点击
			if (session.GetComponent<SessionLockingComponent>() != null)
			{
				response.Error = ErrorCode.ERR_RequestRepeatedly;
				reply();
				session.DisConnect().Coroutine();
				return;
			}

			if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
			{
				response.Error = ErrorCode.ERR_LoginInfoEmpty;
				reply();
				session.DisConnect().Coroutine();
				return;
			}

			if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
			{
				response.Error = ErrorCode.ERR_AccountNameFormError;
				reply();
				session.DisConnect().Coroutine();
				return;
			}

			if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
			{
				response.Error = ErrorCode.ERR_PasswordFormError;
				reply();
				session.DisConnect().Coroutine();
				return;
			}

			using (session.AddComponent<SessionLockingComponent>())
			{
				//  协程锁
				using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
				{
					var accountInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
							.Query<AccountInfo>(d => d.Account.Equals(request.AccountName.Trim()));

					AccountInfo accountInfo = null;
					if (accountInfos != null && accountInfos.Count > 0)
					{
						response.Error = ErrorCode.ERR_AccountAlreadyRegister;
						reply();
						return;
					}

					accountInfo = session.AddChild<AccountInfo>();
					accountInfo.Account = request.AccountName.Trim();
					accountInfo.Password = request.Password;
					accountInfo.CreateTime = TimeHelper.ServerNow();
					accountInfo.AccountType = (int)AccountType.General;

					BagComponent bagComponent = accountInfo.AddComponent<BagComponent>();
					accountInfo.BagId = bagComponent.Id;
					HeroComponent heroComponent = accountInfo.AddComponent<HeroComponent>();
					accountInfo.HeroBagId = heroComponent.Id;
					ETTask t1 = DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(bagComponent);
					ETTask t2 = DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(accountInfo);
					ETTask t3 = DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(heroComponent);
					await ETTaskHelper.WaitAll(new[] { t1, t2, t3 });
				}

				reply();
				await ETTask.CompletedTask;
			}
		}
	}
}
