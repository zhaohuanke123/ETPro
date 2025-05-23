﻿using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class KeyCodeComponent:Entity,IAwake,IDestroy
    {
        public static KeyCodeComponent Instance;
        
        public Dictionary<int,int> KeyMap { get; set; }//[逻辑按键：物理按键编号]

        /// <summary>
        /// 默认键位
        /// </summary>
        public readonly Dictionary<int, int> DefaultKeyCodeMap = new Dictionary<int, int>()
        {
            { KeyCodeType.Skill1, 49 },//KeyCode.Alpha1
            { KeyCodeType.Skill2, 50 },
            { KeyCodeType.Skill3, 51 },
            { KeyCodeType.Skill4, 52 },
            { KeyCodeType.Skill5, 53 },
            { KeyCodeType.Skill6, 54 },
            { KeyCodeType.Skill7, 55 },
        };
    }
}
