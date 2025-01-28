namespace ET
{
	public class SessionComponentAwakeSystem: AwakeSystem<SessionComponent>
	{
		public override void Awake(SessionComponent self)
		{
			SessionComponent.Instance = self;
			// self.GateSession?.Dispose();
		}
	}

	public class SessionComponentDestroySystem: DestroySystem<SessionComponent>
	{
		public override void Destroy(SessionComponent self)
		{
			self.Session.Dispose();
			// self.GateSession?.Dispose();
		}
	}
}
