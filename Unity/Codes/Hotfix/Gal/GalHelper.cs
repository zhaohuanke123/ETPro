namespace ET
{
    public static class GalHelper
    {
        public static async ETTask<int> GetNextGalId(Scene zoneScene)
        {
            G2C_GetNextGalId reponse = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_GetNextGalId()) as G2C_GetNextGalId;

            if (reponse.Error != ErrorCode.ERR_Success)
            {
                return -1;
            }

            return reponse.GalId;
        }

        public static async ETTask<int> PassGal(Scene zoneScene)
        {
            G2C_PassGal reponse = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_PassGal()) as G2C_PassGal;

            if (reponse.Error != ErrorCode.ERR_Success)
            {
                return -1;
            }
            
            return reponse.NextGalId;
        }
    }
}