using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class MaterialComponent : Entity,IAwake
    {
        public static MaterialComponent Instance { get; set; }
        public Dictionary<string, Material> CacheMaterial;
    }
}
