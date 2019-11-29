namespace BattleCruisers.Utils.BattleScene.Pools
{
    public class DummyPool<TPoolable, TArgs> : IPool<TPoolable, TArgs> where TPoolable : class, IPoolable<TArgs>
    {
        public void AddCapacity(int capacityToAdd) { }
        public TPoolable GetItem(TArgs activationArgs) => null;
    }
}