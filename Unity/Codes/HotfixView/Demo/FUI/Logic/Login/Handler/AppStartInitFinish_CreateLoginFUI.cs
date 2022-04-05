using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    internal class AppStartInitFinish_CreateLoginFUI : AEvent<EventType.AppStartInitFinish>
	{
		protected override void Run(EventType.AppStartInitFinish args)
		{
			this.RunAsync(args).Coroutine();
		}
		protected async ETTask RunAsync(EventType.AppStartInitFinish args)
		{
			Scene zoneScene = args.ZoneScene;
			await zoneScene.GetComponent<FUIPackageComponent>().AddPackageAsync(FUIPackage.Login);
			var fuiLogin = await FUI_Login.CreateInstanceAsync(zoneScene);

			fuiLogin.Name = FUI_Login.UIResName;
			fuiLogin.MakeFullScreen();

			fuiLogin.AddComponent<FUILoginComponent>();
			zoneScene.GetComponent<FUIComponent>().Add(fuiLogin, true);
		}
	}
}
