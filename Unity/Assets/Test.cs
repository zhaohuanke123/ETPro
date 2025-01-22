using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class Test: MonoBehaviour
    {
        void Update()
        {
            Matrix4x4 transformLocalToWorldMatrix = this.transform.localToWorldMatrix;
            Debug.Log(transformLocalToWorldMatrix);
        }
    }
}