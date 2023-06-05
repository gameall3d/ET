using System;
using System.Collections.Generic;
using ET;

namespace CubeFighter
{
    [FriendOf(typeof(GameNumericWatcherComponent))]
    public static class GameNumericWatcherComponentSystem
    {
        public class GameNumericWatcherComponentAwakeSystem : AwakeSystem<GameNumericWatcherComponent>
        {
            protected override void Awake(GameNumericWatcherComponent self)
            {
                GameNumericWatcherComponent.Instance = self;
                self.Init();
            }
        }


        public class GameNumericWatcherComponentLoadSystem : LoadSystem<GameNumericWatcherComponent>
        {
            protected override void Load(GameNumericWatcherComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this GameNumericWatcherComponent self)
        {
            self.allWatchers = new Dictionary<int, List<GameNumericWatcherInfo>>();

            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof(GameNumericWatcherAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(GameNumericWatcherAttribute), false);

                foreach (object attr in attrs)
                {
                    GameNumericWatcherAttribute numericWatcherAttribute = (GameNumericWatcherAttribute)attr;
                    IGameNumericWatcher obj = (IGameNumericWatcher)Activator.CreateInstance(type);
                    GameNumericWatcherInfo numericWatcherInfo = new GameNumericWatcherInfo(numericWatcherAttribute.SceneType, obj);
                    if (!self.allWatchers.ContainsKey(numericWatcherAttribute.NumericType))
                    {
                        self.allWatchers.Add(numericWatcherAttribute.NumericType, new List<GameNumericWatcherInfo>());
                    }
                    self.allWatchers[numericWatcherAttribute.NumericType].Add(numericWatcherInfo);
                }
            }
        }

        public static void Run(this GameNumericWatcherComponent self, GameUnit unit, EventType.NumericChange args)
        {
            List<GameNumericWatcherInfo> list;
            if (!self.allWatchers.TryGetValue(args.NumericType, out list))
            {
                return;
            }

            SceneType unitDomainSceneType = unit.DomainScene().SceneType;
            foreach (GameNumericWatcherInfo numericWatcher in list)
            {
                if (numericWatcher.SceneType != unitDomainSceneType)
                {
                    continue;
                }
                numericWatcher.INumericWatcher.Run(unit, args);
            }
        }
    }

    public class GameNumericWatcherInfo
    {
        public SceneType SceneType { get; }
        public IGameNumericWatcher INumericWatcher { get; }

        public GameNumericWatcherInfo(SceneType sceneType, IGameNumericWatcher numericWatcher)
        {
            this.SceneType = sceneType;
            this.INumericWatcher = numericWatcher;
        }
    }
    
    /// <summary>
    /// 监视数值变化组件,分发监听
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class GameNumericWatcherComponent: Entity, IAwake, ILoad
    {
        public static GameNumericWatcherComponent Instance { get; set; }
		
        public Dictionary<int, List<GameNumericWatcherInfo>> allWatchers;
    }
}
