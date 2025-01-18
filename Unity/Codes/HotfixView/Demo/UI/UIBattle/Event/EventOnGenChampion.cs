using System;
using UnityEngine;

namespace ET
{
    public class EventOnGenChampion: AEventAsync<EventType.GenChampion>
    {
        protected override async ETTask Run(EventType.GenChampion args)
        {
            ChampionConfig config = ChampionConfigCategory.Instance.Get(args.cPId);
            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.prefab);
            Scene currentScene = args.zoneScene.CurrentScene();
            GameObjectComponent showView = currentScene.AddChild<GameObjectComponent, GameObject, Action>(go,
                () => { GameObjectPoolComponent.Instance.RecycleGameObject(go); });

            ChampionController controller = go.GetComponent<ChampionController>();

            controller.Init(null, ChampionController.TEAMID_PLAYER);
            controller.gridType = Map.GRIDTYPE_OWN_INVENTORY;
            controller.gridPositionX = args.index;
            controller.SetWorldPosition();
            controller.SetWorldRotation();
            GamePlayController.Instance.ownChampionInventoryArray[args.index] = go;

            await ETTask.CompletedTask;
        }
    }
}