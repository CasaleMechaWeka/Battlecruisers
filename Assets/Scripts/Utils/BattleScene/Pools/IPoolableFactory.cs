namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPoolableFactory<TArgs>
    {
        IPoolable<TArgs> CreateItem();
    }
}