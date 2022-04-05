using FairyGUI;
using System.Collections.Generic;

namespace ET
{
	/// <summary>
	/// 管理所有UI Package
	/// </summary>
	public class FUIPackageComponent : Entity, IAwake
    {
        public const string FUI_PACKAGE_DIR = "Assets/Bundles/FUI";

        public readonly Dictionary<string, UIPackage> Packages = new Dictionary<string, UIPackage>();
	}
}