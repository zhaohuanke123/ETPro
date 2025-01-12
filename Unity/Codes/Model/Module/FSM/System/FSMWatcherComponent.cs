using System;

namespace ET
{
	[ComponentOf(typeof(Scene))]
	public sealed class FSMWatcherComponent:Entity,IAwake,ILoad
	{
		public static FSMWatcherComponent Instance;

		public TypeSystems typeSystems;
		
	}
}
