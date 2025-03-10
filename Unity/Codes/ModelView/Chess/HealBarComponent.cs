﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[ChildOf()]
	public class HealBarComponent: Entity, IAwake<Transform>, IDestroy
	{
		public CanvasGroup canvasGroup;
		public Image hpFill;
		public TMP_Text Lv;
		public TMP_Text Name;
	}
}
