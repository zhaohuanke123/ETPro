namespace ET
{
	[ComponentOf(typeof(Scene))]
	public sealed class SelectWatcherComponent:Entity,IAwake,ILoad
	{
		public static SelectWatcherComponent Instance;
		public TypeSystems typeSystems;
	}
}