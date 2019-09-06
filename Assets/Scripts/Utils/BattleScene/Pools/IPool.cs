namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPool<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        void AddCapacity(int capacityToAdd);
        TPoolable GetItem(TArgs activationArgs);
    }
}