using System;
using UnityEngine;

namespace ET.Test
{
	public class Test1 : MonoBehaviour
	{
		private void Update()
		{
			Debug.Log(nameof(Test1) + ":" + nameof(Update) + " called" + " FrameCount" + Time.frameCount.ToString());	
		}
	}
}
