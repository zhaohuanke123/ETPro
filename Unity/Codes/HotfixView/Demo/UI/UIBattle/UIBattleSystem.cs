using System.Collections.Generic;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIBattle))]
	public class UIBattleOnCreateSystem: OnCreateSystem<UIBattle>
	{
		public override void OnCreate(UIBattle self)
		{
			self.GoldText = self.AddUIComponent<UIText>("Gold/CostGo/cost");
			self.CountDownText = self.AddUIComponent<UIText>("Placement/Timer/Text");
			// self.allCoin = self.AddUIComponent<UICostIN>("Gold/CostGo");
			self.championLimitText = self.AddUIComponent<UIText>("championLimit/Text");
			self.HpText = self.AddUIComponent<UIText>("Hp/Text");

			self.refreshShopButton = self.AddUIComponent<UIButton>("Shop/left menu/refresh");
			self.refreshShopButton.SetOnClickAsync(self.OnRefreshShopBtnClick);

			self.returnBtn = self.AddUIComponent<UIButton>("ReturnBtn");
			self.returnBtn.SetOnClickAsync(self.OnReturnBtnClick);

			const int count = 5;
			self.cContainers = new UIChampionContainer[count];
			for (int i = 0; i < self.cContainers.Length; i++)
			{
				self.cContainers[i] = self.AddUIComponent<UIChampionContainer, int>($"Shop/Layout/CC{i}", i);
			}

			const int bonusCount = 6;
			self.bonusList = new UIBonus[bonusCount];
			for (int i = 0; i < self.bonusList.Length; i++)
			{
				self.bonusList[i] = self.AddUIComponent<UIBonus, int>($"BonusContainer/UIBonus_{i}", i);
			}
		}
	}

	[ObjectSystem]
	[FriendClass(typeof (UIBattle))]
	public class UIBattleLoadSystem: LoadSystem<UIBattle>
	{
		public override void Load(UIBattle self)
		{
		}
	}

	[FriendClass(typeof (UIBattle))]
	public static class UIBattleSystem
	{
		public static async ETTask OnReturnBtnClick(this UIBattle self)
		{
			ChessBattleHelper.SendExitChessMap(self.ZoneScene()).Coroutine();
			await UIManagerComponent.Instance.DestroyWindow<UIBattle>();
		}

		public static void SetGold(this UIBattle self, int gold)
		{
			self.GoldText.SetText(gold.ToString());
		}

		public static void SetShopChampionList(this UIBattle self, List<int> championIds)
		{
			if (championIds != null)
			{
				for (int i = 0; i < self.cContainers.Length; i++)
				{
					self.cContainers[i].SetChampion(championIds[i]);
				}
			}
			else
			{
				Log.Info("SetShopChampionList :: championIds is null");
			}
		}

		public static void SetTimer(this UIBattle self, int timer)
		{
			self.CountDownText.SetText(timer.ToString());
		}

		public static void SetChampionLimit(this UIBattle self, int championLimit)
		{
			self.championLimitText.SetText(championLimit.ToString());
		}

		public static void SetHp(this UIBattle self, int hp)
		{
			self.HpText.SetText(hp.ToString());
		}

		public static async ETTask OnRefreshShopBtnClick(this UIBattle self)
		{
			await ChessBattleHelper.RefreshShopChampionList(self.ZoneScene());
		}

		public static void SetBonusList(this UIBattle self, List<int> messageTypeIdList, List<int> messageCountList)
		{
			for (int i = 0; i < self.bonusList.Length; i++)
			{
				if (i < messageTypeIdList.Count)
				{
					self.bonusList[i].SetBonus(messageTypeIdList[i], messageCountList[i]);
				}
				else
				{
					self.bonusList[i].SetBonus(-1, 0);
				}
			}
		}
	}
}
