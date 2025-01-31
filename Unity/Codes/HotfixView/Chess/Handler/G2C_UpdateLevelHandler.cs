using UnityEngine;

namespace ET
{
	[MessageHandler]
	public class G2C_UpdateLevelHandler: AMHandler<G2C_UpdateLevel>
	{
		protected override async void Run(Session session, G2C_UpdateLevel message)
		{
			// 更新游戏组件中的英雄限制
			GamePlayComponent.Instance.CurrentChampionLimit = message.ChampionLimit;

			// 获取UIBattle界面并更新显示
			UIBattle ui = UIManagerComponent.Instance.GetWindow<UIBattle>();
			if (ui != null)
			{
				// 更新英雄数量限制显示
				ui.SetChampionLimit(message.ChampionLimit);

				// 可以添加等级显示
				ui.SetLevel(message.Level);
				
				ui.SetBuyLvCost(message.NextLevelCost);
			}

			await ETTask.CompletedTask;
		}
	}
}
