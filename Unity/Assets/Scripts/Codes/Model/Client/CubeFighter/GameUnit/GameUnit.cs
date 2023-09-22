using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(GameUnitComponent))]
    public class GameUnit: Entity, IAwake, IDestroy
    {
        public GameUnitType GameUnitType;
        public float2 Position;
    }
}

