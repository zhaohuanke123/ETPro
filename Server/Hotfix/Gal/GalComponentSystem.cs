using System;

namespace ET
{
	[ObjectSystem]
	public class GalComponentAwakeSystem: AwakeSystem<GalComponent>
	{
		public override void Awake(GalComponent self)
		{
		}
	}

	[ObjectSystem]
	public class GalComponentDestroySystem: DestroySystem<GalComponent>
	{
		public override void Destroy(GalComponent self)
		{
		}
	}

	[FriendClass(typeof (GalComponent))]
	public static partial class GalComponentSystem
	{
		public static int GetNextGalId(this GalComponent self)
		{
			return self.nextGalId;
		}

		public static int PassGal(this GalComponent self)
		{
			int galId = self.nextGalId + 1;
			Log.Warning($"PassGal enter");
			try
			{
				GalConfig galConfig = GalConfigCategory.Instance.Get(galId);
				// Player player = self.GetParent<Player>();
				// BagComponent bagComponent = player.GetComponent<BagComponent>();
				// bagComponent.AddItem(ItemDefine.PointId, galConfig.Count);
				// player.SendMessage(new G2C_UpdateItem()
				// {
				// 	ItemId = ItemDefine.PointId,
				// 	ItemCount = bagComponent.GetItemCount(ItemDefine.PointId)
				// });
				Log.Warning($"PassGal Add Point");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				galId = self.nextGalId;
			}
			finally
			{
				self.nextGalId = galId;
				DBComponent dbComponent = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
				dbComponent.Save(self).Coroutine();
			}

			return galId;
		}
	}
}
