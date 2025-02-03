using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationData
{
	public string Name;
	public string ClipPath;
	public float ClipTime;
	public float FrameCnt;
	public bool IsLoop;
	public bool IsApplyRootMotion;
}

public class PlayableData
{
	Dictionary<string, AnimationData> mAnimationDataDict = new Dictionary<string, AnimationData>();
	private AnimationCollector mAnimationCollector;

	public void SetAnimationCollector(AnimationCollector animationCollector)
	{
		mAnimationCollector = animationCollector;
	}

	public void Add(string name, string AnimationClipPath, AnimationClip clip, bool isLoop,
	bool isApplyRootMotion = false)
	{
		AnimationData animationData = new AnimationData();
		animationData.Name = name;
		animationData.ClipPath = AnimationClipPath;
		if (clip == null)
		{
			Debug.LogError($"PlayableData.Add: {name} clip is null");
			return;
		}

		animationData.ClipTime = clip.length;
		animationData.FrameCnt = Mathf.CeilToInt(animationData.ClipTime / 24);
		animationData.IsLoop = isLoop;
		animationData.IsApplyRootMotion = isApplyRootMotion;

		Add(animationData);
	}

	public void Add(AnimationData animationData)
	{
		string name = animationData.Name;
		if (mAnimationDataDict.ContainsKey(name))
		{
			Debug.LogError($"PlayableData.Add: {name} already exists");
			return;
		}

		mAnimationDataDict[name] = animationData;
	}

	public AnimationData GetData(string name)
	{
		mAnimationDataDict.TryGetValue(name, out var data);
		return data;
	}

	public AnimationClip GetClip(string path)
	{
		mAnimationCollector.Dict.TryGetValue(path, out var clip);
		return clip;
	}
}
