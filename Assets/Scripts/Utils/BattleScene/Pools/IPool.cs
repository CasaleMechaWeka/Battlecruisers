namespace BattleCruisers.Utils.BattleScene.Pools
{
    // FELIX  Don't return IPoolable, return generic that extends IPoolable :)
    // That way will get IUnit instead of IPoolable :)
    public interface IPool<TArgs>
    {
        IPoolable<TArgs> GetItem(TArgs activationArgs);
    }
}