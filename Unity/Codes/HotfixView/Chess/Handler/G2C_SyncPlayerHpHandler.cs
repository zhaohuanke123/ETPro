using System;

namespace ET
{
	public class G2C_SyncPlayerHpHandler: AMHandler<G2C_SyncPlayerHp>
	{
		protected override void Run(Session session, G2C_SyncPlayerHp message)
		{
			UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
			if (uiBattle == null)
			{
				return;
			}

			uiBattle.SetHp(message.Hp);
		}
	}
}
