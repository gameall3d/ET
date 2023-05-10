using System.Collections.Generic;
using HybridCLR;
using UnityEngine;
using YooAsset;

namespace ET
{
    public static class HybridCLRHelper
    {
        public static void Load()
        {
            // Dictionary<string, UnityEngine.Object> dictionary = AssetsBundleHelper.LoadBundle("aotdlls.unity3d");
            // foreach (var kv in dictionary)
            // {
            //     byte[] bytes = (kv.Value as TextAsset).bytes;
            //     RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.Consistent);
            // }

            Dictionary<string, UnityEngine.Object> dictionary = YooAssetWrapper.GetAssetsByTagSync("AotDlls");
            foreach (var kv in dictionary)
            {
                byte[] bytes = (kv.Value as TextAsset).bytes;
                RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.Consistent);
            }
            YooAssetWrapper.ReleaseByTag("AotDlls");
        }
    }
}