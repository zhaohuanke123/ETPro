﻿namespace ET
{
    /// <summary>
    /// 快进
    /// </summary>
    public class GalGameEngineFastForwardState : Entity,IAwake
    {
        public FSMComponent FSM;
        public bool BaseAutoPlay;
        public GalGameEngineRunningState Base;
    }
}
