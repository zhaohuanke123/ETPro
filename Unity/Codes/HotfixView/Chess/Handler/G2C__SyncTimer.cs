using System;

namespace ET
{
    [MessageHandler]
    public class G2C__SyncTimerHandler: AMHandler<G2C_SyncTimer>
    {
        protected override void Run(Session session, G2C_SyncTimer message)
        {
            UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
            if (uiBattle == null)
            {
                return;
            }

            uiBattle.SetTimer((int)(message.timer / 1000));
        }
    }
}