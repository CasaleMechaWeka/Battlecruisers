using BattleCruisers.Buildables;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPool<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        void AddCapacity(int capacityToAdd);
        TPoolable GetItem(TArgs activationArgs);
        TPoolable GetItem(TArgs activationArgs, Faction faction);
        void SetMaxLimit(int amount);
    }
}