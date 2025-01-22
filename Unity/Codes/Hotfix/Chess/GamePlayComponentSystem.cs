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
    public class GamePlayFixedUpdateSystem: FixedUpdateSystem<GamePlayComponent>
    {
        public override void FixedUpdate(GamePlayComponent self)
        {
            Log.Warning("GamePlayComponent FixedUpdate");
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