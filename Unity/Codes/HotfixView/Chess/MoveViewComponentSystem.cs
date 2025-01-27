using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class MoveViewComponentAwakeSystem: AwakeSystem<MoveViewComponent>
	{
		public override void Awake(MoveViewComponent self)
		{
		}
	}

	[ObjectSystem]
	public class MoveViewComponentUpdateSystem: UpdateSystem<MoveViewComponent>
	{
		public override void Update(MoveViewComponent self)
		{
			Unit parent = self.GetParent<Unit>();

			parent.ViewPosition = Vector3.Lerp(parent.ViewPosition, parent.Position, 0.5f);
			parent.ViewRotation = Quaternion.Lerp(parent.ViewRotation, parent.Rotation, 0.5f);
		}
	}

	[FriendClass(typeof (MoveViewComponent))]
	public static partial class MoveViewComponentSystem
	{
		public static void Test(this MoveViewComponent self)
		{
		}
	}
}
