﻿using UnityEngine;

namespace ET
{
	[UISystem]
	[FriendClass(typeof (UITransform))]
	public class UITransformOnCreateSystem: OnCreateSystem<UITransform, Transform>
	{
		public override void OnCreate(UITransform self, Transform transform)
		{
			self.transform = transform;
		}
	}

	[FriendClass(typeof (UITransform))]
	[FriendClass(typeof (UIManagerComponent))]
	public static class UITransformSystem
	{
		public static Transform ActivatingComponent(this UITransform self)
		{
			if (self.transform == null)
			{
				var pui = self.Parent;
				self.transform = self.GetParentTransform()?.Find(UIManagerComponent.Instance.pathMap[pui.Id]);
				if (self.transform == null)
				{
					Log.Error(self.Parent.GetType().Name + "路径错误:" + UIManagerComponent.Instance.pathMap[pui.Id]);
				}
			}

			return self.transform;
		}

		public static Transform GetParentTransform(this UITransform self)
		{
			if (self.parentTransform == null)
			{
				var pui = self.Parent.Parent;
				var uitrans = pui.GetUIComponent<UITransform>("");
				if (uitrans == null)
				{
					Log.Error("ParentTransform is null Path:" + UIManagerComponent.Instance.pathMap[self.Id]);
				}
				else
				{
					uitrans.ActivatingComponent();
					self.parentTransform = uitrans.transform;
				}
			}

			return self.parentTransform;
		}

		public static Transform GetTransform(this Entity self)
		{
			UITransform uitrans;
			if (self.GetType() != TypeInfo<UITransform>.Type)
				uitrans = self.GetUIComponent<UITransform>("");
			else
				uitrans = self as UITransform;
			uitrans.ActivatingComponent();
			return uitrans.transform;
		}

		public static GameObject GetGameObject(this Entity self)
		{
			UITransform uitrans;
			if (self.GetType() != TypeInfo<UITransform>.Type)
				uitrans = self.GetUIComponent<UITransform>("");
			else
				uitrans = self as UITransform;
			uitrans.ActivatingComponent();
			return uitrans.transform?.gameObject;
		}

		public static T GetMonoComponent<T>(this Entity self, string path = "") where T : Component
		{
			GameObject gameObject = self.GetGameObject();
			Transform transform = gameObject.transform.Find(path);
			if (transform == null)
			{
				Log.Error("GetComponent is null Path:" + path);
				return null;
			}

			return transform.GetComponent<T>();
		}
	}
}
