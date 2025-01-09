using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof (MoveComponent))]
    public static class TransferHelper
    {
        /// <summary>
        /// 协助进行单位转移至指定场景的功能方法。
        /// </summary>
        /// <param name="unit">需要转移的游戏单元。此单元将被移动到新的场景。</param>
        /// <param name="sceneInstanceId">目标场景的实例ID，用于定位到具体要进入的场景实例。</param>
        /// <param name="sceneName">目标场景的名称，与实例ID一起定义转移目的地。</param>
        /// <returns>返回一个代表转移单元异步操作任务的<see cref="ETTask"/>对象。</returns>
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

        /// <summary>
        /// 异步执行单位区域转移的功能方法。
        /// 此方法负责将单位从当前场景转移至指定场景实例，内部处理相关的数据迁移逻辑。
        /// </summary>
        /// <param name="aoiU">单位的AOI组件，用于获取单位及其相关上下文信息。</param>
        /// <param name="sceneInstanceId">目标场景实例的ID，单位将被转移到这个场景中。</param>
        /// <returns>返回代表该转移操作的异步任务<see cref="ETTask"/>，在操作完成后（无论成功或失败）可等待此任务。</returns>
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