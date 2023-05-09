# YooAsset接入参考
[YooAsset](https://github.com/tuyoogame/YooAsset)是一套用于Unity3D的资源管理系统，用于帮助研发团队快速部署和交付游戏。

## 一、配置YooAsset
### 1. [下载安装](https://www.yooasset.com/docs/guide-editor/QuickStart)
可以直接在Packages/manifest.json中直接添加配置信息,注意填写的版本号是你需要的版本号，一般就填最新的。

```
{
  "dependencies": {
    "com.tuyoogame.yooasset": "1.4.12",
    ......
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.cn",
      "url": "https://package.openupm.cn",
      "scopes": [
        "com.tuyoogame.yooasset"
      ]
    }
  ]
}
```
### 2. 全局配置
通过右键创建配置文件（Project窗体内右键 -> Create -> YooAsset -> Create Setting）
==注意：请将配置文件放在Resources文件夹下==

## 二、在ET中使用YooAsset
### 1. 写一个YooAssetWrapper
YooAsset默认使用的是Coroutine的方式来做异步等待，为了更好的和ET框架融合，这里将其中的方法都封装一层，返回ETTask。这里创建了两个文件：YooAssetWrapper.cs 和 YooAssetOperationExtensions.cs ，放在了Assets\Scripts\Loader\Helper文件夹中.
==注意Unity.Loader.asmdef要引用YooAsset的asmdef.==
Wrapper的实现有参考[L大的文章](http://www.liuocean.com/2022/07/16/ji-yu-ecs-she-ji-xia-de-jia-zai-guan-li/)。
### 2. 跑通更新流程
现在我们需要在ET的框架中初始化并使用YooAsset了，找到ET客户端初始化的代码Assets/Scripts/Loader/MonoBehaviour/Init.cs。在其中的CodeLoader的Start方法调用之前，我们需要走完热更新流程，所以我们在这里加入一个CheckUpdate方法。

```C#
		private async ETTask CheckUpdate()
		{
			YooAssets.Initialize();
			
			await YooAssetWrapper.InitializeAsync(this.GlobalConfig.PlayMode);
			string version = await YooAssetWrapper.UpdateStaticVersion();
			Debug.Log(version);
			var result = await YooAssetWrapper.UpdateManifest(version);

			if (!result)
			{
				UICheckUpdate.Instance.SetMessage("Update Manifest Failed");
			}
			
			YooAssetWrapper.GetDownloadSize();
			result = await YooAssetWrapper.Download();
			if (result)
			{
				if (this.GlobalConfig.PlayMode == EPlayMode.HostPlayMode)
				{
					UICheckUpdate.Instance.Remove();
				}

				Game.AddSingleton<CodeLoader>().Start();
			}
			else
			{
				UICheckUpdate.Instance.SetMessage("Download Resource Failed");
			}
		}
```

### 3. 增加ResComponent
新增一个ResComponent（因为ResourcesComponent被ET默认占用了，所以用一个接近的名字），用于统一上层对外资源加载的接口，这样以后可以方便的替换各种资源加载插件。在客户端初始中加上ResComponent，然后把项目中原先使用ResourcesComponent的地方替换成ResComponent,还有一些地方使用AssetsBundleHelper，也替换成YooAssetWrapper。
这里会发现ResourcesComponent有两处添加，原因可以看这个[帖子](https://et-framework.cn/d/813-resourcescomponent)。