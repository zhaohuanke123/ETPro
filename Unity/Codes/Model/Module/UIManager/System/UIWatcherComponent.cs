using System;
using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof(Scene))]
	public class UIWatcherComponent:Entity,IAwake,ILoad
    {
		public static UIWatcherComponent Instance;
		
		public TypeSystems typeSystems;
		
		public Dictionary<Type, bool> I18NCheckRes = new Dictionary<Type, bool>();

	}
}
