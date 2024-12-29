using System;
using MongoDB.Driver.Core.Events;

namespace ET
{
    [Timer(TimerType.AccountSessionCheckoutTime)]
    public class AccountSessionCheckOutTimer: ATimer<AccountCheckoutTimeComponent>
    {
        public override void Run(AccountCheckoutTimeComponent self)
        {
            try
            {
                self.DeleteSession();
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
    [FriendClassAttribute(typeof(ET.AccountCheckoutTimeComponent))]
    // public static class AccountCheckoutTimeComponentSystem

    public static class AccountCheckoutTimeComponentSystem
    {
        public static void DeleteSession(this AccountCheckoutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            AccountSessionsComponent accountSessionsComponent = session.DomainScene().GetComponent<AccountSessionsComponent>();
            long sessionInstanceId = accountSessionsComponent.Get(self.AccountId);
            if (session.InstanceId == sessionInstanceId)
            {
                accountSessionsComponent.Remove(self.AccountId);
            }

            session.Send(new A2C_Disconnect() { Error = 0 });
            session.DisConnect().Coroutine();
        }
    }
}