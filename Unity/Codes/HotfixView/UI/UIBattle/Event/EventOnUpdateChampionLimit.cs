namespace ET
{
    [Event]
    public class EventOnUpdateChampionLimit: AEvent<UIEventType.UpdateChampionLimit>
    {
        protected override void Run(UIEventType.UpdateChampionLimit args)
        {
            UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
            if (uiBattle != null)
            {
                uiBattle.SetChampionLimit(args.CurrentCount, args.MaxLimit);
            }
        }
    }
} 