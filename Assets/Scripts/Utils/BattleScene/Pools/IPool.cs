namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPool<TArgs>
    {
        IPoolable<TArgs> GetItem(TArgs activationArgs);
    }
}