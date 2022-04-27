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
			await zoneScene.GetComponent<FUIComponent>().ShowUIAsync<FUI_Login, FUILoginComponent>(FUI_Login.UIPackageName);
		}
	}
}
