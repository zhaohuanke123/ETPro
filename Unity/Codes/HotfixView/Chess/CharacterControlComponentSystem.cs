using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class CharacterControlComponentAwakeSystem: AwakeSystem<CharacterControlComponent, GameObject>
	{
		public override void Awake(CharacterControlComponent self, GameObject gameObject)
		{
			self.transform = gameObject.transform;
			self.rotateTransform = gameObject.transform.Find("Model");
			self.AddComponent<HealBarComponent, Transform>(gameObject.transform.Find("HealthBar"));
		}
	}

	[ObjectSystem]
	public class CharacterControlComponentDestroySystem: DestroySystem<CharacterControlComponent>
	{
		public override void Destroy(CharacterControlComponent self)
		{

		}
	}

	[FriendClass(typeof (CharacterControlComponent))]
	public static class CharacterControlComponentSystem
	{
		public static void SetPosition(this CharacterControlComponent self, Vector3 position)
		{
			self.transform.position = position;
		}

		public static void SetRotation(this CharacterControlComponent self, Quaternion rotation)
		{
			self.rotateTransform.rotation = rotation;
		}
	}
}
