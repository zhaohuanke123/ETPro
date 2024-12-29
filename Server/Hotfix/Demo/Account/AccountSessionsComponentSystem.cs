using System.Collections.Generic;

namespace ET
{
    public class AccountSessionsComponentDestorySystem: DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }

    [FriendClass(typeof(AccountSessionsComponent))]
    public static class AccountSessionsComponentSystem
    {
        public static long Get(this AccountSessionsComponent self, long accountId)
        {
            return self.AccountSessionDictionary.GetValueOrDefault(accountId, 0);
        }

        public static void Add(this AccountSessionsComponent self, long accountId, long sessionInstanceId)
        {
            self.AccountSessionDictionary[accountId] = sessionInstanceId;
        }

        public static void Remove(this AccountSessionsComponent self, long accountId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary.Remove(accountId);
            }
        }
    }
}