using UnityEngine;

namespace ET
{
    [ComponentOf]
    public class MapComponent: Entity, IAwake, IDestroy
    {
        public Vector3[] ownInventoryGridPositions;

        // public Vector3[] oponentInventoryGridPositions;

        public Vector3[,] mapGridPositions;

        public Vector3 mapStartPoint;
    }
}