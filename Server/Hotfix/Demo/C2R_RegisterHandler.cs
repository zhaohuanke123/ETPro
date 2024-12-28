using System;

namespace ET
{
    [MessageHandler]
    public class C2R_RegisterHandler: AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async ETTask Run(Session session, C2R_Register request, R2C_Register response, Action reply)
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
                // 2. 存在，返回错误
                if (accountInfos.Count > 0)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegister;
                    reply();
                    return;
                }

                // 3. 不存在，创建账号
                await dbComponent.Save(new AccountInfo() { Account = request.Account, Password = request.Password });
            }

            reply();
        }
    }
}