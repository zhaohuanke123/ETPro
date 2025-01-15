using System.IO;
using System.Text;
using MonKey;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class PathTool
    {
        [MenuItem("Assets/Export Folder Contents Paths")]
        [Command("ExportFolderContentsPaths", "Export Folder Contents Paths", DefaultValidation = DefaultValidation.AT_LEAST_ONE_ASSET)]
        private static void ExportPaths()
        {
            // 获取选中的文件夹
            Object[] selectedObjects = Selection.GetFiltered(typeof (Object), SelectionMode.Assets);

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("请选择一个文件夹。");
                return;
            }

            StringBuilder pathsBuilder = new StringBuilder();

            foreach (Object selectedObject in selectedObjects)
            {
                string folderPath = AssetDatabase.GetAssetPath(selectedObject);

                if (Directory.Exists(folderPath))
                {
                    // 获取文件夹中的第一层文件
                    string[] filePaths = Directory.GetFiles(folderPath);
                    string[] dirPaths = Directory.GetDirectories(folderPath);

                    // 处理文件路径
                    foreach (string filePath in filePaths)
                    {
                        if (!filePath.EndsWith(".meta"))
                        {
                            string relativePath = filePath.Replace("\\", "/");
                            relativePath = relativePath.Replace("Assets/AssetsPackage/", "");
                            pathsBuilder.AppendLine(relativePath);
                        }
                    }

                    // 处理文件夹路径
                    foreach (string dirPath in dirPaths)
                    {
                        string relativePath = dirPath.Replace("\\", "/");
                        relativePath = relativePath.Replace("Assets/AssetsPackage/", "");
                        pathsBuilder.AppendLine(relativePath);
                    }
                }
                else
                {
                    Debug.LogWarning($"选中的对象不是文件夹: {folderPath}");
                }
            }

            // 将路径复制到剪贴板
            if (pathsBuilder.Length > 0)
            {
                GUIUtility.systemCopyBuffer = pathsBuilder.ToString();
                Debug.Log("路径已复制到剪贴板。");
            }
            else
            {
                Debug.LogWarning("没有找到任何文件或文件夹。");
            }
        }
    }
}