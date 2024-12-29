using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class LoginInfoRecordComponentAwakeSystem: AwakeSystem<LoginInfoRecordComponent>
    {
        public override void Awake(LoginInfoRecordComponent self)
        {
        }
    }

    [ObjectSystem]
    public class LoginInfoRecordComponentDestroySystem: DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict.Clear();
        }
    }

    [FriendClass(typeof (LoginInfoRecordComponent))]
    public static partial class LoginInfoRecordComponentSystem
    {
        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            self.AccountLoginInfoDict[key] = value;
        }

        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            self.AccountLoginInfoDict.Remove(key);
        }

        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.GetValueOrDefault(key, 0);
        }

        public static bool IsExit(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }
    }
}