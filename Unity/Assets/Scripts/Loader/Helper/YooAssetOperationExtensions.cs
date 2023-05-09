using UnityEngine;
using YooAsset;

namespace ET
{
    public static class YooAssetOperationExtensions
    {
        public static ETTask<OperationHandleBase> ToETTask(this OperationHandleBase handle)
        {
            var task = ETTask<OperationHandleBase>.Create();
            switch (handle)
            {
                case AssetOperationHandle assetOperationHandle:
                    assetOperationHandle.Completed += op => { task.SetResult(op); };
                    break;
                case SceneOperationHandle sceneOperationHandle:
                    sceneOperationHandle.Completed += op => { task.SetResult(op); };
                    break;
                case SubAssetsOperationHandle subAssetsOperationHandle:
                    subAssetsOperationHandle.Completed += op => { task.SetResult(op); };
                    break;
            }

            return task;
        }

        public static ETTask<AsyncOperationBase> ToETTask(this AsyncOperationBase handle)
        {
            var task = ETTask<AsyncOperationBase>.Create();
            handle.Completed += op => { task.SetResult(op); };
            return task;
        }

        public static void Release(this OperationHandleBase handle)
        {
            switch (handle)
            {
                case AssetOperationHandle assetOperationHandle:
                    assetOperationHandle.Release();
                    break;
                case SceneOperationHandle sceneOperationHandle:
                    Debug.LogWarning("Can't release SceneOperationHandle");
                    break;
                case SubAssetsOperationHandle subAssetsOperationHandle:
                    subAssetsOperationHandle.Release();
                    break;
            }
        }
    }
}