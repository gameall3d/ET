using System.Collections.Generic;
using FairyGUI;

namespace ET
{


	/// <summary>
	/// 管理所有顶层UI, 顶层UI都是GRoot的孩子
	/// </summary>
	public class FUIComponent: Entity, IAwake
	{
		public static FUIComponent Instance { get; set; }
		public FUI Root;
		public Dictionary<string, FUIWindowComponent> AvailableWindows = new Dictionary<string, FUIWindowComponent>();
		public Dictionary<string, Entity> AvailableUI = new Dictionary<string, Entity>();
		public bool IsButtonClicked { get; set; }

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			base.Dispose();

            Root?.Dispose();
            Root = null;
		}

		
	}
}