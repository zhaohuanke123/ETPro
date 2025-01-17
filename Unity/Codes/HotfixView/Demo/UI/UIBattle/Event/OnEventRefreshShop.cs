namespace ET
{
    public class OnEventRefreshShop: AEvent<UIEventType.RefreshShop>
    {
        protected override void Run(UIEventType.RefreshShop args)
        {
            UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
            uiBattle.SetShopChampionList(args.championIds);
        }
    }
}