using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class ToastComponent:Entity,IAwake,IDestroy
    {
        public static ToastComponent Instance;
        public Transform root; 
    }
}
