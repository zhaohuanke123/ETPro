﻿using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (MoveComponent))]
    public static class TransferHelper
    {
        /// <summary>
        /// 异步执行单位转移至指定场景的操作
        /// </summary>
        /// <param name="unit">待转移的游戏单位</param>
        /// <param name="sceneInstanceId">目标场景实例ID</param>
        /// <param name="sceneName">目标场景名称</param>
        /// <returns>一个表示异步操作的任务</returns>
        public static async ETTask Transfer(Unit unit, long sceneInstanceId, string sceneName)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Transfer, unit.Id))
            {
                if (unit.IsDisposed || unit.IsGhost())
                {
                    return;
                }

                // 通知客户端开始切场景
                M2C_StartSceneChange m2CStartSceneChange = new M2C_StartSceneChange() { SceneInstanceId = sceneInstanceId, SceneName = sceneName };
                MessageHelper.SendToClient(unit, m2CStartSceneChange);

                M2M_UnitTransferRequest request = new M2M_UnitTransferRequest();
                ListComponent<int> Stack = ListComponent<int>.Create();
                request.Unit = unit;

                Entity curEntity = unit;
                Stack.Add(-1);
                while (Stack.Count > 0)
                {
                    int index = Stack[Stack.Count - 1];
                    if (index != -1)
                    {
                        curEntity = request.Entitys[index];
                    }

                    Stack.RemoveAt(Stack.Count - 1);
                    foreach (Entity entity in curEntity.Components.Values)
                    {
                        if (entity is ITransfer)
                        {
                            int childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 0 });
                        }
                    }

                    foreach (Entity entity in curEntity.Children.Values)
                    {
                        if (entity is ITransfer)
                        {
                            int childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 1 });
                        }
                    }
                }

                Stack.Dispose();

                // 删除Mailbox,让发给Unit的ActorLocation消息重发
                unit.RemoveComponent<MailBoxComponent>();

                long oldInstanceId = unit.InstanceId;
                // location加锁
                await LocationProxyComponent.Instance.Lock(unit.Id, unit.InstanceId);

                M2M_UnitTransferResponse response =
                        response = await ActorMessageSenderComponent.Instance.Call(sceneInstanceId, request) as M2M_UnitTransferResponse;

                await LocationProxyComponent.Instance.UnLock(unit.Id, oldInstanceId, response.NewInstanceId);

                unit.RemoveComponent<UnitGateComponent>(); //先移除，防止AOI销毁的消息发到了客户端

                Log.Info(unit.Id + " Dispose " + unit.DomainScene().Id);
                unit.Dispose();
            }
        }

        public static async ETTask ExitMap(long playerId, long sceneInstanceId, string sceneName)
        {
            G2M_UnitExitMap g2MUnitExitMapHandler = new G2M_UnitExitMap();

            g2MUnitExitMapHandler.PlyerId = playerId;

            ActorMessageSenderComponent.Instance.Send(sceneInstanceId, g2MUnitExitMapHandler);

            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 异步执行游戏单位区域转移的方法
        /// </summary>
        /// <param name="aoiU">待转移单位的AOI组件</param>
        /// <param name="sceneInstanceId">目标场景实例ID</param>
        /// <returns>表示异步转移操作的任务</returns>
        public static async ETTask AreaTransfer(AOIUnitComponent aoiU, long sceneInstanceId)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Transfer, aoiU.Id))
            {
                if (aoiU.IsDisposed || aoiU.IsGhost()) return;
                Unit unit = aoiU.GetParent<Unit>();
                //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
                M2M_UnitAreaTransferRequest request = new M2M_UnitAreaTransferRequest();
                ListComponent<int> Stack = ListComponent<int>.Create();
                request.Unit = unit;
                Entity curEntity = unit;
                Stack.Add(-1);
                while (Stack.Count > 0)
                {
                    int index = Stack[Stack.Count - 1];
                    if (index != -1)
                    {
                        curEntity = request.Entitys[index];
                    }

                    Stack.RemoveAt(Stack.Count - 1);
                    foreach (Entity entity in curEntity.Components.Values)
                    {
                        if (entity is ITransfer)
                        {
                            int childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 0 });
                        }
                    }

                    foreach (Entity entity in curEntity.Children.Values)
                    {
                        if (entity is ITransfer)
                        {
                            int childIndex = request.Entitys.Count;
                            request.Entitys.Add(entity);
                            Stack.Add(childIndex);
                            request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 1 });
                        }
                    }
                }

                Stack.Dispose();
                // 删除Mailbox,让发给Unit的ActorLocation消息重发
                unit.RemoveComponent<MailBoxComponent>();

                long oldInstanceId = unit.InstanceId;
                // location加锁
                await LocationProxyComponent.Instance.Lock(unit.Id, unit.InstanceId);
                aoiU.GetComponent<GhostComponent>().IsGoast = true;
                M2M_UnitAreaTransferResponse response =
                        await ActorMessageSenderComponent.Instance.Call(sceneInstanceId, request) as M2M_UnitAreaTransferResponse;
                await LocationProxyComponent.Instance.UnLock(unit.Id, oldInstanceId, response.NewInstanceId);
            }
        }

        /// <summary>
        /// 大地图到边缘注册到其他地图
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sceneInstanceId"></param>
        public static void AreaAdd(Unit unit, long sceneInstanceId)
        {
            //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
            M2M_UnitAreaAdd request = new M2M_UnitAreaAdd();
            ListComponent<int> Stack = ListComponent<int>.Create();
            request.Unit = unit;
            Entity curEntity = unit;
            Stack.Add(-1);
            while (Stack.Count > 0)
            {
                int index = Stack[Stack.Count - 1];
                if (index != -1)
                {
                    curEntity = request.Entitys[index];
                }

                Stack.RemoveAt(Stack.Count - 1);
                foreach (Entity entity in curEntity.Components.Values)
                {
                    if (entity is ITransfer)
                    {
                        int childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 0 });
                    }
                }

                foreach (Entity entity in curEntity.Children.Values)
                {
                    if (entity is ITransfer)
                    {
                        int childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 1 });
                    }
                }
            }

            Stack.Dispose();
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    request.MoveInfo = new MoveInfo();
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        Vector3 pos = moveComponent.Targets[i];
                        request.MoveInfo.X.Add(pos.x);
                        request.MoveInfo.Y.Add(pos.y);
                        request.MoveInfo.Z.Add(pos.z);
                    }
                }
            }

            ActorMessageSenderComponent.Instance.Send(sceneInstanceId, request);
        }

        /// <summary>
        /// 在其他区域创建
        /// </summary>
        /// <param name="aoiU"></param>
        /// <param name="sceneInstanceId"></param>
        public static void AreaCreate(AOIUnitComponent aoiU, long sceneInstanceId)
        {
            Unit unit = aoiU.GetParent<Unit>();
            aoiU.GetComponent<GhostComponent>().IsGoast = true;
            //由于是一步步移动过去的，所以不涉及客户端加载场景，服务端自己内部处理好数据转移就好
            M2M_UnitAreaCreate request = new M2M_UnitAreaCreate();
            ListComponent<int> Stack = ListComponent<int>.Create();
            request.Unit = unit;
            Entity curEntity = unit;
            Stack.Add(-1);
            while (Stack.Count > 0)
            {
                int index = Stack[Stack.Count - 1];
                if (index != -1)
                {
                    curEntity = request.Entitys[index];
                }

                Stack.RemoveAt(Stack.Count - 1);
                foreach (Entity entity in curEntity.Components.Values)
                {
                    if (entity is ITransfer)
                    {
                        int childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 0 });
                    }
                }

                foreach (Entity entity in curEntity.Children.Values)
                {
                    if (entity is ITransfer)
                    {
                        int childIndex = request.Entitys.Count;
                        request.Entitys.Add(entity);
                        Stack.Add(childIndex);
                        request.Map.Add(new RecursiveEntitys { ChildIndex = childIndex, ParentIndex = index, IsChild = 1 });
                    }
                }
            }

            Stack.Dispose();

            ActorMessageSenderComponent.Instance.Send(sceneInstanceId, request);
        }
    }
}