namespace ET
{
	[ComponentOf(typeof (GamePlayComponent))]
	public class SendUniPosComponent: Entity, IAwake, IDestroy, IFixedUpdate
	{
		public bool isSendArrived = false;
	}
}
