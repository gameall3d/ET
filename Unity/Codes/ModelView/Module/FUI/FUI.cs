using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}