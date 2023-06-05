using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

namespace ET
{
    public static class YooAssetEditor 
    {
        public static void BuildAssetForce(BuildTarget buildTarget)
        {
            Debug.Log($"开始构建 : {buildTarget}");

            // 构建参数
            string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
            BuildParameters buildParameters = new BuildParameters();
            buildParameters.OutputRoot = defaultOutputRoot;
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline;
            buildParameters.BuildMode = EBuildMode.ForceRebuild;
            buildParameters.PackageName = "DefaultPackage";
            buildParameters.PackageVersion = "1.0";
            buildParameters.VerifyBuildingResult = true;
            buildParameters.ShareAssetPackRule = new DefaultShareAssetPackRule();
            buildParameters.CompressOption = ECompressOption.LZ4;
            buildParameters.OutputNameStyle = EOutputNameStyle.HashName;
            buildParameters.CopyBuildinFileOption = ECopyBuildinFileOption.ClearAndCopyAll;
    
            // 执行构建
            AssetBundleBuilder builder = new AssetBundleBuilder();
            var buildResult = builder.Run(buildParameters);
            if (buildResult.Success)
            {
                Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
            }
            else
            {
                Debug.LogError($"构建失败 : {buildResult.ErrorInfo}");
            }
        }
    }
}
