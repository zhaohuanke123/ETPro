using System;
using UnityEngine;

public class AnimatorRootMotion : MonoBehaviour
{
	bool mIsInit;
	Animator mAnimator;

	public Action<Vector3, Quaternion> onRootMotion;

	private void Init()
	{
		if (mIsInit)
		{
			return;
		}
		mIsInit = true;
		mAnimator = GetComponent<Animator>();
	}

	void OnAnimatorMove()
	{
		if (!mIsInit)
		{
			Init();
		}

		onRootMotion?.Invoke(mAnimator.deltaPosition, mAnimator.deltaRotation);
	}
}
