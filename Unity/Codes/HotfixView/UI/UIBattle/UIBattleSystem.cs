using System;
using System.Collections.Generic;
using ET.UIEventType;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIBattle))]
	public class UIBattleOnCreateSystem: OnCreateSystem<UIBattle>
	{
		public override void OnCreate(UIBattle self)
		{
			self.GoldText = self.AddUIComponent<UIText>("Gold/CostGo/cost");
			self.UITimerText = self.AddUIComponent<UITextmesh>("UITimer/Content/Label_Time");
			// self.allCoin = self.AddUIComponent<UICostIN>("Gold/CostGo");
			self.championLimitText = self.AddUIComponent<UIText>("championLimit/CostGo/cost");
			self.HpText = self.AddUIComponent<UIText>("Hp/Text");

			self.HpSlider = self.AddUIComponent<UISlider>("HpBar/Slider");

			self.refreshShopButton = self.AddUIComponent<UIButton>("Shop/left menu/refresh");
			self.refreshShopButton.SetOnClickAsync(self.OnRefreshShopBtnClick);

			self.returnBtn = self.AddUIComponent<UIButton>("ReturnBtn");
			self.returnBtn.SetOnClickAsync(self.OnReturnBtnClick);

			self.buyLvBtn = self.AddUIComponent<UIButton>("Shop/left menu/buylvl");
			self.buyLvBtn.SetOnClickAsync(self.OnBuyLvBtnClick);

			self.buyLvCostText = self.AddUIComponent<UIText>("Shop/left menu/buylvl/CoasGo/cost");

			self.uIPlayerLv = self.AddUIComponent<UIPlayerLV>("UIPlayerLV");

			self.timeGo = self.GetGameObject().transform.Find("UITimer").gameObject;

			const int count = 3;
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
			await ChessBattleHelper.SendExitChessMap(self.ZoneScene());
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
				for (int i = 0; i < championIds.Count; i++)
				{
					self.cContainers[i].GetGameObject().SetActive(true);
					self.cContainers[i].SetChampion(championIds[i]);
				}

				for (int i = championIds.Count; i < self.cContainers.Length; i++)
				{
					self.cContainers[i].GetGameObject().SetActive(false);
				}
			}
			else
			{
				Log.Info("SetShopChampionList :: championIds is null");
			}
		}

		public static void SetTimer(this UIBattle self, int timer)
		{
			if (timer <= 0)
			{
				// 倒计时结束，隐藏文本
				self.timeGo.SetActive(false);
				return;
			}

			// 确保文本显示
			// self.UITimerText.GetGameObject().SetActive(true);
			self.timeGo.SetActive(true);
			self.UITimerText.SetText(timer.ToString());
		}

		public static void SetChampionLimit(this UIBattle self, int currentCount, int maxLimit)
		{
			self.championLimitText.SetText($"{currentCount}/{maxLimit}");
		}

		public static void SetChampionLimit(this UIBattle self, int maxLimit)
		{
			int currentCount = ChessBattleViewComponent.Instance.ChampionsOnMapCount;
			self.SetChampionLimit(currentCount, maxLimit);
		}

		public static void SetHp(this UIBattle self, int hp)
		{
			self.HpText.SetText($"{hp.ToString()}");
			self.HpSlider.SetValue(hp);
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

		public static void SetLevel(this UIBattle self, int level)
		{
			self.uIPlayerLv.SetLevel(level);
		}

		public static async ETTask OnBuyLvBtnClick(this UIBattle self)
		{
			try
			{
				C2G_LevelUp c2GLevelUp = new C2G_LevelUp();
				G2C_LevelUp response = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2GLevelUp) as G2C_LevelUp;

				if (response.Error != ErrorCode.ERR_Success)
				{
					Game.EventSystem.PublishAsync(new ShowErrorToast()
					{
						Scene = self.ZoneScene(),
						ErrorCode = response.Error
					}).Coroutine();
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void SetBuyLvCost(this UIBattle self, int cost)
		{
			self.buyLvCostText.SetText(cost.ToString());
		}
	}
}
