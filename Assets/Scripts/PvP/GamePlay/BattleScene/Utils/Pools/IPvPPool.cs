using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools
{
    public interface IPvPPool<TPvPPoolable, TArgs> where TPvPPoolable : IPvPPoolable<TArgs>
    {
        void AddCapacity(int capacityToAdd);
        TPvPPoolable GetItem(TArgs activationArgs);
        TPvPPoolable GetItem(TArgs activationArgs, PvPFaction faction);
        void SetMaxLimit(int amount);
    }
}