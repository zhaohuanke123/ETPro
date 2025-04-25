using System;
using UnityEngine;

namespace ET.Test
{
	public class TestLauncher : MonoBehaviour
	{
		private void Update()
		{
			Debug.Log(nameof(TestLauncher) + ":" + nameof(Update) + " called" + " FrameCount" + Time.frameCount.ToString());

			if (Input.GetKeyDown(KeyCode.Space))
			{
				gameObject.AddComponent<Test1>();
			}
		}
	}
}
