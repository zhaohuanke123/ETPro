using System;

namespace ET
{
    [ObjectSystem]
    public class ActorMessageDispatcherComponentAwakeSystem: AwakeSystem<ActorMessageDispatcherComponent>
    {
        public override void Awake(ActorMessageDispatcherComponent self)
        {
            ActorMessageDispatcherComponent.Instance = self;
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ActorMessageDispatcherComponentLoadSystem: LoadSystem<ActorMessageDispatcherComponent>
    {
        public override void Load(ActorMessageDispatcherComponent self)
        {
            self.Load();
        }
    }

    [ObjectSystem]
    public class ActorMessageDispatcherComponentDestroySystem: DestroySystem<ActorMessageDispatcherComponent>
    {
        public override void Destroy(ActorMessageDispatcherComponent self)
        {
            self.ActorMessageHandlers.Clear();
            ActorMessageDispatcherComponent.Instance = null;
        }
    }

    /// <summary>
    /// Actor消息分发组件
    /// </summary>
    [FriendClass(typeof (ActorMessageDispatcherComponent))]
    public static class ActorMessageDispatcherComponentHelper
    {
        public static void Awake(this ActorMessageDispatcherComponent self)
        {
            self.Load();
        }

        /// <summary>
        /// 加载Actor消息处理器到ActorMessageDispatcherComponent中。
        /// </summary>
        /// <param name="self">当前ActorMessageDispatcherComponent实例。</param>
        /// <exception cref="Exception">当消息处理器未继承IMActorHandler抽象类或响应类型错误时抛出异常。</exception>
        public static void Load(this ActorMessageDispatcherComponent self)
        {
            self.ActorMessageHandlers.Clear();

            var types = Game.EventSystem.GetTypes(TypeInfo<ActorMessageHandlerAttribute>.Type);
            foreach (Type type in types)
            {
                object obj = Activator.CreateInstance(type);

                IMActorHandler imHandler = obj as IMActorHandler;
                if (imHandler == null)
                {
                    throw new Exception($"message handler not inherit IMActorHandler abstract class: {obj.GetType().FullName}");
                }

                Type messageType = imHandler.GetRequestType();

                Type handleResponseType = imHandler.GetResponseType();
                if (handleResponseType != null)
                {
                    Type responseType = OpcodeTypeComponent.Instance.GetResponseType(messageType);
                    if (handleResponseType != responseType)
                    {
                        throw new Exception($"message handler response type error: {messageType.FullName}");
                    }
                }

                self.ActorMessageHandlers.Add(messageType, imHandler);
            }
        }

        /// <summary>
        /// 处理Actor消息。
        /// </summary>
        /// <param name="self">当前ActorMessageDispatcherComponent实例。</param>
        /// <param name="entity">接收到消息的实体。</param>
        /// <param name="message">需要处理的消息对象，应为IActorMessage接口的实现。</param>
        /// <param name="reply">回复Actor消息的回调函数，参数类型为IActorResponse。</param>
        /// <returns>一个表示异步操作的ETTask对象。</returns>
        /// <exception cref="Exception">如果找不到对应消息类型的处理器，则抛出异常。</exception>
        public static async ETTask Handle(this ActorMessageDispatcherComponent self, Entity entity, object message, Action<IActorResponse> reply)
        {
            if (!self.ActorMessageHandlers.TryGetValue(message.GetType(), out IMActorHandler handler))
            {
                throw new Exception($"not found message handler: {message}");
            }

            await handler.Handle(entity, message, reply);
        }

        /// <summary>
        /// 尝试从ActorMessageDispatcherComponent中获取指定类型的消息处理器。
        /// </summary>
        /// <param name="self">当前ActorMessageDispatcherComponent实例。</param>
        /// <param name="type">要获取的消息处理器的类型。</param>
        /// <param name="actorHandler">输出参数，找到的消息处理器实例；如果未找到，则为默认值(null)。</param>
        /// <returns>如果找到指定类型的消息处理器则返回true，否则返回false。</returns>
        public static bool TryGetHandler(this ActorMessageDispatcherComponent self, Type type, out IMActorHandler actorHandler)
        {
            return self.ActorMessageHandlers.TryGetValue(type, out actorHandler);
        }
    }
}