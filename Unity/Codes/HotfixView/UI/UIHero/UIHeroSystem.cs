using ET.UIEventType;
using UnityEngine;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UIHeroSystem))]
	public class UIHeroOnCreateSystem: OnCreateSystem<UIHero>
	{
		public override void OnCreate(UIHero self)
		{
			self.GetAllHero().Coroutine();
			self.CurIndex = 0;
			self.CloseButton = self.AddUIComponent<UIButton>("Panel/CloseBtn");
			self.CloseButton.SetOnClickAsync(self.CloseSelf);
			self.Awator = self.AddUIComponent<UIImage>("Panel/HeroAwator/HUD_PortraitContents01/SPR_Example_HeroPortrait");
			self.descText = self.AddUIComponent<UITextmesh>("Panel/DescText");
			self.nameText = self.AddUIComponent<UITextmesh>("Panel/HeroAwator/HitPoints/Label_HitPoints");

			self.BuyButton = self.AddUIComponent<UIButton>("Panel/BuyBtn");
			self.BuyButton.SetOnClickAsync(self.OnBuyBtnClick);

			self.PreButton = self.AddUIComponent<UIButton>("Panel/PreBtn");
			self.PreButton.SetOnClickAsync(self.OnPreBtnClick);
			self.NextButton = self.AddUIComponent<UIButton>("Panel/NextBtn");
			self.NextButton.SetOnClickAsync(self.OnNextBtnClick);
			self.pointNeedText = self.AddUIComponent<UITextmesh>("Panel/PointNeed");
			self.frameImage = self.AddUIComponent<UIImage>("Panel/HeroAwator/Frame");
		}
	}

	[FriendClass(typeof (UIHero))]
	public static class UIHeroSystem
	{
		public static void ShowCur(this UIHero self)
		{
			HeroInfo heroInfo = self.AllHeroes[self.CurIndex];
			HeroConfig config = heroInfo.Config;
			self.pointNeedText.SetText(config.ItemCount.ToString());
			self.BuyButton.GetGameObject().SetActive(!heroInfo.IsOwned);
			self.pointNeedText.GetGameObject().SetActive(!heroInfo.IsOwned);
			self.Awator.SetColor(heroInfo.IsOwned? Color.white : Color.black);
			self.Awator.SetSpritePath(config.Icon).Coroutine();
			self.nameText.SetText(config.Name);
			self.descText.SetText(config.Description);
			string framePath = $"UIGames/UI/Lv{config.Lv.ToString()}.png";
			self.frameImage.SetSpritePath(framePath).Coroutine();

			if (self.CurIndex == 0)
			{
				self.PreButton.GetGameObject().SetActive(false);
				self.NextButton.GetGameObject().SetActive(true);
			}
			else if (self.CurIndex == self.AllHeroes.Count - 1)
			{
				self.NextButton.GetGameObject().SetActive(false);
				self.PreButton.GetGameObject().SetActive(true);
			}
			else
			{
				self.PreButton.GetGameObject().SetActive(true);
				self.NextButton.GetGameObject().SetActive(true);
			}
		}

		public static async ETTask OnNextBtnClick(this UIHero self)
		{
			if (self.CurIndex >= self.AllHeroes.Count - 1)
			{
				return;
			}

			self.CurIndex++;
			self.ShowCur();
			await ETTask.CompletedTask;
		}

		public static async ETTask OnPreBtnClick(this UIHero self)
		{
			if (self.CurIndex <= 0)
			{
				return;
			}

			self.CurIndex--;
			if (self.CurIndex == 0)
			{
				self.PreButton.GetGameObject().SetActive(false);
			}

			self.ShowCur();
			await ETTask.CompletedTask;
		}

		public static async ETTask GetAllHero(this UIHero self)
		{
			// 获取所有英雄配置
			self.AllHeroes.Clear();
			foreach (var heroConfig in HeroConfigCategory.Instance.GetAll())
			{
				self.AllHeroes.Add(new HeroInfo
				{
					ConfigId = heroConfig.Value.Id,
					IsOwned = false
				});
			}

			// 获取已拥有的英雄
			G2C_GetHeroList g2CGetHeroes = await self.ZoneScene().GetComponent<SessionComponent>().Session
					.Call(new C2G_GetHeroList()) as G2C_GetHeroList;

			// 更新拥有状态
			foreach (var heroInfo in self.AllHeroes)
			{
				heroInfo.IsOwned = g2CGetHeroes.HeroIds.Contains(heroInfo.ConfigId);
			}
			self.ShowCur();
		}

		public static async ETTask OnBuyBtnClick(this UIHero self)
		{
			HeroInfo heroInfo = self.AllHeroes[self.CurIndex];
			if (heroInfo.IsOwned)
			{
				return;
			}

			G2C_BuyHero g2CBuyHero = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(new C2G_BuyHero
			{
				HeroConfigId = heroInfo.ConfigId
			}) as G2C_BuyHero;

			if (g2CBuyHero.Error == ErrorCode.ERR_Success)
			{
				heroInfo.IsOwned = true;
				self.ShowCur();
			}
			else
			{
				// 显示错误提示
				await EventSystem.Instance.PublishAsync(new ShowToast
				{
					Text = "购买失败"
				});
			}
		}
	}
}
