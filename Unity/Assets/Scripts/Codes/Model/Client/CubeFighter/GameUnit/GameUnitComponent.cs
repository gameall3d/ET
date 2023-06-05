using System.Collections.Generic;
using ET;

namespace CubeFighter
{
    [ComponentOf(typeof(Scene))]
    public class GameUnitComponent: Entity, IAwake, IDestroy
    {
        public GameUnit Player;

        public List<GameUnit> Enemys = new List<GameUnit>();
    }
}

