namespace CubeFighter
{
    public interface IGameNumericWatcher
    {
        void Run(GameUnit unit, EventType.NumericChange args);
    }
}