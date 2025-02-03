using System.Collections.Generic;

namespace ET
{
	[ObjectSystem]
	public class BagComponentAwakeSystem: AwakeSystem<BagComponent>
	{
		public override void Awake(BagComponent self)
		{
			if (self.items == null)
			{
				self.items = new Dictionary<int, int>();
			}
		}
	}

	[ObjectSystem]
	public class BagComponentDestroySystem: DestroySystem<BagComponent>
	{
		public override void Destroy(BagComponent self)
		{

		}
	}

	[FriendClass(typeof (BagComponent))]
	public static class BagComponentSystem
	{
#if NOT_UNITY
		public static void AddItem(this BagComponent self, int id, int count)
		{
			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(id);

			if (!self.items.TryAdd(id, count))
			{
				self.items[id] += count;
			}

			DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(self).Coroutine();
		}

		public static int GetItemCount(this BagComponent self, int id)
		{
			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(id);

			return self.items.GetValueOrDefault(id, 0);
		}

		public static void GetItems(this BagComponent self, List<ItemInfo> items)
		{
			foreach (var item in self.items)
			{
				items.Add(new ItemInfo()
				{
					ItemId = item.Key,
					Count = item.Value
				});
			}
		}
		
		public static async ETTask SetItemCount (this BagComponent self, int id, int count)
		{
			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(id);

			if (count <= 0)
			{
				self.items.Remove(id);
			}
			else
			{
				self.items[id] = count;
			}

			await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(self);
		}
#endif
	}
}
