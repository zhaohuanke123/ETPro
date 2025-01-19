using UnityEngine;

namespace ET
{
    [ComponentOf]
    public class InputControlComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public int triggerLayer;

        public Camera mainCamera;
        public TriggerInfo triggerInfo = null;

        public Vector3 rayCastStartPosition;

        public Vector3 mousePosition;
    }
}