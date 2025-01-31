using UnityEngine;
using UnityEditor;
using System.IO;

namespace ET
{
    public class CopyAssetPath
    {
        private const string ASSETS_PACKAGE_PATH = "Assets/AssetsPackage/";

        [MenuItem("Assets/复制资源路径")]
        private static void CopyPath()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            
            // 检查是否在AssetsPackage目录下
            if (!assetPath.StartsWith(ASSETS_PACKAGE_PATH))
            {
                Debug.LogWarning("只支持复制AssetsPackage目录下的资源路径!");
                return;
            }

            // 移除前缀路径
            string relativePath = assetPath.Substring(ASSETS_PACKAGE_PATH.Length);
            
            // 复制到剪贴板
            GUIUtility.systemCopyBuffer = relativePath;
            Debug.Log($"已复制路径: {relativePath}");
        }

        // 控制菜单项是否可用
        [MenuItem("Assets/复制资源路径", true)]
        private static bool ValidateCopyPath()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            return assetPath.StartsWith(ASSETS_PACKAGE_PATH);
        }
    }
}