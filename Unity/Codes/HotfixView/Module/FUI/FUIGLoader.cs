using FairyGUI;
using UnityEngine;
using UnityEngine.U2D;

namespace ET
{
    public static class GLoaderType
    {
        public static string Default = "Default";
        public static string SpriteInAtlas = "SpriteInAtlas";   // 读取图集里的图
    }
    public class FUIGLoader : GLoader

    {
        protected override void LoadExternal()
        {
            // url名字规则为：type:bundle名/路径名
            // 例如：SpriteInAtlas: Building/BuildingAtlas/Build_1001
            var url = this.url;
            var urlArray = url.Split(':');
            if (urlArray.Length < 1)
            {
                Log.Error("url of FUIGLoader is invalid: " + url);
                this.onExternalLoadFailed();
                return;
            }
            string loaderType = urlArray[0];
            string pathUrl = urlArray[1];
            var pathArray = urlArray[1].Split('/');
            if (pathArray.Length < 1)
            {
                Log.Error("path of FUIGLoader is invalid: " + urlArray[1]);
                this.onExternalLoadFailed();
                return;
            }

            int firstSlashIndex = pathUrl.IndexOf('/');
            int lastSlashIndex = pathUrl.LastIndexOf('/');
            string bundleName = pathUrl.Substring(0, firstSlashIndex);
            string spriteName = pathUrl.Substring(lastSlashIndex+1, pathUrl.Length - lastSlashIndex - 1);
            string assetName = pathUrl.Substring(firstSlashIndex+1, lastSlashIndex - firstSlashIndex - 1);
            // Debug.LogWarning(bundleName + "|" + assetName + "|" + spriteName);
            if (loaderType == GLoaderType.SpriteInAtlas)
            {
                SpriteAtlas spriteAtlas = ResourcesComponent.Instance.GetAsset(bundleName.StringToAB(),assetName) as SpriteAtlas;
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if (sprite == null)
                {
                    Log.Error("can't load sprite in atlas: " + spriteName);
                    this.onExternalLoadFailed();
                    return;
                }
                this.onExternalLoadSuccess(new NTexture(sprite));
            }
            else
            {
                Sprite sprite = ResourcesComponent.Instance.GetAsset(bundleName.StringToAB(),assetName) as Sprite;
                if (sprite == null)
                {
                    Log.Error("can't load sprite" + spriteName);
                    this.onExternalLoadFailed();
                    return;
                }
                this.onExternalLoadSuccess(new NTexture(sprite));
            }
        }

        protected override void FreeExternal(NTexture texture)
        {
            
        }
    }
}