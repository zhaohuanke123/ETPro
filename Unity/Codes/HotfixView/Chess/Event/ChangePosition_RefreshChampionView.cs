using ET.EventType;
using UnityEngine;

namespace ET
{
    public class ChangePosition_RefreshChampionView: AEvent<CPChangePosition>
    {
        protected override void Run(CPChangePosition args)
        {
            GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }

            Transform transform = gameObjectComponent.GameObject.transform;
            transform.position = args.Unit.ViewPosition;
        }
    }
}