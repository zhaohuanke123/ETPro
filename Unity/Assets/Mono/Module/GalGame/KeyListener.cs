using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class KeyListener : MonoBehaviour
    {
        public event Action<KeyCode> OnKey;
        public event Action<KeyCode> OnKeyDown;
        public event Action<KeyCode> OnKeyUp;
        HashSet<KeyCode> codes = new HashSet<KeyCode>();
        // Update is called once per frame
        void Update()
        {
            if (Input.anyKey)
            {
                foreach (KeyCode keyCode in Enum.GetValues(TypeInfo<KeyCode>.Type))
                {
                    if (Input.GetKey(keyCode))
                    {
                        OnKey?.Invoke(keyCode);
                        break;
                    }
                }
            }
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(TypeInfo<KeyCode>.Type))
                {
                    if (Input.GetKey(keyCode))
                    {
                        codes.Add(keyCode);
                        OnKeyDown?.Invoke(keyCode);
                        break;
                    }
                }
            }
            foreach (KeyCode keyCode in codes)
            {
                if (Input.GetKeyUp(keyCode))
                {
                    OnKeyUp?.Invoke(keyCode);
                    break;
                }
            }

        }
    }
}
