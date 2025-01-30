using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	public static class ItemDefine
	{
		public const int PointId = 1001;
	}

	public class Item: Entity, IAwake, IDestroy
	{
		public int configId;

		[BsonIgnore]
		public ItemConfig Config => ItemConfigCategory.Instance.Get(configId);
	}
}
