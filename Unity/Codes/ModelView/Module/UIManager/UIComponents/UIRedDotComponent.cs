using UnityEngine;

namespace ET
{
    [UIComponent]
    public class UIRedDotComponent:Entity,IAwake,IOnCreate<string>,IOnCreate<string,Vector2>,IOnEnable
    {
        public GameObject tempObj;
        public RedDotMonoView reddot;
        public Vector3 scaler;
        public Vector2 positionOffset;
        public string target;
        public bool isRedDotActive = false;
    }
}
