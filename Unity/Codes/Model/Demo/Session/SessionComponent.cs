namespace ET
{
	[ComponentOf(typeof(Scene))]
	public class SessionComponent: Entity, IAwake, IDestroy
	{
		public static SessionComponent Instance;
		public Session Session { get; set; }
		// public Session GateSession { get; set; }
	}
}
