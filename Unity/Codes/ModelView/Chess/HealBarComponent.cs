using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[ComponentOf(typeof (CharacterControlComponent))]
	public class HealBarComponent: Entity, IAwake<Transform>, IDestroy
	{
		public CanvasGroup canvasGroup;
		public Image hpFill;
	}
}
