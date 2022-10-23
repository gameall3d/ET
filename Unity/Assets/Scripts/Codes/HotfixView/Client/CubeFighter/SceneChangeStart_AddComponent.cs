using CubeFighter.EventType;
using ET;
using ET.Client;
using UnityEngine.SceneManagement;
using Scene = ET.Scene;

namespace CubeFighter.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeStart_AddComponent : AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();
            
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景

            await SceneManager.LoadSceneAsync(currentScene.Name);
            
            GameUnit unit = UnitFactory.CreatePlayer(currentScene);
            
            EventSystem.Instance.Publish(currentScene, new AfterCreateGameUnit(){GameUnit = unit});
        }
    }
}

