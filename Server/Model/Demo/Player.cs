﻿namespace ET
{
    [ChildOf(typeof (PlayerComponent))]
    public sealed class Player: Entity, IAwake<string>
    {
        public string Account { get; set; }

        public long UnitId { get; set; }

        public Session Session { get; set; }
    }
}