using System.Collections.Generic;

namespace ET
{
	public class TmpAiChampionComponentComponentAwakeSystem: AwakeSystem<TmpAiChampionComponentComponent, Player>
	{
		public override void Awake(TmpAiChampionComponentComponent self, Player player)
		{
			self.aiCp = new List<ChampionInfo>();
			self.aiPlayer = PlayerComponent.Instance.AddChild<Player, string>("AI");
			self.aiPlayer.SetCamp(Camp.Player1);
			PlayerComponent.Instance.Add(self.aiPlayer);
			self.player = player;

			GamePlayComponent gamePlayComponent = self.GetParent<GamePlayComponent>();
			gamePlayComponent.AddPlayer(self.aiPlayer, true);
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

			(int x, int z) = championMapArrayComponent.GetEmptyPos(self.aiPlayer);
			if (x == -1)
			{
				return;
			}

			int randomChampionId = shopComponent.GetRandomChampionId();
			ChampionInfo championInfo = championMapArrayComponent.AddNewToGrid(self.aiPlayer, x, z);
			championInfo.SetConfigId(randomChampionId);
			self.aiCp.Add(championInfo);
			Log.Info($"AI生成了一个{championInfo} 位置 {x} | {z}");
		}
	}
}
