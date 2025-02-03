public interface IPlayPlayableController
{
	bool IsPauseUpdate { get; set; }

	void Play(string aniName, float fadeTime, float fixedTimeOffset = 0);
	void Update(float deltaTime, bool isUseGameDeltaTime = true);
	float GetCurNormalizedTime();
}
