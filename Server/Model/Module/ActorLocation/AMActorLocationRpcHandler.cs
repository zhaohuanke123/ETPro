using System;

namespace ET
{
    /// <summary>
    /// 提供处理Actor位置相关RPC请求的基础抽象类。
    /// 此类设计用于实现消息处理逻辑，特别是针对那些需要在特定Actor实例上执行的位置相关操作。
    /// 子类需实现<see cref="Run"/>方法以定义具体的业务处理过程。
    /// </summary>
    /// <typeparam name="E">继承自<see cref="Entity"/>的单元类型，代表处理请求的实体类型。</typeparam>
    /// <typeparam name="Request">继承自<see cref="IActorLocationRequest"/>的请求消息类型。</typeparam>
    /// <typeparam name="Response">继承自<see cref="IActorLocationResponse"/>的响应消息类型。</typeparam>
    [ActorMessageHandler]
    public abstract class AMActorLocationRpcHandler<E, Request, Response>: IMActorHandler where E : Entity
            where Request : class, IActorLocationRequest where Response : class, IActorLocationResponse
    {
        protected abstract ETTask Run(E unit, Request request, Response response, Action reply);

        /// <summary>
        /// 处理Actor位置相关的RPC请求的抽象基类。
        /// 实现了从接收的消息中提取请求与响应的逻辑，并调用子类定义的具体处理方法。
        /// </summary>
        public async ETTask Handle(Entity entity, object actorMessage, Action<IActorResponse> reply)
        {
            try
            {
                Request request = actorMessage as Request;
                if (request == null)
                {
                    Log.Error($"消息类型转换错误: {actorMessage.GetType().FullName} to {TypeInfo<Request>.TypeName}");
                    return;
                }

                E ee = entity as E;
                if (ee == null)
                {
                    Log.Error($"Actor类型转换错误: {entity.GetType().Name} to {TypeInfo<E>.TypeName} --{TypeInfo<Request>.TypeName}");
                    return;
                }

                int rpcId = request.RpcId;
                Response response = Activator.CreateInstance<Response>();

                void Reply()
                {
                    response.RpcId = rpcId;
                    reply.Invoke(response);
                }

                try
                {
                    await this.Run(ee, request, response, Reply);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                    response.Error = ErrorCore.ERR_RpcFail;
                    response.Message = exception.ToString();
                    Reply();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"解释消息失败: {actorMessage.GetType().FullName}", e);
            }
        }

        public Type GetRequestType()
        {
            return TypeInfo<Request>.Type;
        }

        public Type GetResponseType()
        {
            return TypeInfo<Response>.Type;
        }
    }
}