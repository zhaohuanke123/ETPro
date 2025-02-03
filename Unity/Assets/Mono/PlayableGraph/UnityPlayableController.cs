using System;
using System.Collections.Generic;
// using Common.Input;
using UnityEngine;

public class UnityPlayableController: MonoBehaviour
{
	public AnimationCollector animationCollector;
	public List<AniClipLoadData> aniaClipLoadDatas;
	public PlayableController playableController;

	public Animator animator;

	public float fadeTime = 0.5f;
	public float offsetTime;
}
