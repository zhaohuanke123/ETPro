
#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace ET
{
    [InitializeOnLoad]
    public class AutoBuildHelper
    {
        private static FileSystemWatcher watcher;

        private static bool isFocused;

        public static bool hasChange;
        static AutoBuildHelper()
        {
            hasChange = false;
            RefreshState();
            EditorApplication.update += Update;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode://停止播放事件监听后被监听
                    OnEditorFocus(true);
                    break;
                
            }
        }

        //定义事件处理程序
        private static void OnChanged(object sender,FileSystemEventArgs e)
        {
            hasChange = true;
        }
        private static void Update()
        {
            if (isFocused == UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                return;
            }
            isFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
            OnEditorFocus(isFocused);
        }
        /// <summary>
        /// 当重新聚焦
        /// </summary>
        /// <param name="focus"></param>
        private static void OnEditorFocus(bool focus)
        {
            if (focus&&hasChange)
            {
                bool autoBuild = EditorPrefs.HasKey("AutoBuild");
                if (!autoBuild)
                    return;
                BuildAssemblieEditor.BuildCodeAuto();
            }
        }


        public static void ChangeAutoBuild(bool autoBuild)
        {
            if (autoBuild)
            {
                EditorPrefs.SetInt("AutoBuild",1);
            }
            else
            {
                EditorPrefs.DeleteKey("AutoBuild");
            }
            
            RefreshState();
        }


        private static void RefreshState()
        {
            bool autoBuild = EditorPrefs.HasKey("AutoBuild");
            if (autoBuild && watcher == null)
            {
                //创建一个新的FileSystemWatcher并设置其属性
                watcher = new FileSystemWatcher();
                watcher.IncludeSubdirectories = true;
                watcher.Path = new DirectoryInfo("Codes").FullName;
                /*监视LastAcceSS和LastWrite时间的更改以及文件或目录的重命名*/
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                        NotifyFilters.FileName | NotifyFilters.DirectoryName;
                //只监视cs文件
                watcher.Filter = "*.cs";
                //添加事件句柄
                //当由FileSystemWatcher所指定的路径中的文件或目录的
                //大小、系统属性、最后写时间、最后访问时间或安全权限
                //发生更改时，更改事件就会发生
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                //由FileSystemWatcher所指定的路径中文件或目录被创建时，创建事件就会发生
                watcher.Created += new FileSystemEventHandler(OnChanged);
                //当由FileSystemWatcher所指定的路径中文件或目录被删除时，删除事件就会发生
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                //当由FileSystemWatcher所指定的路径中文件或目录被重命名时，重命名事件就会发生
                watcher.Renamed += new RenamedEventHandler(OnChanged);
                //开始监视
                watcher.EnableRaisingEvents = true;
            }
            else if (!autoBuild && watcher!=null)
            {
                watcher.Dispose();
                watcher = null;
            }
        }
    }
}

#endif
