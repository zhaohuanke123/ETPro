namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AOISceneComponent:Entity,IAwake<int>,IDestroy,IUpdate
    {
        public int gridLen { get; set; }
        public float halfDiagonal;
    }
    
}