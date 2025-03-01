using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClassAttribute(typeof(ET.Unit))]
    public class EventOnGenChampionView : AEventAsync<EventType.GenChampionView>
    {
        protected override async ETTask Run(EventType.GenChampionView args)
        {
            ChessBattleViewComponent.Instance.HideAllInMap();
            Unit unit = args.unit;

            unit.ViewPosition = unit.Position;
            unit.ViewRotation = unit.Rotation;
            unit.AddComponent<MoveViewComponent>();

            ChampionInfoPB infoPb = args.ChampionInfoPb;
            ChampionConfig config = ChampionConfigCategory.Instance.Get(infoPb.ConfigId);
            GameObject go = await GameObjectPoolComponent.Instance.GetGameObjectAsync(config.prefab);
            GameObjectComponent showView = ChessBattleViewComponent.Instance.AddChild<GameObjectComponent, GameObject, Action>(go,
            () =>
            {
                GameObjectPoolComponent.Instance.RecycleGameObject(go);
            });

            unit.AddComponent(showView);
            unit.AddComponent<CharacterControlComponent, GameObject>(go);
            unit.ViewReadTask.SetResult();
            // showView.AddComponent<CpAnimatorComponent>();

            int lv = infoPb.Lv;
            float newScale = 1;
            if (lv == 2)
            {
                newScale = 1.5f;
            }
            else if (lv == 3)
            {
                newScale = 2f;
            }

            go.transform.localScale = new Vector3(newScale, newScale, newScale);
            go.transform.position = unit.ViewPosition;

            await ETTask.CompletedTask;
        }
    }
}
