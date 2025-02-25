using System;

namespace ET
{
	[Serializable]
	public class Execise
	{
		public int money;
		public int echo;

		public Execise(string json)
		{
		}

		public override string ToString()
		{
			return $"{money.ToString()} : {echo.ToString()}";
		}
	}
}
