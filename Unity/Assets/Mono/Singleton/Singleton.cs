using UnityEngine.UI;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
	public static T Instance { get; private set; } = new T();

	protected abstract void InitSingleton();

	public virtual void Create()
	{

	}
}
