namespace ET
{
    [ComponentOf(typeof (Unit))]
    public class SendUniPosComponent: Entity, IAwake<Player>, IDestroy, IFixedUpdate
    {
        public Unit selfUnit;
        public Player toPlayer;
        public bool isSendArrived = false;
    }
}