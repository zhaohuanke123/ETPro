using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationCollectorData
{
	public string path;
	public AnimationClip clip;
}

[CreateAssetMenu(fileName = "AnimationCollector", menuName = "AnimationCollector")]
public class AnimationCollector : ScriptableObject
{
	public List<AnimationCollectorData> datas = new List<AnimationCollectorData>();

	Dictionary<string, AnimationClip> mDict;

	public Dictionary<string, AnimationClip> Dict
	{
		get
		{
			if (mDict == null)
			{
				mDict = new Dictionary<string, AnimationClip>();
				foreach (var data in datas)
				{
					if (data.clip != null)
					{
						mDict[data.path] = data.clip;
					}
				}
			}
			return mDict;
		}
	}

	public void Sort()
	{
		datas.Sort((a, b) => string.Compare(a.path, b.path, StringComparison.Ordinal));
	}
}
