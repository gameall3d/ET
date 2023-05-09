﻿using System;
using System.Threading;
using CommandLine;
using UnityEngine;
using YooAsset;

namespace ET
{
	public class Init: MonoBehaviour
	{
		public GlobalConfig GlobalConfig;
		
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
			
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
				
			Game.AddSingleton<MainThreadSynchronizationContext>();

			// 命令行参数
			string[] args = "".Split(" ");
			Parser.Default.ParseArguments<Options>(args)
				.WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
				.WithParsed(Game.AddSingleton);
			
			Game.AddSingleton<TimeInfo>();
			Game.AddSingleton<Logger>().ILog = new UnityLogger();
			Game.AddSingleton<ObjectPool>();
			Game.AddSingleton<IdGenerater>();
			Game.AddSingleton<EventSystem>();
			Game.AddSingleton<TimerComponent>();
			Game.AddSingleton<CoroutineLockComponent>();
			
			ETTask.ExceptionHandler += Log.Error;

			// Game.AddSingleton<CodeLoader>().Start();
			this.CheckUpdate().Coroutine();
		}

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

		private void Update()
		{
			Game.Update();
		}

		private void LateUpdate()
		{
			Game.LateUpdate();
			Game.FrameFinishUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}