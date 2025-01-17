using System;

namespace ET
{
    public class G2C_RefreshGoldHandler: AMHandler<G2C_RefreshGold>
    {
        protected override void Run(Session session, G2C_RefreshGold message)
        {
            UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
            if (uiBattle == null)
            {
                return;
            }
            
            uiBattle.SetGold(message.GlodCount);
        }
    }
}