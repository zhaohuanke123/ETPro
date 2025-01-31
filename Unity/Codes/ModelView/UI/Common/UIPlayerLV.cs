using UnityEngine;

namespace ET
{
	[UIComponent]
	public class UIPlayerLV: Entity, IAwake, IDestroy, IOnCreate, IOnEnable
	{
		public static readonly int LevelUp = Animator.StringToHash("LevelUp");
		public Animator animator;
		public UITextmesh lvText;
	}
}
