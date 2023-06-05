using ET;
using ET.Client;
using UnityEngine;

namespace CubeFighter.Client
{
    [FriendOf(typeof(PlayerViewComponent))]
    public static class PlayerViewComponentSystem
    {
        public class PlayerViewComponentAwakeSystem: AwakeSystem<PlayerViewComponent>
        {
            protected override void Awake(PlayerViewComponent self)
            {
                self.Awake();
            }
        }


        public static void Awake(this PlayerViewComponent self)
        {
            GameObject playerPrfb = (GameObject)ResComponent.Instance.LoadAssetSync<GameObject>("Player");
            GameObject go = UnityEngine.Object.Instantiate(playerPrfb, GlobalComponent.Instance.Unit, true);
            self.RootGO = go;
        }
    }
}

