﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// <para>GameObject缓存池</para>
    /// <para>注意：</para>
    /// <para>1、所有需要预设都从这里加载，不要直接到ResourcesManager去加载，由这里统一做缓存管理</para>
    /// <para>2、缓存分为两部分：从资源层加载的原始GameObject(Asset)，从GameObject实例化出来的多个Inst</para>
    /// <para>原则: 有借有还，再借不难，借出去的东西回收时，请不要污染(pc上会进行检测，发现则报错)</para>
    /// <para>何为污染？不要往里面添加什么节点，借出来的是啥样子，返回来的就是啥样子</para>
    /// <para>GameObject内存管理，采用lru cache来管理prefab</para>
    /// <para>为了对prefab和其产生的go的内存进行管理，所以严格管理go生命周期</para>
    /// <para>1、创建 -> GetGameObjectAsync</para>
    /// <para>2、回收 -> 绝大部分的时候应该采用回收(回收go不能被污染)，对象的销毁对象池会自动管理 RecycleGameObject</para>
    /// <para>3、销毁 -> 如果的确需要销毁，或一些用不到的数据想要销毁，也必须从这GameObjectPool中进行销毁，
    ///         严禁直接调用GameObject.Destroy方法来进行销毁，而应该采用GameObjectPool.DestroyGameObject方法</para>
    /// <para>不管是销毁还是回收，都不要污染go，保证干净</para>
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class GameObjectPoolComponent: Entity, IAwake, IDestroy
    {
        public Transform cacheTransRoot;
        public static GameObjectPoolComponent Instance { get; set; }
        public LruCache<string, GameObject> goPool;
        public Dictionary<string, int> goInstCountCache; //go: inst_count 用于记录go产生了多少个实例

        public Dictionary<string, int> goChildsCountPool; //path: child_count 用于在editor模式下检测回收的go是否被污染 path:num

        public Dictionary<string, List<GameObject>> instCache; //path: inst_array
        public Dictionary<GameObject, string> instPathCache; // inst : prefab_path 用于销毁和回收时反向找到inst对应的prefab TODO:这里有优化空间path太占内存
        public Dictionary<string, bool> persistentPathCache; //需要持久化的资源
        public Dictionary<string, Dictionary<string, int>> detailGoChildsCount; //记录go子控件具体数量信息
    }
}