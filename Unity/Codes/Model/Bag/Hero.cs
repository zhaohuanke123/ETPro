namespace ET
{
	public class Hero: Entity, IAwake, IDestroy
	{
		public int configId;

		public HeroConfig Config => HeroConfigCategory.Instance.Get(configId);
	}
}
