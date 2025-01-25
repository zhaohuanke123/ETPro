namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class CpCombatComponent: Entity, IAwake, IDestroy, IFixedUpdate
    {
        public Unit target;
    }
}