using ET.EventType;

namespace ET
{
    [MessageHandler]
    public class A2C_DisconnectHandler: AMHandler<A2C_Disconnect>
    {
        protected override async void Run(Session session, A2C_Disconnect message)
        {
            Log.Info($"断开连接");
            //TODO 断开连接后的处理
            await ETTask.CompletedTask;
        }
    }
}