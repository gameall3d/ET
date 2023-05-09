using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ET;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityObject = UnityEngine.Object;

namespace YooAsset
{
    /// <summary>
    /// 内置文件查询服务类
    /// </summary>
    class GameQueryServices : IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
        }
    }
    public static class YooAssetWrapper
    {
        private static readonly Dictionary<UnityObject, OperationHandleBase> _Obj2Handle = new();

        private static readonly Dictionary<GameObject, UnityObject> _GO2Obj = new();

        private static readonly Dictionary<string, List<UnityObject>> _ObjsInTag = new();

        // 加载图集中的图片规则为：图集名[图片名],跟addressable中相同
        // 例如：BuildingAtlas[Build_1001]
        private static readonly Regex _SA_MATCH = new("[*.\\w/]+");

        private static ResourceDownloaderOperation _DOWNLOADER;
        public static Action<int, int, long, long> OnDownloadProgress;

        public static ETTask<AsyncOperationBase> InitializeAsync(EPlayMode playMode, string packageName = "DefaultPackage")
        {
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                package = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(package);
            }

            InitializeParameters parameters = null;

            switch (playMode)
            {
                case EPlayMode.EditorSimulateMode:
                    var editorarParam = new EditorSimulateModeParameters();
                    editorarParam.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                    parameters = editorarParam;
                    break;
                case EPlayMode.OfflinePlayMode:
                    parameters = new OfflinePlayModeParameters();
                    break;
                case EPlayMode.HostPlayMode:
                    parameters = new HostPlayModeParameters
                    {
                        QueryServices = new GameQueryServices(),
                        DefaultHostServer = GetHostServerURL(),
                        FallbackHostServer = GetHostServerURL()
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof (playMode), playMode, null);
            }

            return package.InitializeAsync(parameters).ToETTask();
        }

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        public static string GetHostServerURL()
        {
            string hostServerIP = "http://127.0.0.1";
            string gameVersion = "v1.0";
#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{gameVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
            else
                return $"{hostServerIP}/CDN/StandaloneWindows64/{gameVersion}";
#else
		    if (Application.platform == RuntimePlatform.Android)
			    return $"{hostServerIP}/CDN/Android/{gameVersion}";
		    else if (Application.platform == RuntimePlatform.IPhonePlayer)
			    return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
		    else if (Application.platform == RuntimePlatform.WebGLPlayer)
			    return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
		    else
			    return $"{hostServerIP}/CDN/StandaloneWindows64/{gameVersion}";
#endif
        }
        

        public static async ETTask<string> UpdateStaticVersion(int time_out = 60, string packageName = "DefaultPackage")
        {
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync(true, time_out);

            await operation.ToETTask();

            if(operation.Status != EOperationStatus.Succeed)
            {
                //更新失败
                Debug.LogError(operation.Error);
                return "-1";
            }

            //更新失败
            Debug.Log($"Updated package Version : {operation.PackageVersion}");
            return operation.PackageVersion;
        }

        public static async ETTask<bool> UpdateManifest(string packageVersion, bool autoSaveVersion = true, int time_out = 60, string packageName = "DefaultPackage")
        {
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion, autoSaveVersion, time_out);

            await operation.ToETTask();

            return operation.Status == EOperationStatus.Succeed;
        }

        public static long GetDownloadSize(int downloading_max_num = 10, int retry = 3, string packageName = "DefaultPackage")
        {
            var package = YooAssets.GetPackage(packageName);
            _DOWNLOADER = package.CreateResourceDownloader(downloading_max_num, retry);

            return _DOWNLOADER.TotalDownloadCount == 0 ? 0 : _DOWNLOADER.TotalDownloadBytes;
        }

        public static async ETTask<bool> Download(IProgress<float> progress = null)
        {
            if(_DOWNLOADER is null)
            {
                return false;
            }

            _DOWNLOADER.OnDownloadProgressCallback = (count, downloadCount, bytes, downloadBytes) =>
            {
                OnDownloadProgress?.Invoke(count, downloadCount, bytes, downloadBytes);
            };

            _DOWNLOADER.BeginDownload();

            await _DOWNLOADER.ToETTask();

            return _DOWNLOADER.Status == EOperationStatus.Succeed;
        }
        
        public static async ETTask<GameObject> InstantiateAsync(string           location,
                                                                 Transform        parent_transform = null,
                                                                 bool             stay_world_space = false,
                                                                 IProgress<float> progress         = null)
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>(location);

            await handle.ToETTask();

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsWrapper] Failed to load asset: {location}");
            }

            _Obj2Handle.TryAdd(handle.AssetObject, handle);

            if(UnityObject.Instantiate(handle.AssetObject, parent_transform, stay_world_space) is not GameObject go)
            {
                Release(handle.AssetObject);
                throw new Exception($"[YooAssetsWrapper] Failed to instantiate asset: {location}");
            }

            _GO2Obj.Add(go, handle.AssetObject);

            return go;
        }
        
        public static GameObject InstantiateSync(string  location,
                                                    Transform        parent_transform = null,
                                                    bool             stay_world_space = false)
        {
            var handle = YooAssets.LoadAssetSync<GameObject>(location);

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsWrapper] Failed to load asset: {location}");
            }

            _Obj2Handle.TryAdd(handle.AssetObject, handle);

            if(UnityObject.Instantiate(handle.AssetObject, parent_transform, stay_world_space) is not GameObject go)
            {
                Release(handle.AssetObject);
                throw new Exception($"[YooAssetsWrapper] Failed to instantiate asset: {location}");
            }

            _GO2Obj.Add(go, handle.AssetObject);

            return go;
        }

        public static bool CheckLocationValid(string location)
        {
            return YooAssets.CheckLocationValid(location);
        }

        public static async ETTask<T> LoadAssetAsync<T>(string location, IProgress<float> progress = null)
            where T : UnityObject
        {
            return await LoadAssetAsync(location, typeof (T)) as T;
        }

        public static async ETTask<UnityObject> LoadAssetAsync(string location, Type type)
        {
            if(type == typeof(Sprite))
            {
                var matches = _SA_MATCH.Matches(location);

                if(matches.Count == 2)
                {
                    var atlasLocation = matches[0].Value;
                    // fairygui 为了做alpha通道分离的文件，会尝试加载xx!a.png文件，在这边是加载不到的
                    if (!YooAssets.CheckLocationValid(atlasLocation))
                    {
                        return null;
                    }
                    
                    var sa_handle = YooAssets.LoadAssetAsync<SpriteAtlas>(matches[0].Value);

                    await sa_handle.ToETTask();

                    if(!sa_handle.IsValid)
                    {
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite atlas: {matches[0].Value}");
                    }

                    if(sa_handle.AssetObject is not SpriteAtlas sa)
                    {
                        sa_handle.Release();
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite atlas: {matches[0].Value}");
                    }

                    var sprite = sa.GetSprite(matches[1].Value);

                    if(sprite is null)
                    {
                        sa_handle.Release();
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite: {location}");
                    }

                    _Obj2Handle.TryAdd(sprite, sa_handle);

                    return sprite;
                }
            }
            
            if (!YooAssets.CheckLocationValid(location))
            {
                return null;
            }

            var handle = YooAssets.LoadAssetAsync(location, type);

            await handle.ToETTask();

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsWrapper] Failed to load asset: {location}");
            }

            _Obj2Handle.TryAdd(handle.AssetObject, handle);

            return handle.AssetObject;
        }

        public static T LoadAssetSync<T>(string location) where T : UnityObject
        {
            return LoadAssetSync(location, typeof (T)) as T;
        }

        public static UnityObject LoadAssetSync(string location, Type type)
        {
            if(type == typeof(Sprite))
            {
                var matches = _SA_MATCH.Matches(location);

                if(matches.Count == 2)
                {
                    var atlasLocation = matches[0].Value;
                    if (!YooAssets.CheckLocationValid(atlasLocation))
                    {
                        return null;
                    }
                    
                    var sa_handle = YooAssets.LoadAssetSync<SpriteAtlas>(matches[0].Value);

                    if(!sa_handle.IsValid)
                    {
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite atlas: {matches[0].Value}");
                    }

                    if(sa_handle.AssetObject is not SpriteAtlas sa)
                    {
                        sa_handle.Release();
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite atlas: {matches[0].Value}");
                    }

                    var sprite = sa.GetSprite(matches[1].Value);

                    if(sprite is null)
                    {
                        sa_handle.Release();
                        throw new Exception($"[YooAssetsWrapper] Failed to load sprite: {location}");
                    }

                    _Obj2Handle.TryAdd(sprite, sa_handle);

                    return sprite;
                }
            }
            
            if (!YooAssets.CheckLocationValid(location))
            {
                return null;
            }


            var handle = YooAssets.LoadAssetSync(location, type);
            
            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsWrapper] Failed to load asset: {location}");
            }

            _Obj2Handle.TryAdd(handle.AssetObject, handle);

            return handle.AssetObject;
        }

        public static async ETTask LoadSceneAsync(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            SceneOperationHandle handle = YooAssets.LoadSceneAsync(location, sceneMode, activateOnLoad);
            
            await handle.ToETTask();

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsWrapper] Failed to load scene: {location}");
            }
        }
        
        public static ETTask<RawFileOperationHandle> GetRawFileAsync(string path)
        {
            ETTask<RawFileOperationHandle> result = ETTask<RawFileOperationHandle>.Create();
            RawFileOperationHandle rawFileOperation = YooAssets.LoadRawFileAsync(path);
            rawFileOperation.Completed += handle => { result.SetResult(rawFileOperation); };
            return result;
        }

        public static async ETTask<Dictionary<string, UnityObject>> GetAssetsByTagAsync(string tag)
        {
            if (!_ObjsInTag.TryGetValue(tag, out List<UnityObject> objs))
            {
                objs = new List<UnityObject>();
                
                AssetInfo[] assetInfos = YooAssets.GetAssetInfos(tag);
                for (int i = 0; i < assetInfos.Length; i++)
                {
                    var assetInfo = assetInfos[i];
                    Debug.Log(assetInfo.Address);
                    var obj = await LoadAssetAsync<UnityObject>(assetInfo.Address);
                    objs.Add(obj);
                }
                // foreach (var assetInfo in assetInfos)
                // {
                //     Debug.Log(assetInfo.Address);
                //     Debug.Log(assetInfo.AssetPath);
                //     
                //     var obj = await LoadAssetAsync<UnityObject>(assetInfo.Address);
                //     objs.Add(obj);
                // }
            }

            Dictionary<string, UnityObject> objectDict = new Dictionary<string, UnityObject>();
            foreach (UnityObject o in objs)
            {
                objectDict.Add(o.name, o);
            }

            return objectDict;
        }
        
        public static Dictionary<string, UnityObject> GetAssetsByTagSync(string tag)
        {
            if (!_ObjsInTag.TryGetValue(tag, out List<UnityObject> objs))
            {
                objs = new List<UnityObject>();
                
                AssetInfo[] assetInfos = YooAssets.GetAssetInfos(tag);
                for (int i = 0; i < assetInfos.Length; i++)
                {
                    var assetInfo = assetInfos[i];
                    var obj = LoadAssetSync<UnityObject>(assetInfo.Address);
                    objs.Add(obj);
                }
                // foreach (var assetInfo in assetInfos)
                // {
                //     Debug.Log(assetInfo.Address);
                //     Debug.Log(assetInfo.AssetPath);
                //     
                //     var obj = await LoadAssetAsync<UnityObject>(assetInfo.Address);
                //     objs.Add(obj);
                // }
            }

            Dictionary<string, UnityObject> objectDict = new Dictionary<string, UnityObject>();
            foreach (UnityObject o in objs)
            {
                objectDict.Add(o.name, o);
            }

            return objectDict;
        }

        public static void ReleaseByTag(string tag)
        {
            if (_ObjsInTag.TryGetValue(tag, out List<UnityObject> objs))
            {
                foreach (UnityObject o in objs)
                {
                   Release(o); 
                }

                _ObjsInTag.Remove(tag);
            }
        }

        public static void ReleaseInstance(GameObject go)
        {
            if(go is null)
            {
                return;
            }

            UnityObject.Destroy(go);

            _GO2Obj.Remove(go, out UnityObject obj);

            Release(obj);
        }

        public static void Release(UnityObject obj)
        {
            if(obj is null)
            {
                return;
            }

            _Obj2Handle.Remove(obj, out OperationHandleBase handle);

            handle?.Release();
        }
    }
}