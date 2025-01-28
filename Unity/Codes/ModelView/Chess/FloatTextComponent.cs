using TMPro;
using UnityEngine;

namespace ET
{
	[ChildOf]
	public class FloatTextComponent: Entity, IAwake<GameObject> , IDestroy
	{
		public TextMeshProUGUI tmp;
		public Transform transform;
		public CanvasGroup canvasGroup;
		public Vector3 moveDirection;
		public float timer;
		public float fadeOutTime = 1f;
		public float speed = 3;
		public long MoveTimer;
		public long UpdateTime;
	}
}
