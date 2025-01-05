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
    }
}