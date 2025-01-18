using UnityEngine;

namespace ET
{
    public class EventOnGenChampion: AEventAsync<EventType.GenChampion>
    {
        protected override async ETTask Run(EventType.GenChampion args)
        {
            ChampionConfig config = ChampionConfigCategory.Instance.Get(args.cpId);
            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.prefab);
            ChampionController controller = go.GetComponent<ChampionController>();
            controller.Init(null, ChampionController.TEAMID_PLAYER);
            controller.gridType = Map.GRIDTYPE_OWN_INVENTORY;
            controller.gridPositionX = 0;
            controller.SetWorldPosition();

            // controller.SetWorldPosition(Map.GRIDTYPE_OWN_INVENTORY, 0,0);

            Log.Debug(go.ToString());
            await ETTask.CompletedTask;
        }
    }
}