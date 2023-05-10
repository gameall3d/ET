
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 游戏对象池管理器, 参考了Motion Framework
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class GameObjectPoolComponent: Entity, IAwake, IUpdate, IDestroy
    {
        [StaticField]
        public static GameObjectPoolComponent Instance;

        public Dictionary<string, GameObjectPool> _poolDict = new Dictionary<string, GameObjectPool>(100);
        public readonly List<GameObjectPool> _removeList = new List<GameObjectPool>(100);

        // 缓存池根节点
        public Transform _root;
        
        /// <summary>
        /// 默认的初始容器值
        /// </summary>
        public int _defaultInitCapacity = 0;
        
        /// <summary>
        /// 默认的最大容器值
        /// </summary>
        public int _defaultMaxCapacity = int.MaxValue;
        
        /// <summary>
        /// 默认的静默销毁时间
        /// 注意：小于零代表不主动销毁
        /// </summary>
        public float _defaultDestroyTime = -1f;
    }
}