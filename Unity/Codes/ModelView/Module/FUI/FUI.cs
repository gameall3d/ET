using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{

	
	public class FUI : Entity, IAwake<GObject>
	{
        public GObject GObject;

        public string Name
        {
            get
            {
                if(GObject == null)
                {
                    return string.Empty;
                }

                return GObject.name;
            }

            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.name = value;
            }
        }
        
        public bool Visible
        {
            get
            {
                if (GObject == null)
                {
                    return false;
                }

                return GObject.visible;
            }
            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.visible = value;
            }
        }

        public bool IsComponent
        {
            get
            {
                return GObject is GComponent;
            }
        }

        public bool IsRoot
        {
            get
            {
                return GObject is GRoot;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return GObject == null;
            }
        }

        public Dictionary<string, FUI> FUIChildren = new Dictionary<string, FUI>();

        protected bool isFromFGUIPool = false;
		
		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}
			
			base.Dispose();
			
			// 从父亲中删除自己
			GetParent<FUI>()?.RemoveNoDispose(Name);

			// 删除所有的孩子
			foreach (FUI ui in FUIChildren.Values.ToArray())
			{
				ui.Dispose();
			}

            FUIChildren.Clear();

            // 删除自己的UI
            if (!IsRoot && !isFromFGUIPool)
            {
                GObject.Dispose();
            }

            GObject = null;
            isFromFGUIPool = false;
        }

        /// <summary>
        /// 一般情况不要使用此方法，如需使用，需要自行管理返回值的FUI的释放。
        /// </summary>
        public  FUI RemoveNoDispose(string name)
        {
            if (IsDisposed)
            {
                return null;
            }

            FUI ui;

            if (FUIChildren.TryGetValue(name, out ui))
            {
                FUIChildren.Remove(name);

                if (ui != null)
                {
                    if (IsComponent)
                    {
                        GObject.asCom.RemoveChild(ui.GObject, false);
                    }

                    ui.Parent?.RemoveChild(ui);
                }
            }

            return ui;
        }

        private static GObject CreateGObject(string packageName, string resName)
        {
            return UIPackage.CreateObject(packageName, resName);
        }

        private static void CreateGObjectAsync(string packageName, string resName, UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(packageName, resName, result);
        }

        public static T CreateInstance<T>(Entity parent, string packageName, string resName) where T: FUI
        {
            return parent.AddChild<T, GObject>(CreateGObject(packageName, resName));
        }
        
        public static ETTask<T> CreateInstanceAsync<T>(Entity parent, string packageName, string resName) where T: FUI
        {
            ETTask<T> tcs = ETTask<T>.Create(true);
            
            CreateGObjectAsync(packageName, resName, (go) =>
            {
                tcs.SetResult(parent.AddChild<T, GObject>(go));
            });

            return tcs;
        }
        
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T Create<T>(Entity parent, GObject go) where T: FUI
        {
            return parent.AddChild<T, GObject>(go);
        }
        
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static T GetFormPool<T>(Entity domain, GObject go) where T: FUI
        {
            var fui = go.Get<T>();
        
            if(fui == null)
            {
                fui = Create<T>(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
    }
}