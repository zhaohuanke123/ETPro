using System;

namespace ET
{
    [FriendClassAttribute(typeof (ET.CharacterControlComponent))]
    public class G2C_SyncPowerHandler: AMHandler<G2C_SyncPower>
    {
        protected override void Run(Session session, G2C_SyncPower message)
        {
            Scene currentScene = session.ZoneScene().CurrentScene();
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            long id = message.ToId;
            Unit unit = unitComponent.Get(id);

            if (unit == null || unit.IsDisposed)
            {
                return;
            }

            CharacterControlComponent characterControlComponent = unit.GetComponent<CharacterControlComponent>();
            characterControlComponent.pwBar.SetRatio(1.0f * message.Power / 100, true);
        }
    }
}