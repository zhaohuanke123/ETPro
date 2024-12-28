using System;
using System.Text.RegularExpressions;

namespace ET.Account.Handle
{
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session.Dispose();
                return;
            }

            if (Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session.Dispose();
                return;
            }

            var accountInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                    .Query<AccountInfo>(d => d.Account.Equals(request.AccountName.Trim()));
            AccountInfo accountInfo = null;
            if (accountInfos.Count > 0)
            {
                accountInfo = accountInfos[0];
                session.AddChild(accountInfo);
                if (accountInfo.AccountType == (int)AccountType.BlackList)
                {
                    response.Error = ErrorCode.ERR_LoginInfoError;
                    reply();
                    session.Dispose();
                    return;
                }

                if (!accountInfo.Password.Equals(request.Password))
                {
                    response.Error = ErrorCode.ERR_LoginInfoError;
                    reply();
                    session.Dispose();
                    return;
                }
            }
            else
            {
                accountInfo = session.AddChild<AccountInfo>();
                accountInfo.Account = request.AccountName.Trim();
                accountInfo.Password = request.Password;
                accountInfo.CreateTime = TimeHelper.ServerNow();
                accountInfo.AccountType = (int)AccountType.General;
                await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(accountInfo);
            }

            string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);
            TokenComponent tokenComponent = session.DomainScene().GetComponent<TokenComponent>();
            tokenComponent.Remove(accountInfo.Id);
            tokenComponent.Add(accountInfo.Id, Token);

            response.AccountId = accountInfo.Id;
            response.Token = Token;

            reply();
        }
    }
}