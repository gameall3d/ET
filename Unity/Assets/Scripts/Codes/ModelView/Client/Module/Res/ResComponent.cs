
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class ResComponent: Entity, IAwake, IDestroy
    {
        public static ResComponent Instance { get; set; }
    }

    public static class ResComponentSystem
    {
        public static async ETTask<GameObject> InstantiateAsync(this ResComponent self, string location, Transform parent_transform = null,
        bool stay_world_space = false)
        {
            return await YooAssetWrapper.InstantiateAsync(location, parent_transform, stay_world_space);
        }
        
        public static GameObject InstantiateSync(this ResComponent self, string location, Transform parent_transform = null,
        bool stay_world_space = false)
        {
            return YooAssetWrapper.InstantiateSync(location, parent_transform, stay_world_space);
        }

        public static void ReleaseInstance(this ResComponent self, GameObject go)
        {
            YooAssetWrapper.ReleaseInstance(go);
        }
        
        public static async ETTask<T> LoadAssetAsync<T>(this ResComponent self, string location)
                where T : UnityEngine.Object
        {
            return await YooAssetWrapper.LoadAssetAsync<T>(location);
        }

        public static async ETTask<UnityEngine.Object> LoadAssetAsync(this ResComponent self, string location, System.Type type)
        {
            return await YooAssetWrapper.LoadAssetAsync(location, type);
        }
        
        public static T LoadAssetSync<T>(this ResComponent self, string location)
                where T : UnityEngine.Object
        {
            return YooAssetWrapper.LoadAssetSync<T>(location);
        }

        public static async ETTask LoadSceneAsync(this ResComponent self, string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            await YooAssetWrapper.LoadSceneAsync(location, sceneMode, activateOnLoad);
        }

        public static void Release(this ResComponent self, UnityEngine.Object obj)
        {
            YooAssetWrapper.Release(obj);
        }

        public static Dictionary<string, UnityEngine.Object> GetAssetsByTagSync(this ResComponent self, string tag)
        {
            return YooAssetWrapper.GetAssetsByTagSync(tag);
        }

        public static void ReleaseByTag(this ResComponent self, string tag)
        {
            YooAssetWrapper.ReleaseByTag(tag);
        }
    }
}