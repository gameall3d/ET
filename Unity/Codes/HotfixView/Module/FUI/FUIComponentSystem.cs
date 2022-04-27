using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET
{
	public class FUIComponentAwakeSystem : AwakeSystem<FUIComponent>
	{
		public override void Awake(FUIComponent self)
		{
			self.Awake();
		}
	}

	public static class FUIComponentSystem
    {
	    public static void Awake(this FUIComponent self)
	    {
		    FUIComponent.Instance = self;
		    self.Root = self.Domain.AddChild<FUI, GObject>(GRoot.inst);
		    
		    // 设置加载器
		    UIObjectFactory.SetLoaderExtension(typeof(FUIGLoader));
	    }
	    
		public static void Add(this FUIComponent self, FUI ui, bool asChildGObject)
		{
			self.Root?.Add(ui, asChildGObject);
		}

		public static void Remove(this FUIComponent self, string name)
		{
			self.Root?.Remove(name);
		}

		public static FUI Get(this FUIComponent self, string name)
		{
			return self.Root?.Get(name);
		}

		public static FUI[] GetAll(this FUIComponent self)
		{
			return self.Root?.GetAll();
		}

		public static void Clear(this FUIComponent self)
		{
			var childrens = self.GetAll();

			if (childrens != null)
			{
				foreach (var fui in childrens)
				{
					self.Remove(fui.Name);
				}
			}
		}

		public static void AddWindow(this FUIComponent self, string name, FUIWindowComponent window)
		{
			self.AvailableWindows.Add(name, window);
		}

		public static FUIWindowComponent GetWindow(this FUIComponent self, string name)
		{
			if (self.AvailableWindows.ContainsKey(name))
			{
				return self.AvailableWindows[name];	
			}

			return null;
		}

		public static void HideWindow(this FUIComponent self, string name)
		{
			if (self.AvailableWindows.TryGetValue(name, out FUIWindowComponent win))
			{
				win.Window?.Hide();
			}
		}

		/// <summary>
		/// 异步显示一个UI组件
		/// </summary>
		/// <param name="packageBundle">组件所在的ab包</param>
		/// <typeparam name="T">FUI组件类型</typeparam>
		/// <typeparam name="K">对应的自定义逻辑组件类型</typeparam>
		/// <returns>对应的自定义逻辑组件 </returns>
		public static async ETTask<K> ShowUIAsync<T, K>(this FUIComponent self, string packageBundle) where T : FUI where K : Entity, IAwake, new()
		{
			// 对应fairgui中的包内部的资源名
			string uiResName = (string) typeof (T).GetField("UIResName").GetValue(null);
			if (self.AvailableUI.TryGetValue(uiResName, out Entity comp))
			{
				FUI fui = comp.GetParent<FUI>();
				fui.Visible = true;
				return comp as K;
			}
			else
			{
				var zoneScene = self.ZoneScene();
				await zoneScene.GetComponent<FUIPackageComponent>().AddPackageAsync(packageBundle);
				// 对应fairgui中的包名
				string uiPackageName = (string) typeof(T).GetField("UIPackageName").GetValue(null);

				T fui = await FUI.CreateInstanceAsync<T>(zoneScene, uiPackageName, uiResName);
				fui.Name = uiResName;
				fui.MakeFullScreen();

				var logicComp = fui.AddComponent<K>();
				self.Add(fui, true);
				self.AvailableUI.Add(uiResName, logicComp);
				return logicComp;
			}
			
			
		}
		
		public static K ShowUI<T, K>(this FUIComponent self, string packageBundle) where T : FUI where K : Entity, IAwake, new()
		{
			string uiResName = (string) typeof (T).GetField("UIResName").GetValue(null);
			if (self.AvailableUI.TryGetValue(uiResName, out Entity comp))
			{
				FUI fui = comp.GetParent<FUI>();
				fui.Visible = true;
				return comp as K;
			}
			else
			{
				var zoneScene = self.ZoneScene();
				zoneScene.GetComponent<FUIPackageComponent>().AddPackage(packageBundle);
				// 对应fairgui中的包名
				string uiPackageName = (string) typeof(T).GetField("UIPackageName").GetValue(null);
				// 对应fairgui中的包内部的资源名
			
				T fui = FUI.CreateInstance<T>(zoneScene, uiPackageName, uiResName);

				fui.Name = uiResName;
				fui.MakeFullScreen();

				var logicComp = fui.AddComponent<K>();
				self.Add(fui, true);
				self.AvailableUI.Add(uiResName, logicComp);
				return logicComp;
			}
		}

		public static Entity GetUI(this FUIComponent self, string name)
		{
			if (self.AvailableUI.TryGetValue(name, out Entity comp))
			{
				return comp;
			}
			
			return null;
		}

		public static void HideUI(this FUIComponent self, string name)
		{
			if (self.AvailableUI.TryGetValue(name, out Entity comp))
			{
				FUI fui = comp.GetParent<FUI>();
				fui.Visible = false;
			}
		}

		public static void RemoveUI(this FUIComponent self, string name)
		{
			self.AvailableUI.Remove(name);
			self.Remove(name);
		}
		
		public static K ShowWindow<T, K>(this FUIComponent self, string packageBundle) where T : FUI
				where K : FUIWindowComponent, new()
		{
			var zoneScene = self.ZoneScene();

			var type = typeof (T);

			// 对应fairgui中的包内部的资源名
			string uiResName = (string) type.GetField("UIResName").GetValue(null);
			var win = self.GetWindow(uiResName);
			if (win != null)
			{
				win.Window?.Show();
				return win as K;
			}
			else
			{
				// 对应fairgui中的包名
				string uiPackageName = (string) type.GetField("UIPackageName").GetValue(null);
				
				zoneScene.GetComponent<FUIPackageComponent>().AddPackage(packageBundle);
				T fui = FUI.CreateInstance<T>(zoneScene, uiPackageName, uiResName);

				fui.Name = uiResName;
				fui.MakeFullScreen();
				var logicComp = fui.AddComponent<K>();
				self.AddWindow(uiResName, logicComp);
				logicComp.Window?.Show();
				return logicComp;
			}
		}

		public static async ETTask<K> ShowWindowAsync<T, K>(this FUIComponent self, string packageBundle) where T : FUI
				where K : FUIWindowComponent, new()
		{
			var zoneScene = self.ZoneScene();

			var type = typeof (T);

			// 对应fairgui中的包内部的资源名
			string uiResName = (string) type.GetField("UIResName").GetValue(null);
			var win = self.GetWindow(uiResName);
			if (win != null)
			{
				win.Window?.Show();
				return win as K;
			}
			else
			{
				// 对应fairgui中的包名
				string uiPackageName = (string) type.GetField("UIPackageName").GetValue(null);
				
				await zoneScene.GetComponent<FUIPackageComponent>().AddPackageAsync(packageBundle);
				T fui = await FUI.CreateInstanceAsync<T>(zoneScene, uiPackageName, uiResName);

				fui.Name = uiResName;
				fui.MakeFullScreen();
				var logicComp = fui.AddComponent<K>();
				self.AddWindow(uiResName, logicComp);
				logicComp.Window?.Show();
				return logicComp;
			}
		}
    }
}
