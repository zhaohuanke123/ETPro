﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    [FriendClass(typeof (UnitGateComponent))]
    public static class MessageHelper
    {
        public static void Broadcast(Unit unit, IActorMessage message, GhostComponent ghost = null)
        {
            if (ghost == null)
            {
                ghost = unit.GetComponent<AOIUnitComponent>()?.GetComponent<GhostComponent>();
            }

            if (ghost != null && !ghost.IsGoast)
            {
                unit.GetComponent<AOIUnitComponent>()?.GetComponent<GhostComponent>()?.HandleMsg(message);
            }

            (ushort opcode, MemoryStream stream) = MessageSerializeHelper.MessageToStream(message);
            foreach (var u in unit.GetBeSeeUnits())
            {
                SendToClient(u.GetParent<Unit>(), stream, opcode, message);
            }
        }

        public static void SendToClient(Unit unit, IActorMessage message)
        {
            if (unit.GetComponent<UnitGateComponent>() != null)
                SendActor(unit.GetComponent<UnitGateComponent>().GateSessionActorId, message);
        }

        private static void SendToClient(Unit unit, MemoryStream stream, ushort opcode, IActorMessage message)
        {
            if (unit.GetComponent<UnitGateComponent>() != null)
            {
                var actorId = unit.GetComponent<UnitGateComponent>().GateSessionActorId;
                if (actorId == 0)
                {
                    throw new Exception($"actor id is 0: {message}");
                }

                ProcessActorId processActorId = new ProcessActorId(actorId);
                Session session = NetInnerComponent.Instance.Get(processActorId.Process);
                OpcodeHelper.LogMsg(session.DomainZone(), opcode, message);
                session.Send(processActorId.ActorId, stream);
            }
        }

        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            ActorLocationSenderComponent.Instance.Send(id, message);
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long actorId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallLocationActor(long id, IActorLocationRequest message)
        {
            return await ActorLocationSenderComponent.Instance.Call(id, message);
        }

        public static void SendToPlayer(Player player, IMessage message)
        {
            player.Session.Send(message);
        }
    }
}