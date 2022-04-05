using FairyGUI;
using UnityEngine;
using System.Threading.Tasks;

namespace ET
{
    public static class FUIPackageComponentSystem
    {
		public static void AddPackage(this FUIPackageComponent self, string type)
		{
			if (Define.IsEditor)
			{
				UIPackage uiPackage = UIPackage.AddPackage($"{FUIPackageComponent.FUI_PACKAGE_DIR}/{type}");
				self.Packages.Add(type, uiPackage);
			}
			else
			{
				string uiBundleDesName = AssetBundleHelper.StringToAB($"{type}_fui");
				string uiBundleResName = AssetBundleHelper.StringToAB(type);
				ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
				resourcesComponent.LoadBundle(uiBundleDesName);
				resourcesComponent.LoadBundle(uiBundleResName);

				AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
				AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
				UIPackage uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);
				self.Packages.Add(type, uiPackage);
			}
		}

		public static async Task AddPackageAsync(this FUIPackageComponent self, string type)
		{
			if (Define.IsEditor)
			{
				await Task.CompletedTask;

				UIPackage uiPackage = UIPackage.AddPackage($"{FUIPackageComponent.FUI_PACKAGE_DIR}/{type}");

				self.Packages.Add(type, uiPackage);
			}
			else
			{
				string uiBundleDesName = AssetBundleHelper.StringToAB($"{type}_fui");
				string uiBundleResName = AssetBundleHelper.StringToAB(type);
				ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
				await resourcesComponent.LoadBundleAsync(uiBundleDesName);
				await resourcesComponent.LoadBundleAsync(uiBundleResName);

				AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
				AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
				UIPackage uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);

				self.Packages.Add(type, uiPackage);
			}
		}

		public static void RemovePackage(this FUIPackageComponent self, string type)
		{
			UIPackage package;

			if (self.Packages.TryGetValue(type, out package))
			{
				var p = UIPackage.GetByName(package.name);

				if (p != null)
				{
					UIPackage.RemovePackage(package.name);
				}

				self.Packages.Remove(package.name);
			}

			if (!Define.IsEditor)
			{
				string uiBundleDesName = AssetBundleHelper.StringToAB($"{type}_fui");
				string uiBundleResName = AssetBundleHelper.StringToAB(type);
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleDesName);
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleResName);
			}
		}
	}
}
