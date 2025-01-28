using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof(ET.ChessBattleViewComponent))]
    public class EventOnGenChampion : AEventAsync<EventType.GenChampions>
    {
        protected override async ETTask Run(EventType.GenChampions args)
        {
            List<ChampionInfoPB> list = args.CPInfos;

            ChessBattleViewComponent.Instance.Clear();
            foreach (ChampionInfoPB infoPb in list)
            {
                ChampionConfig config = ChampionConfigCategory.Instance.Get(infoPb.ConfigId);
                GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.prefab);
                // Scene currentScene = args.zoneScene.CurrentScene();
                int index = infoPb.GridPositionX;
                GameObjectComponent gameObjectComponent = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(go,
                () =>
                {
                    GameObjectPoolComponent.Instance.RecycleGameObject(go);
                    ChessBattleViewComponent.Instance.ownChampionInventoryArray[infoPb.GridPositionX] = null;
                });
                ChessBattleViewComponent.Instance.Replace(gameObjectComponent, index);

                // Unit unit = args.unit;
                // unit.AddComponent(showView);
                // unit.AddComponent<MoveViewComponent>();

                ChampionControllerComponent championControllerComponent =
                        gameObjectComponent.AddComponent<ChampionControllerComponent, GameObjectComponent>(gameObjectComponent);
                CharacterControlComponent characterControlComponent = gameObjectComponent.AddComponent<CharacterControlComponent, GameObject>(go);
                HealBarComponent healBarComponent = characterControlComponent.GetComponent<HealBarComponent>();
                healBarComponent.SetVisible(false);

                championControllerComponent.Init(index);
                championControllerComponent.SetLevel(infoPb.Lv);
            }

            await ETTask.CompletedTask;
        }
    }
}
