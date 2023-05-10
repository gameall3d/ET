using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GameObjectPool
    {
        private readonly Queue<PoolObject> _cache;
        private readonly Transform _root;
        
        private float _lastRestoreRealTime = -1f;
        
        /// <summary>
        /// 资源定位地址
        /// </summary>
        public string Location { private set; get; }
        
        /// <summary>
        /// 对象池的初始容量
        /// </summary>
        public int InitCapacity { private set; get; }

        /// <summary>
        /// 对象池的最大容量
        /// </summary>
        public int MaxCapacity { private set; get; }

        /// <summary>
        /// 静默销毁时间
        /// </summary>
        public float DestroyTime { private set; get; }
        
        /// <summary>
        /// 内部缓存总数
        /// </summary>
        public int CacheCount
        {
            get { return _cache.Count; }
        }

        public string PoolName
        {
            get
            {
                return this.Location;
            }
        }

        /// <summary>
        /// 外部使用总数
        /// </summary>
        public int SpawnCount { private set; get; } = 0;

        public GameObjectPool(Transform root, string location, int initCapacity, int maxCapacity, float destroyTime)
        {
            _root = new GameObject(location + "_Pool").transform;
            _root.SetParent(root, false);
            Location = location;
            InitCapacity = initCapacity;
            MaxCapacity = maxCapacity;
            DestroyTime = destroyTime;
            
            // 创建缓存池
            _cache = new Queue<PoolObject>(initCapacity);
        }

        /// <summary>
        /// 查询静默时间内是否可以销毁
        /// </summary>
        public bool CanAutoDestroy()
        {
            if (this.DestroyTime < 0) return false;

            if (this._lastRestoreRealTime > 0 && this.SpawnCount <= 0)
            {
                return (Time.realtimeSinceStartup - this._lastRestoreRealTime) > this.DestroyTime;
            }
            
            return false;
        }

        /// <summary>
        /// 同步产生一个游戏对象
        /// </summary>
        /// <returns></returns>
        public GameObject SpawnSync()
        {
            PoolObject poolObject;
            if (this._cache.Count > 0)
            {
                poolObject = this._cache.Dequeue();
                SetSpawnCloneObject(poolObject.gameObject);
            }
            else
            {
                var go = ResComponent.Instance.InstantiateSync(this.Location);
                poolObject = go.AddComponent<PoolObject>();
                poolObject.PoolName = this.PoolName;
                SetSpawnCloneObject(go);
            }

            poolObject.IsPooled = false;
            this.SpawnCount++;
            return poolObject.gameObject;
        }

        /// <summary>
        /// 回收游戏对象
        /// </summary>
        /// <param name="poolObject"></param>
        public void Restore(PoolObject poolObject)
        {
            if (poolObject.PoolName != this.PoolName)
            {
                Log.Error($"Trying to add object with PoolName[{poolObject.PoolName}] to incorrect pool[{this.PoolName}]");
                return;
            }
            
            this.SpawnCount--;
            if (this.SpawnCount <= 0)
            {
                this._lastRestoreRealTime = Time.realtimeSinceStartup;
            }

            GameObject go = poolObject.gameObject;
            if (poolObject.IsPooled)
            {
                if (Define.IsEditor)
                {
                    Log.Warning($"{go.name} is already in pool. Why are you trying to return it again? Check usage.");
                }

                return;
            }
            
            poolObject.IsPooled = true;
            if (go != null)
            {
                SetRestoreCloneObject(go);
                if (this._cache.Count < this.MaxCapacity)
                {
                    this._cache.Enqueue(poolObject);
                }
                else
                {
                    // 大于上限，直接销毁
                    ResComponent.Instance.ReleaseInstance(go);
                }
            }
        }
        
        private void SetSpawnCloneObject(GameObject cloneObj)
        {
            cloneObj.SetActive(true);
            cloneObj.transform.SetParent(null);
            cloneObj.transform.localPosition = Vector3.zero;
        }
        private void SetRestoreCloneObject(GameObject cloneObj)
        {
            cloneObj.SetActive(false);
            cloneObj.transform.SetParent(_root);
            cloneObj.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        public void Destroy()
        {
            foreach (PoolObject poolObject in this._cache)
            {
                var go = poolObject.gameObject;
                if (go != null)
                {
                    ResComponent.Instance.ReleaseInstance(go);
                }
            }
            this._cache.Clear();
            this.SpawnCount = 0;
            GameObject.Destroy(this._root.gameObject);
        }
    }
}