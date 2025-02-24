using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPool<TPoolable, TArgs> where TPoolable : IPoolable<TArgs>
    {
        Task AddCapacity(int capacityToAdd);
        Task<TPoolable> GetItem(TArgs activationArgs);
        Task<TPoolable> GetItem(TArgs activationArgs, Faction faction);
        void SetMaxLimit(int amount);
    }
}