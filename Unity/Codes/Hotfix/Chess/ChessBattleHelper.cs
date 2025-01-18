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

        public static async ETTask TryBuyChampion(Scene zoneScene, int index)
        {
            Session session = zoneScene.GetComponent<SessionComponent>().Session;
            C2G_BuyChampion request = new C2G_BuyChampion();
            request.SlopIndex = index;

            G2C_BuyChampion response = await session.Call(request) as G2C_BuyChampion;

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            int responseCpId = response.CPId;
            await Game.EventSystem.PublishAsync(new EventType.GenChampion()
            {
                zoneScene = zoneScene, cPId = responseCpId, index = response.InventoryIndex
            });
        }
    }
}