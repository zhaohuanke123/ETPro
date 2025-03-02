using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
	public class GameObjectCreator
	{
		[MenuItem("Tools/生成角色预制体")]
		private static void CreateGameObjectStructure()
		{
			Debug.LogWarning(Selection.activeObject.GetType());
			// 获取当前选中的对象
			GameObject selectedObject = Selection.activeGameObject;

			if (selectedObject == null)
			{
				Debug.LogWarning("请先选择一个角色模型！");
				return;
			}

			GameObject topObject = new GameObject("Character");
			topObject.transform.position = Vector3.zero;

			GameObject middleObject = new GameObject("Model");
			middleObject.transform.position = Vector3.zero;
			middleObject.transform.SetParent(topObject.transform, false);
			GameObject model = Object.Instantiate(selectedObject, middleObject.transform, false);
			model.transform.localScale = Vector3.one * 2;

			// 挂载脚本
			ReferenceCollector referenceCollector = topObject.AddComponent<ReferenceCollector>();
			UnityPlayableController controller = topObject.AddComponent<UnityPlayableController>();
			controller.animator = model.GetComponent<Animator>();
			controller.aniaClipLoadDatas = new List<AniClipLoadData>(UnityPlayableControllerEditor.defaultClips);
			EditorUtility.SetDirty(controller);

			// 保存场景
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}

		[MenuItem("Assets/Collect Animation Clips")]
		public static void CollectAnimationClips()
		{
			// 当前选中的路径
			string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			folderPath = Path.GetDirectoryName(folderPath);
			// 获取选中的对象
			var selectedObjects = Selection.objects;

			List<AnimationClip> animationClips = selectedObjects.OfType<AnimationClip>().ToList();

			// 创建AnimationCollector资产
			AnimationCollector collector = ScriptableObject.CreateInstance<AnimationCollector>();
			collector.datas.Clear();

			int cnt = 0;
			List<string> paths = new List<string>()
			{
				"Idle",
				"Run",
				"Attack",
				"BeHit",
				"Dead"
			};
			foreach (AnimationClip clip in animationClips)
			{
				AnimationCollectorData data = new AnimationCollectorData
				{
					path = paths[cnt++],
					clip = clip
				};
				collector.datas.Add(data);
				if (cnt == 5)
				{
					cnt = 0;
				}
			}

			collector.Sort();

			// 保存AnimationCollector资产
			string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/AnimationCollector.asset");
			AssetDatabase.CreateAsset(collector, assetPath);
			AssetDatabase.SaveAssets();

			Debug.Log("AnimationCollector created successfully: " + assetPath);
		}
	}
}

[CustomEditor(typeof (UnityPlayableController))]
public class UnityPlayableControllerEditor: Editor
{
	public static readonly List<AniClipLoadData> defaultClips = new List<AniClipLoadData>
	{
		new AniClipLoadData
		{
			Name = "Idle",
			Path = "Idle",
			isLoop = true,
			IsApplyRootMotion = false
		},
		new AniClipLoadData
		{
			Name = "Run",
			Path = "Run",
			isLoop = true,
			IsApplyRootMotion = false
		},
		new AniClipLoadData
		{
			Name = "Attack",
			Path = "Attack",
			isLoop = false,
			IsApplyRootMotion = false
		},
		new AniClipLoadData
		{
			Name = "BeHit",
			Path = "BeHit",
			isLoop = false,
			IsApplyRootMotion = false
		},
		new AniClipLoadData
		{
			Name = "Dead",
			Path = "Dead",
			isLoop = false,
			IsApplyRootMotion = false
		},
		new AniClipLoadData()
		{
			Name = "SAttack",
			Path = "SAttack",
			isLoop = false,
			IsApplyRootMotion = false
		}
	};

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		UnityPlayableController controller = (UnityPlayableController)target;
		if (GUILayout.Button("生成默认动画配置"))
		{
			Undo.RecordObject(controller, "Generate Default Animation Config");
			controller.aniaClipLoadDatas = new List<AniClipLoadData>(defaultClips);
			EditorUtility.SetDirty(controller);
		}
	}
}
