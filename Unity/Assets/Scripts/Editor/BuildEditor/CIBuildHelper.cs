using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using YooAsset;

namespace ET
{
    public static class CIBuildHelper
    {
        static string InitScenePath = "Assets/Scenes/Init.unity";
        public static string BuildOutputPath = "/BuildOutputPath";

        [MenuItem("CIBuild/BuildIOS")]
        public static void BuildIOS()
        {
            var outDir = System.Environment.CurrentDirectory + BuildOutputPath + "/IOS";
            // var outputPath = Path.Combine(outDir, Application.productName + ".exe");
            BuildTarget target = BuildTarget.iOS;
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, target);

            string[] scenes = new[] { InitScenePath };
            Debug.Log("Begin Build IOS");
            UnityEditor.BuildPipeline.BuildPlayer(scenes, outDir, target, BuildOptions.None);
            Debug.Log("End Build IOS");
        }

        // 打整包
        [MenuItem("CIBuild/BuildWin")]
        public static void BuildWin()
        {
            // 设置全局配置
            var globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>("Assets/Resources/GlobalConfig.asset");
            globalConfig.CodeMode = CodeMode.Client;
            globalConfig.PlayMode = EPlayMode.OfflinePlayMode;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // var globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");

            // 编译代码
            Debug.Log("===Compile Code Begin===");
            BuildAssembliesHelper.BuildModel(CodeOptimization.Release, globalConfig);
            BuildAssembliesHelper.BuildHotfix(CodeOptimization.Release, globalConfig);
            Debug.Log("===Compile Code End===");
            
            var outDir = System.Environment.CurrentDirectory + BuildOutputPath + "/Win";
            var outputPath = Path.Combine(outDir, Application.productName + ".exe");
            //文件夹处理
            if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
            if (File.Exists(outputPath)) File.Delete(outputPath);
            
            // HybridCLR->Generate->all
            Debug.Log("===HybridCLR Generate All Begin===");
            PrebuildCommand.GenerateAll();
            Debug.Log("===HybridCLR Generate All End===");
            
            //开始项目第一次打包，为了打出AOT使用的裁剪后的DLL
            string[] scenes = new[] { InitScenePath };
            Debug.Log("===Build App For Stripped Dll Begin===");
            UnityEditor.BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
            if (File.Exists(outputPath))
            {
                Debug.Log("Build Success :" + outputPath);
            }
            else
            {
                Debug.LogException(new Exception("Build Fail! Please Check the log! "));
            }
            Debug.Log("===Build App For Stripped Dll End===");
            
            // 把需要补充元数据的dll复制到Assets/Bundles/AotDlls目录
            Debug.Log("===Copy Aot Dll To Bundles Begin===");
            HybridCLREditor.CopyAotDllToBundles();
            Debug.Log("===Copy Aot Dll To Bundles End===");
            
            // 打包资源
            Debug.Log("===YooAsset Build Bundles Begin===");
            YooAssetEditor.BuildAssetForce(BuildTarget.StandaloneWindows64);
            Debug.Log("===YooAsset Build Bundles End===");
            
            //开始项目一键打包
            Debug.Log("===Build App Begin===");
            UnityEditor.BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
            if (File.Exists(outputPath))
            {
                Debug.Log("Build Success :" + outputPath);
            }
            else
            {
                Debug.LogException(new Exception("Build Fail! Please Check the log! "));
            }
            Debug.Log("===Build App End===");
        }

        [MenuItem("CIBuild/BuildApk")]
        public static void BuildApk()
        {
            var outDir = System.Environment.CurrentDirectory + BuildOutputPath + "/Android";
            var outputPath = Path.Combine(outDir, Application.productName + ".apk");
            //文件夹处理
            if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
            if (File.Exists(outputPath)) File.Delete(outputPath);

            //开始项目一键打包
            string[] scenes = new[] { InitScenePath };
            UnityEditor.BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.Android, BuildOptions.None);
            if (File.Exists(outputPath))
            {
                Debug.Log("Build Success :" + outputPath);
            }
            else
            {
                Debug.LogException(new Exception("Build Fail! Please Check the log! "));

            }
        }
    }
}
