using System.Collections.Generic;

namespace ET
{
    [ComponentOf()]
    public class AccountInfoComponent: Entity, IAwake, IDestroy
    {
        public string Token { get; set; }
        public long AccountId { get; set; }
    }
}