namespace ET
{
	public class AccountInfoComponentDAwakeSystem: AwakeSystem<AccountInfoComponent>
	{
		public override void Awake(AccountInfoComponent self)
		{
			AccountInfoComponent.Instance = self;
		}
	}

	public class AccountInfoComponentDestroySystem: DestroySystem<AccountInfoComponent>
	{
		public override void Destroy(AccountInfoComponent self)
		{
			self.Token = null;
			self.AccountId = 0;
		}
	}

	public static class AccountInfoComponentSystem
	{
	}
}
