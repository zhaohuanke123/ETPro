using UnityEngine;

namespace ET
{
	[ChildOf(typeof(ToastComponent))]
    public class UIToast : Entity,IAwake,IOnCreate,IOnEnable<string>, IOnDisable
	{
		public static readonly int Active = Animator.StringToHash("Active");
		public UITextmesh Text;
		public UITransform Content;
		public Animator animator;
	}
}
