namespace ET
{
	[FriendClassAttribute(typeof (ET.ChampionInfo))]
	public static class ChampionInfoHelper
	{
		public static void SetPos(this ChampionInfo self, int x, int z)
		{
			self.gridPositionX = x;
			self.gridPositionZ = z;
		}

		public static void SetConfigId(this ChampionInfo self, int configId)
		{
			self.configId = configId;
		}

		public static ChampionInfoPB GetChampionInfoPb(this ChampionInfo self)
		{
			ChampionInfoPB championInfoPb = new ChampionInfoPB();
			championInfoPb.ConfigId = self.configId;
			championInfoPb.GridPositionX = self.gridPositionX;
			championInfoPb.GridPositionZ = self.gridPositionZ;
			championInfoPb.Type = self.gridType;
			return championInfoPb;
		}
	}
}
