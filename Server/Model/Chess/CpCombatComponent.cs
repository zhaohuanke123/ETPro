using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof (Unit))]
	public class CpCombatComponent: Entity, IAwake, IDestroy, IFixedUpdate
	{
		public List<Unit> targetList = new List<Unit>();
	}
}
