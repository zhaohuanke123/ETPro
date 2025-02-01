using UnityEngine;

namespace ET
{
    [ComponentOf]
    public class ProjectileComponent : Entity, IAwake<Unit, float>, IDestroy
    {
        public Unit Target { get; set; }
        public float Speed { get; set; }
        public ETTask Tcs { get; set; }
    }
} 