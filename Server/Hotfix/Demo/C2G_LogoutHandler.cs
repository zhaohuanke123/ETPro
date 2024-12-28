using System;
using CommandLine;

namespace ET
{
    [FriendClass(typeof (SessionPlayerComponent))]
    public class C2G_LogoutHandler: AMRpcHandler<C2G_Logout, G2C_Logout>
    {
        protected override async ETTask Run(Session session, C2G_Logout request, G2C_Logout response, Action reply)
        {
            // 获取当前会话绑定的玩家组件
            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (sessionPlayerComponent == null)
            {
                response.Error = ErrorCode.ERR_NotBindPlayer;
                response.Message = "当前会话未绑定玩家!";
                reply();
                return;
            }

            Scene scene = session.DomainScene();
            PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();

            // 根据PlayerId找到玩家对象
            long playerId = sessionPlayerComponent.PlayerId;
            Player player = playerComponent.Get(playerId);
            
            if (player != null)
            {
                // 移除玩家组件中的玩家
                playerComponent.Remove(playerId);
                player.Dispose(); // 清理玩家数据
            }

            // 返回响应
            response.Message = "登出成功!";
            reply();

            await TimerComponent.Instance.WaitAsync(2000);

            // 移除会话上的相关组件
            // session.RemoveComponent<SessionPlayerComponent>();
            // session.RemoveComponent<MailBoxComponent>();
            session.Dispose();
        }
    }
}