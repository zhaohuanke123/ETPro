using System;

namespace ET
{
	public static class MatchHelper
	{
		public static async ETTask StartMatch(Scene zoneScene)
		{
			G2C_StartMatch g2CStartMatch = null;
			try
			{
				ETCancellationToken cancel = new ETCancellationToken();
				g2CStartMatch = (G2C_StartMatch)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_StartMatch(), cancel);
			}
			catch (Exception e)
			{
				Log.Error(e);
				return;
			}

			if (g2CStartMatch.Error != ErrorCode.ERR_Success)
			{
				Log.Error($"StartMatch error : {g2CStartMatch.Error.ToString()}");
				return;
			}
		}
	}
}
