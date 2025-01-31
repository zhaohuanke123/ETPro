using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[UIComponent]
	public class UIBattle: Entity, IAwake, ILoad, IOnCreate, IOnEnable
	{
		public static string PrefabPath => "UI/UIBattle/Prefabs/UIBattle.prefab";
		public UIText GoldText;

		public UITextmesh UITimerText;

		// public UICostIN allCoin;
		public UIText HpText;
		public UISlider HpSlider;
		public UIButton returnBtn;
		public UIButton buyLvBtn;
		public UIText buyLvCostText;
		public UIText championLimitText;
		public UIPlayerLV uIPlayerLv;
		public UIChampionContainer[] cContainers;

		public UIBonus[] bonusList;
		public UIButton refreshShopButton;

		public GameObject timeGo;
		// public UIText levelText; // 等级显示
	}
}
