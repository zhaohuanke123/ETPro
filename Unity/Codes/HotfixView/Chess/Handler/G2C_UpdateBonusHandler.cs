using System;

namespace ET
{
    public class G2C_UpdateBonusHandler: AMHandler<G2C_UpdateBonus>
    {
        protected override void Run(Session session, G2C_UpdateBonus message)
        {
            UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
            if (uiBattle != null)
            {
                uiBattle.SetBonusList(message.TypeIdList, message.CountList);
            }
        }
    }
}