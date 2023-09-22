using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(GameUnitComponent))]
    [FriendOf(typeof(GameUnit))]
    public static class GameUnitComponentSystem
    {
        public class GameUnitComponentAwakeSystem: AwakeSystem<GameUnitComponent>
        {
            protected override void Awake(GameUnitComponent self)
            {
                self.Awake();
            }
        }

    

        public static void Awake(this GameUnitComponent self)
        {
        }

        // public static void SetPlayer(this GameUnitComponent self, GameUnit player)
        // {
        //     self.Player = player;
        // }

        public static GameUnit CreatePlayer(this GameUnitComponent self)
        {
            var gameUnit = self.AddChild<GameUnit>();
            gameUnit.GameUnitType = GameUnitType.Player;
            self.Player = gameUnit;
            return gameUnit;
        }

        public static GameUnit GetPlayer(this GameUnitComponent self)
        {
            return self.Player;
        }

        public static GameUnit CreateEnemy(this GameUnitComponent self)
        {
            var gameUnit = self.AddChild<GameUnit>();
            gameUnit.GameUnitType = GameUnitType.Enemy;
            self.Enemys.Add(gameUnit);
            return gameUnit;
        }

        public static List<GameUnit> GetEnemys(this GameUnitComponent self)
        {
            return self.Enemys;
        }
    }
}

