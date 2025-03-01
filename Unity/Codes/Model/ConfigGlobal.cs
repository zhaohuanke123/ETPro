using System.Collections.Generic;

namespace ET
{
	public static class ConfigGlobal
	{
		public static int AddPowerBeHit = 20;
		public static int AddPower = 50;
		public static int MaxPower = 100;
		public static int MaxBonusCount = 6;
		public static int ShopCanBuyCount = 3;

		public static List<float> lvScaleList = new List<float>()
		{
			0f,
			1f,
			1.5f,
			2f
		};
	}
}
