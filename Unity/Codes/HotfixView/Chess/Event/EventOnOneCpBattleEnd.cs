namespace ET
{
	public class EventOnOneCpBattleEnd: AEvent<EventType.OneCpBattleEnd>
	{
		protected override void Run(EventType.OneCpBattleEnd args)
		{
			ChessBattleViewComponent.Instance.ShowAllInMap();
		}
	}
}
