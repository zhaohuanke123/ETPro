using ET.EventType;

namespace ET
{
    public class ChangePosition_RefreshChampionView: AEventClass<EventType.ChangePosition>
    {
        protected override void Run(object changePosition)
        {
            if (changePosition is ChangePosition posInfo)
            {
                Unit unit = posInfo.Unit;
            }
        }
    }
}