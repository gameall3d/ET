using CubeFighter.EventType;
using ET;

namespace CubeFighter.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(GameUnit))]
    public class AfterCreateGameUnit_CreateUnitView : AEvent<AfterCreateGameUnit>
    {
        protected override async ETTask Run(Scene scene, AfterCreateGameUnit args)
        {
            var gameUnit = args.GameUnit;

            switch (gameUnit.GameUnitType)
            {
                case GameUnitType.Player:
                    gameUnit.AddComponent<PlayerViewComponent>();
                    break;
            }
            
            await ETTask.CompletedTask;
        }
    }
}

