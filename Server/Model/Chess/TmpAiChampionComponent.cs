using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof (GamePlayComponent))]
	public class TmpAiChampionComponentComponent: Entity, IAwake<Player>, IDestroy
	{
		public List<ChampionInfo> aiCp;
		public Player aiPlayer;
		public Player player;
	}
}
