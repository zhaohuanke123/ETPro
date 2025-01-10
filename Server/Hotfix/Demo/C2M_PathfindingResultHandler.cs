using UnityEngine;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_PathfindingResultHandler: AMActorLocationHandler<Unit, C2M_PathfindingResult>
    {
        protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
        {
            if (!unit.GetComponent<MoveComponent>().Enable)
            {
                //被行为禁止
                return;
            }

            Vector3 target = new Vector3(message.X, message.Y, message.Z);

            unit.FindPathMoveToAsync(target).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}