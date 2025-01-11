using System;

namespace ET
{
    [FriendClass(typeof (AccountCheckoutTimeComponent))]
    [FriendClassAttribute(typeof (ET.SessionPlayerComponent))]
    public class C2A_LogoutRequestHandler: AMRpcHandler<C2A_LogoutRequest, A2C_LogoutResponse>
    {
        protected override async ETTask Run(Session session, C2A_LogoutRequest request, A2C_LogoutResponse response, Action reply)
        {
            // 检查会话是否存在
            if (session == null || session.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                return;
            }

            // 获取当前账号ID 
            AccountCheckoutTimeComponent accountCheckout = session.GetComponent<AccountCheckoutTimeComponent>();
            if (accountCheckout == null)
            {
                response.Error = ErrorCode.ERR_AccountNotLoggedIn;
                reply();
                return;
            }

            long accountId = accountCheckout.AccountId;

            // 删除会话超时组件
            session.RemoveComponent<AccountCheckoutTimeComponent>();

            // 移除令牌
            TokenComponent tokenComponent = session.DomainScene().GetComponent<TokenComponent>();
            tokenComponent?.Remove(accountId);

            // 移除账号会话
            AccountSessionsComponent accountSessionsComponent = session.DomainScene().GetComponent<AccountSessionsComponent>();
            accountSessionsComponent?.Remove(accountId);

            Scene scene = session.DomainScene();
            long playerId = session.GetComponent<SessionPlayerComponent>().PlayerId;
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
            Player player = playerComponent.Get(playerId);

            if (player == null)
            {
                response.Error = ErrorCode.ERR_PlayerNotLoggedIn;
                reply();
                return;
            }

            playerComponent.Remove(playerId);

            // 断开会话
            session.DisConnect().Coroutine();

            // 响应成功退出
            response.Error = ErrorCode.ERR_Success;
            reply();
            await ETTask.CompletedTask;
        }
    }
}