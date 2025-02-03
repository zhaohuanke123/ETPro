using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayableControllerMgr : Singleton<PlayableControllerMgr>
{
	protected override void InitSingleton()
	{

	}

	public void LoadPlayableData(List<AniClipLoadData> aniaClipLoadDatas, AnimationCollector collector,
		Action<PlayableData> cb)
	{
		PlayableData playableData = new PlayableData();
		playableData.SetAnimationCollector(collector);

		for (int i = 0; i < aniaClipLoadDatas.Count; i++)
		{
			AniClipLoadData aniClipLoadData = aniaClipLoadDatas[i];
			string path = aniClipLoadData.Path;
			AnimationClip clip = playableData.GetClip(path);
			if (clip == null)
			{
				Debug.LogError($"PlayableControllerMgr.LoadPlayableData: {path} clip is null");
				return;
			}
			playableData.Add(aniClipLoadData.Name, path, clip, aniClipLoadData.IsApplyRootMotion);
		}

		cb?.Invoke(playableData);
	}

	private string GetPathEdiotr(string name)
	{
		return string.Format("Asset/{0}", name);
	}
}
