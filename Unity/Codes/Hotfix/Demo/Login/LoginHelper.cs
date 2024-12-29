using System;

namespace ET
{
    [FriendClass(typeof (RouterDataComponent))]
    [FriendClass(typeof (GetRouterComponent))]
    [FriendClassAttribute(typeof (ET.AccountInfoComponent))]
    public static class LoginHelper
    {
        [Timer(TimerType.LoginTimeOut)]
        public class LoginTimeOut: ATimer<ETCancellationToken>
        {
            public override void Run(ETCancellationToken self)
            {
                try
                {
                    self.Cancel();
                    Log.Info("Login Time Out");
                }
                catch (Exception e)
                {
                    Log.Error($"move timer error: LoginTimeOut\n{e}");
                }
            }
        }

        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            zoneScene.RemoveComponent<SessionComponent>();
            A2C_LoginAccount a2CLoginAccount = null;
            Session accountSession = null;
            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);
                a2CLoginAccount = (A2C_LoginAccount)await accountSession.Call(new C2A_LoginAccount() { AccountName = account, Password = password });

                // 创建一个ETModel层的Session
                // R2C_Login r2CLogin;
                // Session session = null;
                // long timerId = 0;
                // try
                // {
                //     session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                //     ETCancellationToken cancel = new ETCancellationToken();
                //     timerId = TimerComponent.Instance.NewOnceTimer(TimeInfo.Instance.ClientNow() + 10000, TimerType.LoginTimeOut, cancel);
                //     r2CLogin = (R2C_Login)await session.Call(new C2R_Login() { Account = account, Password = password },
                //         cancel);
                // }
                // finally
                // {
                //     session?.Dispose();
                // }
                //
                // TimerComponent.Instance.Remove(ref timerId);
                // long channelId = RandomHelper.RandInt64();
                // var routercomponent = zoneScene.AddComponent<GetRouterComponent, long, long>(r2CLogin.GateId, channelId);
                // string routerAddress = await routercomponent.Tcs;
                // if (routerAddress == "")
                // {
                //     zoneScene.RemoveComponent<GetRouterComponent>();
                //     throw new Exception("routerAddress 失败");
                // }
                //
                // Log.Debug("routerAddress 获取成功:" + routerAddress);
                // zoneScene.RemoveComponent<GetRouterComponent>();
                // // 创建一个gate Session,并且保存到SessionComponent中
                // Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(channelId, NetworkHelper.ToIPEndPoint(routerAddress));
                // gateSession.AddComponent<RouterDataComponent>().Gateid = r2CLogin.GateId;
                //
                // gateSession.AddComponent<PingComponent>();
                // zoneScene.AddComponent<SessionComponent>().Session = gateSession;
                //
                // G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(new C2G_LoginGate()
                // {
                //     Key = r2CLogin.Key, GateId = r2CLogin.GateId
                // });
                //
                // Log.Debug("登陆gate成功!");
                //
                // await Game.EventSystem.PublishAsync(new EventType.LoginFinish() { ZoneScene = zoneScene, Account = account });
                // callBack.Invoke(true);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            if (a2CLoginAccount.Error != ErrorCode.ERR_Success)
            {
                accountSession?.Dispose();
                return a2CLoginAccount.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = accountSession;
            // 心跳消息组件
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            AccountInfoComponent accountInfoComponent = zoneScene.GetComponent<AccountInfoComponent>();
            accountInfoComponent.Token = a2CLoginAccount.Token;
            accountInfoComponent.AccountId = a2CLoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> Register(Scene zoneScene, string address, string account, string password)
        {
            // 创建一个ETModel层的Session
            R2C_Register r2CRegister = null;
            Session session = null;
            try
            {
                long timerId = 0;
                try
                {
                    session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                    ETCancellationToken cancel = new ETCancellationToken();
                    timerId = TimerComponent.Instance.NewOnceTimer(TimeInfo.Instance.ClientNow() + 10000, TimerType.LoginTimeOut, cancel);
                    r2CRegister = (R2C_Register)await session.Call(new C2R_Register() { Account = account, Password = password },
                        cancel);
                }
                finally
                {
                    TimerComponent.Instance.Remove(ref timerId);
                    session?.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            if (r2CRegister.Error != ErrorCode.ERR_Success)
            {
                return r2CRegister.Error;
            }

            return ErrorCode.ERR_Success;
        }

        // 推出登录
        public static async ETTask Logout(Scene zoneScene, Action<bool> callBack)
        {
            try
            {
                Session gateSession = zoneScene.GetComponent<SessionComponent>().Session;
                await gateSession.Call(new C2G_Logout());
                gateSession.Dispose();
                zoneScene.RemoveComponent<SessionComponent>();

                callBack?.Invoke(true);
            }
            catch (Exception e)
            {
                Log.Error(e);
                callBack?.Invoke(false);
            }
        }
    }
}