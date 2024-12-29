using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent: Entity, IAwake
    {
        /// <summary>
        ///  key: accountId, value: token
        /// </summary>
        public readonly Dictionary<long, string> accountDic = new Dictionary<long, string>();
    }
}