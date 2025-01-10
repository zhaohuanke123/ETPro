using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// Actor消息调度器组件
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class ActorMessageDispatcherComponent: Entity, IAwake, IDestroy, ILoad
    {
        public static ActorMessageDispatcherComponent Instance;

        public readonly Dictionary<Type, IMActorHandler> ActorMessageHandlers = new Dictionary<Type, IMActorHandler>();
    }
}