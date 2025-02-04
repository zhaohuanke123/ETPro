using System;

namespace ET
{
    [FriendClassAttribute(typeof(ET.GamePlayComponent))]
    public class G2C_UnitDeadHandler : AMHandler<G2C_UnitDead>
    {
        protected override async void Run(Session session, G2C_UnitDead message)
        {
            await GamePlayComponent.Instance.isViewReadyTask;
            
            long unitId = message.UnitId;
            Scene currentScene = session.ZoneScene().CurrentScene();
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();

            Unit unit = unitComponent.Get(unitId);
            CharacterControlComponent characterControlComponent = unit.GetComponent<CharacterControlComponent>();
            characterControlComponent.PlayAnim(AnimDefine.Dead, 0.5f);
            await TimerComponent.Instance.WaitAsync((long)(characterControlComponent.GetAnimTime(AnimDefine.Dead) * 1000));

            unitComponent.Remove(unitId);
        }
    }
}
