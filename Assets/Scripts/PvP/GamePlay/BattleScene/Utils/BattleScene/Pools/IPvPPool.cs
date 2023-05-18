using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPool<TPoolable, TArgs> where TPoolable : IPvPPoolable<TArgs>
    {
        void AddCapacity(int capacityToAdd);
        TPoolable GetItem(TArgs activationArgs);
        TPoolable GetItem(TArgs activationArgs, PvPFaction faction);
        void SetMaxLimit(int amount);
    }
}