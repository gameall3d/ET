using System.Numerics;
using ET;

namespace CubeFighter
{
    [ChildOf(typeof(GameUnitComponent))]
    public class GameUnit: Entity, IAwake, IDestroy
    {
        public GameUnitType GameUnitType;
        public Vector2 Position;
    }
}

