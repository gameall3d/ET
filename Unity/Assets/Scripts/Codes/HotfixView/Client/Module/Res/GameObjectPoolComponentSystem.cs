using UnityEngine;

namespace ET
{
    public class GameObjectPoolComponentAwakeSystem: AwakeSystem<GameObjectPoolComponent>
    {
        protected override void Awake(GameObjectPoolComponent self)
        {
            self.Awake();
        }
    }
    
    public class GameObjectPoolComponentUpdateSystem: UpdateSystem<GameObjectPoolComponent>
    {
        protected override void Update(GameObjectPoolComponent self)
        {
            self.Update();
        }
    }
    
    public class GameObjectPoolComponentDestroySystem: DestroySystem<GameObjectPoolComponent>
    {
        protected override void Destroy(GameObjectPoolComponent self)
        {
            self.Destroy();
        }
    }
    
    [FriendOf(typeof(GameObjectPoolComponent))]
    public static class GameObjectPoolComponentSystem
    {
        public static void Awake(this GameObjectPoolComponent self)
        {
            GameObjectPoolComponent.Instance = self;

            var gloabl = GameObject.Find("Global");
            var poolRootGO = new GameObject("PoolRoot");
            poolRootGO.transform.position = Vector3.zero;
            poolRootGO.transform.rotation = Quaternion.identity;
            poolRootGO.transform.SetParent(gloabl.transform, false);
            self._root = poolRootGO.transform;
        }

        public static GameObject SpawnSync(this GameObjectPoolComponent self, string location)
        {
            if (!self._poolDict.TryGetValue(location, out GameObjectPool pool))
            {
                pool = self.CreatePool(location, self._defaultInitCapacity, self._defaultMaxCapacity, self._defaultDestroyTime);
            }

            return pool.SpawnSync();
        }

        private static GameObjectPool CreatePool(this GameObjectPoolComponent self, string location, int initCapacity, int maxCapacity, float destroyTime)
        {
            GameObjectPool pool = new GameObjectPool(self._root, location, initCapacity, maxCapacity, destroyTime);
            self._poolDict.Add(location, pool);

            return pool;
        }

        public static void Restore(this GameObjectPoolComponent self, GameObject go)
        {
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                if (Define.IsEditor)
                {
                    Log.Warning($"Specified object is not a pooled instance: {go.name}");
                }

                return;
            }

            if (self._poolDict.TryGetValue(po.PoolName, out GameObjectPool pool))
            {
                pool.Restore(po);
            }
            else
            {
                if (Define.IsEditor)
                {
                    Log.Warning($"No pool available with name: {po.PoolName}");
                }
            }
        }

        public static void Update(this GameObjectPoolComponent self)
        {
            self._removeList.Clear();
            foreach ((string key, GameObjectPool value) in self._poolDict)
            {
                if (value.CanAutoDestroy())
                {
                    self._removeList.Add(value);
                }
            }
            
            foreach (GameObjectPool pool in self._removeList)
            {
                self._poolDict.Remove(pool.Location);
                pool.Destroy();
            }
        }

        public static void Destroy(this GameObjectPoolComponent self)
        {
            foreach ((string key, GameObjectPool pool) in self._poolDict)
            {
                pool.Destroy();
            }
            self._poolDict.Clear();
        }
    }
}
