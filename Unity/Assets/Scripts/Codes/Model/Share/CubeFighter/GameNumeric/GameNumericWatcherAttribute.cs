using ET;

namespace CubeFighter
{
    public class GameNumericWatcherAttribute : BaseAttribute
    {
        public SceneType SceneType { get; }
		
        public int NumericType { get; }

        public GameNumericWatcherAttribute(SceneType sceneType, int type)
        {
            this.SceneType = sceneType;
            this.NumericType = type;
        }
    }
}