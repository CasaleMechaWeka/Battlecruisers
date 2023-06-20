using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPool<TPoolable, TArgs> where TPoolable : IPvPPoolable<TArgs>
    {
        Task AddCapacity(int capacityToAdd);
        Task<TPoolable> GetItem(TArgs activationArgs);
        Task<TPoolable> GetItem(TArgs activationArgs, PvPFaction faction);
        void SetMaxLimit(int amount);
    }
}