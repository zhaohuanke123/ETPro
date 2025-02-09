using System;

namespace ET
{
	[MessageHandler]
	public class G2C_UpdateShppChampionHandler: AMHandler<G2C_UpdateShppChampion>
	{
		protected override void Run(Session session, G2C_UpdateShppChampion message)
		{
			UIBattle uiBattle = UIManagerComponent.Instance.GetWindow<UIBattle>();
			if (uiBattle == null)
			{
				return;
			}

			uiBattle.SetShopChampionList(message.championIds);
		}
	}
}
