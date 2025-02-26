namespace ET
{
	[ComponentOf()]
	public class AccountInfoComponent: Entity, IAwake, IDestroy
	{
		public bool isOjUser;
		public static AccountInfoComponent Instance;
		public string Token { get; set; }
		public long AccountId { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
	}
}
