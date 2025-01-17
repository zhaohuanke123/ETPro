namespace ET
{
    /// <summary>
    /// 场景切换辅助类，提供场景切换相关的便捷方法。
    /// </summary>
    public static class SceneChangeHelper
    {
        private static WaitType.Wait_CreateMyUnit waitCreateMyUnit { get; set; }

        public static async ETTask SceneChangeTo(Scene zoneScene, string sceneName, long sceneInstanceId)
        {
            // zoneScene.RemoveComponent<AIComponent>();
            zoneScene.AddComponent<KeyCodeComponent>();
            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的

            Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, zoneScene.Zone, sceneName, currentScenesComponent);
            currentScene.AddComponent<UnitComponent>();
            currentScene.AddComponent<AOISceneComponent, int>(16);

            // 可以订阅这个事件中创建Loading界面
            using (ListComponent<ETTask> tasks = ListComponent<ETTask>.Create())
            {
                tasks.Add(WaitChangeScene(zoneScene, sceneName)); //等收到消息并且地图创建完成
                tasks.Add(WaitCreateMyUnit(zoneScene)); // 等待CreateMyUnit的消息
                await ETTaskHelper.WaitAll(tasks);
            }

            M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
            var task = zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_LoadAOISceneFinish>(); //先声明出来

            UnitFactory.Create(currentScene, m2CCreateMyUnit.Unit); //添加AOI组件时会Wait_LoadAOISceneFinish

            await task;

            await Game.EventSystem.PublishAsync(new EventType.SceneChangeFinish() { ZoneScene = zoneScene, CurrentScene = currentScene });

            // 通知等待场景切换的协程
            zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_SceneChangeFinish());
        }

        public static async ETTask WaitChangeScene(Scene zoneScene, string sceneName)
        {
            await Game.EventSystem.PublishAsync(new EventType.SceneChangeStart() { ZoneScene = zoneScene, Name = sceneName });
        }

        public static async ETTask WaitCreateMyUnit(Scene zoneScene)
        {
            waitCreateMyUnit = await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnit>();
        }

        public static async ETTask SceneChangeToLogin(Scene zoneScene)
        {
            // 1. 清理场景
            zoneScene.RemoveComponent<KeyCodeComponent>();
            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose();

            await Game.EventSystem.PublishAsync(new EventType.SceneChangeLoginBegin() { });

            await Game.EventSystem.PublishAsync(new EventType.SceneChangeLoginFinish() { });

            zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_SceneChangeFinish());
        }

        public static async ETTask SceneChangeToChessMap(Scene zoneScene, string sceneName, long sceneInstanceId)
        {
            if (zoneScene.GetComponent<KeyCodeComponent>() == null)
            {
                zoneScene.AddComponent<KeyCodeComponent>();
            }

            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的

            Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, zoneScene.Zone, sceneName, currentScenesComponent);

            await Game.EventSystem.PublishAsync(new EventType.SceneChangeChessMapStart() { ZoneScene = zoneScene, Name = sceneName });

            await Game.EventSystem.PublishAsync(new EventType.SceneChangeChessMapFinish() { ZoneScene = zoneScene, CurrentScene = currentScene });
        }
    }
}