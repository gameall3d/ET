using ET;

namespace CubeFighter.Client
{
    public static class UnitFactory
    {
        public static GameUnit CreatePlayer(Scene currentScene)
        {
            GameUnitComponent gameUnitComponent = currentScene.GetComponent<GameUnitComponent>();
            GameUnit gameUnit = gameUnitComponent.CreatePlayer();
            
            NumericComponent numericComponent = gameUnit.AddComponent<NumericComponent>();

            return gameUnit;
        }
    }
}

