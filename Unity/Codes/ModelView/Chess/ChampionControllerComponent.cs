using UnityEngine;

namespace ET
{
    [ComponentOf]
    public class ChampionControllerComponent: Entity, IAwake<GameObjectComponent>, IDestroy, IUpdate
    {
        public Transform transform;
        public Vector3 gridTargetPosition;
        public bool _isDragged;

        public int gridType = 0;
        public int gridPositionX = 0;
        public int gridPositionZ = 0;
        public int teamID;

        public ChampionController championController;
    }
}