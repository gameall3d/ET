using FairyGUI;
using System;
using System.Linq;

namespace ET
{
	public class FUIAwakeSystem : AwakeSystem<FUI, GObject>
	{
		public override void Awake(FUI self, GObject gObject)
		{
			self.GObject = gObject;
		}
	}

	public static class FUISystem
    {
        public static void Add(this FUI self,FUI ui, bool asChildGObject)
        {
            if (ui == null || ui.IsEmpty)
            {
                throw new Exception($"ui can not be empty");
            }

            if (string.IsNullOrWhiteSpace(ui.Name))
            {
                throw new Exception($"ui.Name can not be empty");
            }

            if (self.FUIChildren.ContainsKey(ui.Name))
            {
                throw new Exception($"ui.Name({ui.Name}) already exist");
            }

            self.FUIChildren.Add(ui.Name, ui);

            if (self.IsComponent && asChildGObject)
            {
                self.GObject.asCom.AddChild(ui.GObject);
            }

            self.AddChild(ui);
        }

        public static void MakeFullScreen(this FUI self)
        {
            self.GObject?.asCom?.MakeFullScreen();
        }

        public static void Remove(this FUI self, string name)
        {
            if (self.IsDisposed)
            {
                return;
            }

            FUI ui;

            if (self.FUIChildren.TryGetValue(name, out ui))
            {
                self.FUIChildren.Remove(name);

                if (ui != null)
                {
                    if (self.IsComponent)
                    {
                        self.GObject.asCom.RemoveChild(ui.GObject, false);
                    }

                    ui.Dispose();
                }
            }
        }

        public static void RemoveChildren(this FUI self)
        {
            foreach (var child in self.FUIChildren.Values.ToArray())
            {
                child.Dispose();
            }

            self.FUIChildren.Clear();
        }

        public static FUI Get(this FUI self, string name)
        {
            FUI child;

            if (self.FUIChildren.TryGetValue(name, out child))
            {
                return child;
            }

            return null;
        }

        public static FUI[] GetAll(this FUI self)
        {
            return self.FUIChildren.Values.ToArray();
        }
    }
}
