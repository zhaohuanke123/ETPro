using System.Collections.Generic;

namespace ET
{
	[UIComponent]
	[FriendClass(typeof (UIHero))]
	public class UIHero: Entity, IAwake, IDestroy, IOnEnable, IOnCreate
	{
		public static string PrefabPath = "UI/UIHero/Prefabs/UIHero.prefab";

		// public UILoopListView2 heroLoopView;
		public UIButton CloseButton;
		public UIButton BuyButton;
		public UIButton PreButton;
		public UIButton NextButton;
		public UITextmesh pointNeedText;
		public UIImage Awator;
		public UITextmesh descText;
		public UITextmesh nameText;

		public UIImage frameImage;

		public int CurIndex;

		public List<HeroInfo> AllHeroes = new List<HeroInfo>();
	}

	public class HeroInfo
	{
		public int ConfigId;

		public HeroConfig Config => HeroConfigCategory.Instance.Get(ConfigId);

		public bool IsOwned;
	}
}
