

using System;

namespace ET
{
    [MessageHandler]
    [FriendClassAttribute(typeof(ET.UILobbyView))]
    public class G2C_NotifyFirstGalHandler : AMHandler<G2C_NotifyFirstGal>
    {
        protected override void Run(Session session, G2C_NotifyFirstGal message)
        {
            GalConfig galConfig = GalConfigCategory.Instance.Get(1);
            GalGameEngineComponent.Instance.PlayChapterByName(galConfig.ChapterName,
            async res =>
            {
                if (res)
                {
                    int nextId = await GalHelper.PassGal(session.ZoneScene());
                    UILobbyView uiLobbyView = UIManagerComponent.Instance.GetWindow<UILobbyView>();
                    if (uiLobbyView != null)
                    {
                        uiLobbyView.nextGalId = nextId;
                        uiLobbyView.RefreshGalBtns();
                    }
                }
            }).Coroutine();
        }
    }
}
