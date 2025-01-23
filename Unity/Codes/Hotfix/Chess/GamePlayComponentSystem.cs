using MongoDB.Driver.Core.Events;

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
            // Log.Warning("GamePlayComponent FixedUpdate");
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
        public static void AddPlayer(this GamePlayComponent self, Player player)
        {
            ShopComponent shopComponent = self.GetComponent<ShopComponent>();
            shopComponent.AddPlayer(player);
            ChampionArrayComponent championArrayComponent = self.GetComponent<ChampionArrayComponent>();
            shopComponent.AddPlayer(player);
            ChampionMapArrayComponent championMapArrayComponent = self.GetComponent<ChampionMapArrayComponent>();
            championMapArrayComponent.AddPlayer(player);
        }
    }
}