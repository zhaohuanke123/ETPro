using System;
using System.Net;

namespace ET
{
    [MessageHandler]
    public class C2R_LoginHandler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            // 验证账号密码合法性
            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoEmpty;
                reply();
                return;
            }

            DBComponent dbComponent = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.Account.GetHashCode()))
            {
                // 1. 查询账号是否存在
                var accountInfos = await dbComponent.Query<AccountInfo>(a => a.Account == request.Account);
                // 2. 不存在
                if (accountInfos.Count <= 0)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply();
                    return;
                }

                // 3. 存在，验证密码
                if (accountInfos[0].Password != request.Password)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply();
                    return;
                }
            }

            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone());
            Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(config.InstanceId,
                new R2G_GetLoginKey() { Account = request.Account });

            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            reply();
        }

        private async ETTask CloseSession(Session session)
        {
            await TimerComponent.Instance.WaitAsync(1000);
            session.Dispose();
        }
    }
}