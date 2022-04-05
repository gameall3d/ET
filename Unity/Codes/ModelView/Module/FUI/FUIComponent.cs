using FairyGUI;

namespace ET
{


	/// <summary>
	/// 管理所有顶层UI, 顶层UI都是GRoot的孩子
	/// </summary>
	public class FUIComponent: Entity, IAwake
	{
		public FUI Root;
		
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