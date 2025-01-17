using System;
using System.Text.RegularExpressions;

namespace ET.Account.Handle
{
    [FriendClassAttribute(typeof (ET.SessionPlayerComponent))]
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

            // 防止多次点击登录
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

            AccountInfo accountInfo = null;
            using (session.AddComponent<SessionLockingComponent>())
            {
                //  协程锁，防止多个人同时登录一个账号
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    var accountInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<AccountInfo>(d => d.Account.Equals(request.AccountName.Trim()));

                    if (accountInfos != null && accountInfos.Count > 0)
                    {
                        accountInfo = accountInfos[0];
                        session.AddChild(accountInfo);
                        if (accountInfo.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            reply();
                            session.DisConnect().Coroutine();
                            accountInfo?.Dispose();
                            return;
                        }

                        if (!accountInfo.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();
                            session.DisConnect().Coroutine();
                            accountInfo?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        response.Error = ErrorCode.ERR_AccountNotExistError;
                        reply();
                        session.DisConnect().Coroutine();
                        return;
                    }
                }
            }

            // 账号服务器请求登录中心服
            // StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
            // long loginCenterInstanceId = startSceneConfig.InstanceId;
            // var loginAccountResponse = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId,
            //     new A2L_LoginAccountRequest() { AccountId = accountInfo.Id });

            // if (loginAccountResponse.Error != ErrorCode.ERR_Success)
            // {
            //     response.Error = loginAccountResponse.Error;
            //
            //     reply();
            //     session.DisConnect().Coroutine();
            //     accountInfo.Dispose();
            //     return;
            // }

            // 把之前的Session(已经登录的)踢下线
            AccountSessionsComponent accountSessionsComponent = session.DomainScene().GetComponent<AccountSessionsComponent>();
            long accountSessionInstanceId = accountSessionsComponent.Get(accountInfo.Id);
            if (Game.EventSystem.Get(accountSessionInstanceId) is Session otherSession)
            {
                otherSession.Send(new A2C_Disconnect() { Error = 0 });
                otherSession.DisConnect().Coroutine();
            }

            accountSessionsComponent.Add(accountInfo.Id, session.InstanceId);
            // 设置账号超时时间
            session.AddComponent<AccountCheckoutTimeComponent, long>(accountInfo.Id);

            string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);
            TokenComponent tokenComponent = session.DomainScene().GetComponent<TokenComponent>();
            tokenComponent.Remove(accountInfo.Id);
            tokenComponent.Add(accountInfo.Id, Token);

            response.AccountId = accountInfo.Id;
            response.Token = Token;

            // 随机分配一个Gate
            // StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone());
            // Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            // G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(config.InstanceId,
            //     new R2G_GetLoginKey() { Account = request.AccountName });
            //
            // response.Address = config.OuterIPPort.ToString();
            // response.Key = g2RGetLoginKey.Key;
            // response.GateId = g2RGetLoginKey.GateId;

            Scene scene = session.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.AddChild<Player, string>(request.AccountName);
            playerComponent.Add(player);
            player.Session = session;
            session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

            reply();
        }
    }
}