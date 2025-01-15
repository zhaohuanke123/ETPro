using System;

namespace ET
{
    public class C2G_EnterChessMapHandler: AMRpcHandler<C2G_EnterChessMap, G2C_EnterChessMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterChessMap request, G2C_EnterChessMap response, Action reply)
        {
            string map = "Main";
            // StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), map);

            //TODO: 临时
            response.SceneInstanceId = 123;
            response.SceneName = map;

            reply();
            await ETTask.CompletedTask;
        }
    }
}