namespace ET
{
    [ComponentOf]
    public class ChampionControllerComponent: Entity, IAwake<GameObjectComponent>, IDestroy
    {
        public ChampionController championController;
    }
}