﻿using UnityEngine.Events;

namespace ET
{
    [UIComponent]
    public class UIPointerClick : Entity,IAwake,IOnCreate,IOnEnable
    {
        public UnityAction onClick;
        public PointerClick pointerClick;
    }
}