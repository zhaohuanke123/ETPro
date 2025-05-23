﻿using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UIManagerComponent : Entity,IAwake,IDestroy,ILoad,IInput
    {
        public static UIManagerComponent Instance;
        public Dictionary<string, UIWindow> windows;//所有存活的窗体  {ui_name:window}
        public Dictionary<UILayerNames, LinkedList<string>> windowStack;//窗口记录队列
        public int MaxOderPerWindow = 10;
        public float ScreenSizeflag { get; set; }
        public float WidthPadding;
        public Dictionary<long, MultiDictionary<string,Type, Entity>> componentsMap = new Dictionary<long, MultiDictionary<string,Type, Entity>>();
        public Dictionary<long, int> lengthMap = new Dictionary<long, int>();
        public Dictionary<long, string> pathMap = new Dictionary<long, string>();
    }
}
