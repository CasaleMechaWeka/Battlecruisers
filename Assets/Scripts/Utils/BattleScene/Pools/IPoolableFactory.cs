namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPoolableFactory<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        TPoolable CreateItem();
    }
}