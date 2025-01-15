using UnityEditor;
using UnityEngine;

namespace ET
{
	public static class SomeTool
	{
		[MenuItem("Tools/MYTool/打开PersistentDataPath")]
		public static void OpenPersistentDataPath()
		{
			System.Diagnostics.Process.Start(Application.persistentDataPath);
		}

		[MenuItem("Tools/MYTool/删除PlayerPrefs数据")]
		public static void DeletePlayerPrefsData()
		{
			PlayerPrefs.DeleteAll();
			Debug.Log("PlayerPrefs数据已删除");
		}

		// [MenuItem("Tools/MYTool/ReloadCode %F12")]
		// public static void ReloadCode()
		// {
		// 	CodeLoader.Instance.LoadLogic();
		// 	Game.EventSystem.Add(CodeLoader.Instance.GetHotfixTypes());
		// 	Game.EventSystem.Load();
		// 	Debug.Log("hot reload success!");
		// }
	}
}
