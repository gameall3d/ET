using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace ET
{
	public class FUIComponentAwakeSystem : AwakeSystem<FUIComponent>
	{
		public override void Awake(FUIComponent self)
		{
			self.Root = self.Domain.AddChild<FUI, GObject>(GRoot.inst);
		}
	}

	public static class FUIComponentSystem
    {
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
	}
}
