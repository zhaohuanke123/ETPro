using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class FSMComponent:Entity,IAwake,IDestroy
    {
        public Dictionary<Type,Entity> m_dic = new Dictionary<Type, Entity>(); //状态机维护的一组状态
        public Entity CurrentState; //当前状态
    }
}
