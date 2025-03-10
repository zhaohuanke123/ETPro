﻿using System;
using System.Collections.Generic;
using ET.EventType;
using UnityEngine;

namespace ET
{
    [FriendClassAttribute(typeof(ET.GamePlayComponent))]
    [FriendClassAttribute(typeof(ET.Unit))]
    public class G2C_CreateCpUnitsHandler : AMHandler<G2C_CreateCpUnits>
    {
        protected override async void Run(Session session, G2C_CreateCpUnits message)
        {
            GamePlayComponent.Instance.isViewReadyTask = ETTask.Create();
            Scene currentScene = session.ZoneScene().CurrentScene();
            if (currentScene == null)
            {
                Log.Error($"当前场景为空，无法创建单位");
            }

            GamePlayComponent gamePlayComponent = currentScene.GetComponent<GamePlayComponent>();
            gamePlayComponent.currentGameStage = GameStage.Combat;
            Map.Instance.HideIndicators();

            Game.EventSystem.PublishAsync(new UIEventType.ShowToast()
            {
                Text = "战斗开始",
                showTime = 1,
            }).Coroutine();

            GamePlayComponent.Instance.championConfigDict.Clear();
            List<UnitInfo> units = message.Units;

            List<ETTask> tasks = new List<ETTask>();
            for (int i = 0; i < units.Count; i++)
            {
                UnitInfo unitInfo = units[i];
                ChampionInfoPB championInfoPb = message.ChampionInfoPBList[i];
                ChampionConfig config = ChampionConfigCategory.Instance.Get(championInfoPb.ConfigId);
                Unit unit = UnitFactory.Create(currentScene, unitInfo);
                GamePlayComponent.Instance.championConfigDict[unit] = config;

                //TODO 临时处理
                if (message.IsPlayer1 == false)
                {
                    session.ZoneScene().CurrentScenesComponent().Camp = Camp.Player2;
                    championInfoPb.GridPositionX = GPDefine.HexMapSizeX - 1 - championInfoPb.GridPositionX;
                    championInfoPb.GridPositionZ = GPDefine.HexMapSizeZ - 1 - championInfoPb.GridPositionZ;
                    unit.Position = MapComponent.Instance.GetMapPosition(championInfoPb.GridPositionX, championInfoPb.GridPositionZ);

                    if (unitInfo.Camp == (int)Camp.Player1)
                    {
                        unit.Rotation = Quaternion.Euler(new Vector3(unit.Rotation.x, 20, unit.Rotation.z));
                    }
                    else
                    {
                        unit.Rotation = Quaternion.Euler(new Vector3(unit.Rotation.x, 200, unit.Rotation.z));
                    }
                }

                unit.ViewReadTask = ETTask.Create();
                tasks.Add(EventSystem.Instance.PublishAsync(new GenChampionView()
                {
                    unit = unit,
                    ChampionInfoPb = championInfoPb
                }));
            }

            await ETTaskHelper.WaitAll(tasks);
            GamePlayComponent.Instance.isViewReadyTask.SetResult();
        }
    }
}
