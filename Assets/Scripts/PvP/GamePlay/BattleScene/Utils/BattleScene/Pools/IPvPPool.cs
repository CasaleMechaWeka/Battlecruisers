using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPool<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        void AddCapacity(int capacityToAdd);
        TPoolable GetItem(TArgs activationArgs);
        TPoolable GetItem(TArgs activationArgs, Faction faction);
        void SetMaxLimit(int amount);
    }
}