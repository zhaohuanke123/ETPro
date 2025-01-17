namespace ET
{
    public static class ChessBattleHelper
    {
        public static async ETTask RefreshShopChampionList(Scene zoneScene)
        {
            Session session = zoneScene.GetComponent<SessionComponent>().Session;
            G2C_RefreshShop g2CRefreshShop = await session.Call(new C2G_RefreshShop()) as G2C_RefreshShop;

            if (g2CRefreshShop.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            Game.EventSystem.Publish(new UIEventType.RefreshShop() { championIds = g2CRefreshShop.championIds });
        }

        public static async ETTask TryBuyChampion(int index)
        {
            Session session = Game.Scene.GetComponent<SessionComponent>().Session;
            C2G_BuyChampion request = new C2G_BuyChampion();
            request.slopIndex = index;

            G2C_BuyChampion response = await session.Call(request) as G2C_BuyChampion;

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            int responseCpId = response.cpId;
            Game.EventSystem.Publish(new EventType.GenChampion() { cpId = responseCpId });
        }
    }
}