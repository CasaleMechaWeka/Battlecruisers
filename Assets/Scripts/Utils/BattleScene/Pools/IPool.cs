namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPool<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        TPoolable GetItem(TArgs activationArgs);
    }
}