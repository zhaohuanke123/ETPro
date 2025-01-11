namespace ET
{
    [MessageHandler]
    public class M2C_StartSceneChangeToLoginHandler: AMHandler<M2C_StartSceneChangeToLogin>
    {
        protected override void Run(Session session, M2C_StartSceneChangeToLogin message)
        {
            SceneChangeHelper.SceneChangeToLogin(session.ZoneScene()).Coroutine();
        }
    }
}