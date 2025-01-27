using System.Collections.Generic;

namespace ET
{
	[ObjectSystem]
	public class UnitComponentAwakeSystem: AwakeSystem<UnitComponent>
	{
		public override void Awake(UnitComponent self)
		{
		}
	}

	[ObjectSystem]
	public class UnitComponentDestroySystem: DestroySystem<UnitComponent>
	{
		public override void Destroy(UnitComponent self)
		{
		}
	}

	public static class UnitComponentSystem
	{
		public static void Add(this UnitComponent self, Unit unit)
		{
		}

		public static Unit Get(this UnitComponent self, long id)
		{
			Unit unit = self.GetChild<Unit>(id);
			return unit;
		}

		public static void Remove(this UnitComponent self, long id)
		{
			Unit unit = self.GetChild<Unit>(id);
			unit?.Dispose();
		}

		public static void RemoveAll(this UnitComponent self)
		{
			List<Entity> units = new List<Entity>(self.Children.Count);
			foreach (Entity unit in self.Children.Values)
			{
				units.Add(unit);
			}
			units.ForEach(unit => unit.Dispose());
		}
	}
}
