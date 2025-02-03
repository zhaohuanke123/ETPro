using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableController : IPlayPlayableController
{
	bool mIsInit;
	bool mIsGraphPlay;

	PlayableGraph mPlayableGraph;
	AnimationClipPlayable mClipPlayable;
	AnimationMixerPlayable mMixerPlayable;

	private PlayableData mPlayableData;

	int mCurMixerPort = -1;
	int mCurMixerInputCount;
	List<bool> mPortStateList;

	bool mIsStartFade;
	float mWeigetTimer;
	List<float> mCurWeightList;
	List<float> mOriginWeightList;
	float mFadeTime;

	float mAnimDuration;

	public bool IsPauseUpdate { get; set; }
	float mPauseDuration;
	double mPlaySpeed = 1;
	float mPlaySpeedScale = 1;

	public PlayableController()
	{
		mPortStateList = new List<bool>();
		mCurWeightList = new List<float>();
		mOriginWeightList = new List<float>();
	}

	public float GetAnimDuration()
	{
		return mAnimDuration;
	}

	public PlayableData GetPlayableData()
	{
		return mPlayableData;
	}

	public void Init(Animator animator, PlayableData playableData, float playSpeedScale = 1)
	{
		if (!animator)
		{
			// Log.Error("PlayableController.Init: animator is null");
			return;
		}

		if (playableData == null)
		{
			// PELog.Error("PlayableController.Init: playableData is null");
			return;
		}

		if (mIsInit)
		{
			return;
		}
		mIsInit = true;

		mPlayableData = playableData;

		mPlayableGraph = PlayableGraph.Create();
		mPlayableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

		var playableOutput = AnimationPlayableOutput.Create(mPlayableGraph, "Animation", animator);
		mMixerPlayable = AnimationMixerPlayable.Create(mPlayableGraph, 0);
		playableOutput.SetSourcePlayable(mMixerPlayable);

		mPlaySpeed = mMixerPlayable.GetSpeed();
		mPlaySpeedScale = playSpeedScale;
		if (Mathf.Approximately(mPlaySpeedScale, -1))
		{
			mMixerPlayable.SetSpeed(mPlaySpeed * mPlaySpeedScale);
		}
	}

	public void Play(string aniName, float fadeTime, float fixedTimeOffset = 0)
	{
		if (!mIsInit)
		{
			return;
		}

		AnimationData animationData = mPlayableData.GetData(aniName);
		if (animationData == null)
		{
			// PELog.Error($"PlayableController.Play: {aniName} animationData is null");
			return;
		}

		AnimationClip clip = mPlayableData.GetClip(animationData.ClipPath);
		if (clip == null)
		{
			// PELog.Error($"PlayableController.Play: {aniName} clip is null");
			return;
		}

		if (mCurMixerInputCount > 1000)
		{
			// 不要每帧播放
			// PELog.Warn();
		}

		UpdateOriginWeight();

		mClipPlayable = AnimationClipPlayable.Create(mPlayableGraph, clip);

		int curPort = -1;
		for (int i = 0; i < mPortStateList.Count; i++)
		{
			if (!mPortStateList[i])
			{
				curPort = i;
				mPortStateList[i] = true;
				// break;
			}
		}

		if (curPort == -1)
		{
			mCurMixerInputCount++;
			mPortStateList.Add(true);
			curPort = mCurMixerInputCount - 1;
		}

		mCurMixerPort = curPort;

		mMixerPlayable.SetInputCount(mCurMixerInputCount);
		mPlayableGraph.Connect(mClipPlayable, 0, mMixerPlayable, mCurMixerPort);
		mClipPlayable.SetTime(fixedTimeOffset);

		mWeigetTimer = 0;
		if (mCurMixerInputCount <= 1)
		{
			mFadeTime = 0;
			SetWeight(mCurWeightList, mCurMixerPort, 1);
		}
		else
		{
			if (fadeTime > 0)
			{
				mFadeTime = fadeTime;
				mIsStartFade = true;
				SetWeight(mCurWeightList, mCurMixerPort, 0);
			}
			else
			{
				mFadeTime = 0;
				DeleteOldAni();
				SetWeight(mCurWeightList, mCurMixerPort, 1);
			}
		}

		if (!mIsGraphPlay)
		{
			mPlayableGraph.Play();
			mIsGraphPlay = true;
		}

		mAnimDuration = 0;
		mPauseDuration = 0;
	}

	void DeleteOldAni()
	{
		for (int i = 0; i < mCurMixerInputCount; i++)
		{
			if (i != mCurMixerPort)
			{
				mPlayableGraph.Disconnect(mMixerPlayable, i);
			}
		}

		int deleteCount = mCurMixerInputCount - 1 - mCurMixerPort;
		if (deleteCount > 0)
		{
			int removeIndex = mCurWeightList.Count - deleteCount;
			mCurWeightList.RemoveRange(removeIndex, deleteCount);
			mOriginWeightList.RemoveRange(removeIndex, deleteCount);
		}

		mCurMixerInputCount = mCurMixerPort + 1;
		mMixerPlayable.SetInputCount(mCurMixerInputCount);

		if (mCurMixerInputCount > 1)
		{
			for (int i = 0; i < mCurMixerInputCount - 1; i++)
			{
				mPortStateList[i] = false;
			}
		}
	}

	public void Update(float deltaTime, bool isUseGameDeltaTime = true)
	{
		if (!mIsInit)
		{
			return;
		}

		mAnimDuration += deltaTime;

		if (!IsPauseUpdate)
		{
			if (mPauseDuration > 0)
			{
				deltaTime += mPauseDuration;
				mPauseDuration = 0;
			}

			MixerOldNewAnim(deltaTime);

			// if (isUseGameDeltaTime)
			// {
			// 	float speed = Game.DeltaTime > 0 ? deltaTime / Game.DeltaTime : 0;
			// 	SetSpeed(speed);
			// }
			// else
			// {
				float speed = deltaTime > 0 ? deltaTime / Time.deltaTime : 0;
				SetSpeed(speed);
			// }
		}
		else
		{
			mPauseDuration += deltaTime;
			SetSpeed(0);
		}
	}

	private void SetSpeed(double speed)
	{
		if (mPlaySpeed != speed)
		{
			mPlaySpeed = speed;
			mMixerPlayable.SetSpeed(mPlaySpeed * mPlaySpeedScale);
		}
	}

	void MixerOldNewAnim(float deltaTime)
	{
		if (!mIsStartFade)
		{
			return;
		}

		float sum = 0;
		float weight = mFadeTime > 0 ? mWeigetTimer / mFadeTime : 1;
		mWeigetTimer += deltaTime;

		float curWeight = Mathf.Clamp01(weight);

		float oldWeightScale = 1 - curWeight;
		for (int i = 0; i < mCurMixerInputCount; i++)
		{
			if (i != mCurMixerPort)
			{
				float oldWeight = GetWeight(mOriginWeightList, i);
				float curOldWeight = Mathf.Clamp01(oldWeight * oldWeightScale);
				SetWeight(mCurWeightList, i, curOldWeight);
				sum += curOldWeight;
			}
		}
		SetWeight(mCurWeightList, mCurMixerPort, curWeight);

		if (weight >= 1)
		{
			mIsStartFade = false;
			DeleteOldAni();
		}
	}

	public void OnDestroy()
	{
		if (!mIsInit)
		{
			return;
		}

		mPlayableGraph.Destroy();
		mIsInit = false;
		mIsGraphPlay = false;
		mCurMixerPort = -1;
		mCurMixerInputCount = 0;
		mWeigetTimer = 0;
		mPortStateList.Clear();
		mCurWeightList.Clear();
		mOriginWeightList.Clear();
	}

	public float GetCurNormalizedTime()
	{
		return (float)mMixerPlayable.GetTime();
	}

	void SetWeight(List<float> weightList, int port, float weight)
	{
		if (weightList.Count <= port)
		{
			int originCount = weightList.Count;
			int addCount = port - originCount + 1;
			for (int i = 0; i < addCount; i++)
			{
				if (i + originCount == port)
				{
					weightList.Add(weight);
				}
				else
				{
					weightList.Add(0);
					// PELog.Error("Jump SetWeight");
				}
			}
		}
		else
		{
			weightList[port] = weight;
		}

		mMixerPlayable.SetInputWeight(port, weight);
	}

	float GetWeight(List<float> wightList, int port)
	{
		if (wightList.Count <= port)
		{
			// PELog.Error("GetWeight is too big", port);
			return 0;
		}

		return wightList[port];
	}

	void UpdateOriginWeight()
	{
		for (int i = 0; i < mCurWeightList.Count; i++)
		{
			SetWeight(mOriginWeightList, i, mCurWeightList[i]);
		}
	}

	public float GetAnimTime(string aniName)
	{
		AnimationData animationData = mPlayableData.GetData(aniName);
		if (animationData == null)
		{
			// PELog.Error($"PlayableController.GetAnimTime: {aniName} animationData is null");
			return 0;
		}

		return animationData.ClipTime;
	}
}
