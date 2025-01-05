using System;
using System.Text.RegularExpressions;

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
            if (!IsValidInput(account, password, out var errorCode))
            {
                return errorCode;
            }

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
                long channelId = RandomHelper.RandInt64();
                var routercomponent = zoneScene.AddComponent<GetRouterComponent, long, long>(a2CLoginAccount.GateId, channelId);
                string routerAddress = await routercomponent.Tcs;
                if (routerAddress == "")
                {
                    zoneScene.RemoveComponent<GetRouterComponent>();
                    throw new Exception("routerAddress 失败");
                }
                
                Log.Debug("routerAddress 获取成功:" + routerAddress);
                zoneScene.RemoveComponent<GetRouterComponent>();
                
                // 创建一个gate Session,并且保存到SessionComponent中
                Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(channelId, NetworkHelper.ToIPEndPoint(routerAddress));
                gateSession.AddComponent<RouterDataComponent>().Gateid = a2CLoginAccount.GateId;
                
                gateSession.AddComponent<PingComponent>();
                zoneScene.AddComponent<SessionComponent>().GateSession = gateSession;
                
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(new C2G_LoginGate()
                {
                    Key = a2CLoginAccount.Key, GateId = a2CLoginAccount.GateId
                });
                
                Log.Debug("登陆gate成功!");
                
                await Game.EventSystem.PublishAsync(new EventType.LoginFinish() { ZoneScene = zoneScene, Account = account });
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

            zoneScene.GetComponent<SessionComponent>().Session = accountSession;
            // 心跳消息组件
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            AccountInfoComponent accountInfoComponent = zoneScene.GetComponent<AccountInfoComponent>();
            accountInfoComponent.Token = a2CLoginAccount.Token;
            accountInfoComponent.AccountId = a2CLoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> Register(Scene zoneScene, string address, string account, string password)
        {
            if (!IsValidInput(account, password, out var errorCode))
            {
                return errorCode;
            }

            // 创建一个ETModel层的Session
            A2C_RegisterAccount a2CRegisterAccount = null;
            Session session = null;
            try
            {
                long timerId = 0;
                try
                {
                    session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                    ETCancellationToken cancel = new ETCancellationToken();
                    timerId = TimerComponent.Instance.NewOnceTimer(TimeInfo.Instance.ClientNow() + 10000, TimerType.LoginTimeOut, cancel);
                    password = MD5Helper.StringMD5(password);
                    a2CRegisterAccount = (A2C_RegisterAccount)await session.Call(
                        new C2A_RegisterAccount() { AccountName = account, Password = password },
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

            if (a2CRegisterAccount.Error != ErrorCode.ERR_Success)
            {
                return a2CRegisterAccount.Error;
            }

            return ErrorCode.ERR_Success;
        }

        // 推出登录
        public static async ETTask<int> Logout(Scene zoneScene)
        {
            A2C_LogoutResponse a2C_LogoutResponse = null;
            Session accountSession = null;
            try
            {
                accountSession = zoneScene.GetComponent<SessionComponent>().Session;
                a2C_LogoutResponse = (A2C_LogoutResponse)await accountSession.Call(new C2A_LogoutRequest());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            if (a2C_LogoutResponse.Error != ErrorCode.ERR_Success)
            {
                return a2C_LogoutResponse.Error;
            }

            accountSession.Dispose();
            return ErrorCode.ERR_Success;
        }

        private static bool IsValidInput(string account, string password, out int errorCode)
        {
            errorCode = ErrorCode.ERR_Success;
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                errorCode = ErrorCode.ERR_LoginInfoEmpty;
                return false;
            }

            if (!Regex.IsMatch(account.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                errorCode = ErrorCode.ERR_AccountNameFormError;
                return false;
            }

            return true;
        }
    }
}