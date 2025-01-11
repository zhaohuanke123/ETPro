using System;
using MongoDB.Driver.Core.Events;

namespace ET
{
    [Timer(TimerType.AccountSessionCheckoutTime)]
    public class AccountSessionCheckOutTimer: ATimer<AccountCheckoutTimeComponent>
    {
        public override async void Run(AccountCheckoutTimeComponent self)
        {
            try
            {
                await self.DeleteSession();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    public class AccountCheckoutTimeComponentAwakeSystem: AwakeSystem<AccountCheckoutTimeComponent, long>
    {
        public override void Awake(AccountCheckoutTimeComponent self, long accountId)
        {
            self.AccountId = accountId;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckoutTime, self);
        }
    }

    public class AcccountCheckoutTimeComponentDestroySystem: DestroySystem<AccountCheckoutTimeComponent>
    {
        public override void Destroy(AccountCheckoutTimeComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    [FriendClass(typeof (ET.AccountCheckoutTimeComponent))]
    [FriendClassAttribute(typeof (ET.SessionPlayerComponent))] // public static class AccountCheckoutTimeComponentSystem
    public static class AccountCheckoutTimeComponentSystem
    {
        public static async ETTask DeleteSession(this AccountCheckoutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            AccountSessionsComponent accountSessionsComponent = session.DomainScene().GetComponent<AccountSessionsComponent>();
            long sessionInstanceId = accountSessionsComponent.Get(self.AccountId);
            if (session.InstanceId == sessionInstanceId)
            {
                accountSessionsComponent.Remove(self.AccountId);
            }

            string map = "Map1";
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), map);
            MapSceneConfig mapSceneConfig = MapSceneConfigCategory.Instance.Get(startSceneConfig.Id);

            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (sessionPlayerComponent == null)
            {
                session.DisConnect().Coroutine();
                return;
            }

            long playerId = sessionPlayerComponent.PlayerId;
            await TransferHelper.ExitMap(playerId, startSceneConfig.InstanceId, mapSceneConfig.Name);

            session.Send(new A2C_Disconnect() { Error = 0 });
            session.DisConnect().Coroutine();
        }
    }
}