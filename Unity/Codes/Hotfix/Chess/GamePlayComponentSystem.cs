namespace ET.Chess
{
    [ObjectSystem]
    public class GamePlayAwakeSystem: AwakeSystem<GamePlayComponent>
    {
        public override void Awake(GamePlayComponent self)
        {
            self.currentGameStage = GameStage.Preparation;
        }
    }

    [ObjectSystem]
    public class GamePlayUpdateSystem: UpdateSystem<GamePlayComponent>
    {
        public override void Update(GamePlayComponent self)
        {
    
        }
    }

    [ObjectSystem]
    public class GamePlayDestroySystem: DestroySystem<GamePlayComponent>
    {
        public override void Destroy(GamePlayComponent self)
        {
        }
    }

    [FriendClass(typeof (GamePlayComponent))]
    public static class GamePlayComponentSystemSystem
    {
    
    }
}