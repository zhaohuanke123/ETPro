using System.Collections.Generic;
using ET.Chess;

namespace ET
{
	public class TmpAiChampionComponentComponentAwakeSystem: AwakeSystem<TmpAiChampionComponentComponent, Player>
	{
		public override void Awake(TmpAiChampionComponentComponent self, Player player)
		{
			self.aiCp = new List<ChampionInfo>();
			self.aiPlayer = PlayerComponent.Instance.AddChild<Player, string>("AI");
			self.aiPlayer.SetCamp(Camp.Player2);
			PlayerComponent.Instance.Add(self.aiPlayer);
			self.player = player;

			GamePlayComponent gamePlayComponent = self.GetParent<GamePlayComponent>();
			gamePlayComponent.AddPlayer(self.aiPlayer);
		}
	}

	public class TmpAiChampionComponentComponentDestroySystem: DestroySystem<TmpAiChampionComponentComponent>
	{
		public override void Destroy(TmpAiChampionComponentComponent self)
		{
			self.aiPlayer.Dispose();
		}
	}

	[FriendClass(typeof (TmpAiChampionComponentComponent))]
	public static class TmpAiChampionComponentComponentSystem
	{
		public static void GenAnAI(this TmpAiChampionComponentComponent self)
		{
			GamePlayComponent gamePlayComponent = self.GetParent<GamePlayComponent>();
			ChampionMapArrayComponent championMapArrayComponent = gamePlayComponent.GetComponent<ChampionMapArrayComponent>();
			ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();

			ChampionInfo championInfo = new ChampionInfo();
			(int x, int z) = championMapArrayComponent.GetEmptyPos(self.aiPlayer);
			if (x == -1)
			{
				return;
			}

			int randomChampionId = shopComponent.GetRandomChampionId();
			championInfo.SetConfigId(randomChampionId);
			championMapArrayComponent.AddToGrid(self.player, championInfo, x, z);
			self.aiCp.Add(championInfo);
			Log.Info($"AI生成了一个{championInfo}");
		}
	}
}
