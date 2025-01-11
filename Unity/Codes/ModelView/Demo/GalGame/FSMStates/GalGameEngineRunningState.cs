namespace ET
{

    /// <summary>
    /// 运行
    /// </summary>
    [ComponentOf]
    public class GalGameEngineRunningState : Entity,IAwake<FSMComponent>,IAwake
    {
        public FSMComponent FSM;
        public GalGameEngineComponent Engine;
        public ChapterCategory ChapterCategory;
        public bool stop;
        public bool isRunning;
    }
}