namespace ET
{
    [ComponentOf()]
    public class GalComponent: Entity, IAwake, IDestroy
    {
        public int nextGalId = 1;
    }
}