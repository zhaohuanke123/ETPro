namespace ET
{
    [FriendClass(typeof (TokenComponent))]
    public static class TokenComponentSystem
    {
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.accountDic.Add(key, token);

            self.TimeOutRemoveKey(key, token).Coroutine();
        }

        public static string Get(this TokenComponent self, long key)
        {
            return self.accountDic.TryGetValue(key, out string token)? token : null;
        }

        public static void Remove(this TokenComponent self, long key)
        {
            if (self.accountDic.ContainsKey(key))
            {
                self.accountDic.Remove(key);
            }
        }

        private static async ETTask TimeOutRemoveKey(this TokenComponent self, long key, string tokenKey)
        {
            await TimerComponent.Instance.WaitAsync(600000);

            string onlineToken = self.Get(key);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
            {
                self.Remove(key);
            }
        }
    }
}