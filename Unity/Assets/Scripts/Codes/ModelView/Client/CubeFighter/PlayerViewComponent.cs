using ET;
using UnityEngine;

namespace CubeFighter.Client
{
    [ComponentOf(typeof(GameUnit))]
    public class PlayerViewComponent: Entity, IAwake, IDestroy
    {
        public GameObject UnitRoot;
    }
}

