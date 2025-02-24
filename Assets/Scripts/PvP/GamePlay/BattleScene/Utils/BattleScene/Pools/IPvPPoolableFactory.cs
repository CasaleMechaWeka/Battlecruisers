using System.Threading.Tasks;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPoolableFactory<TPvPPoolable, TPvPArgs> where TPvPPoolable : IPoolable<TPvPArgs>
    {
        Task<TPvPPoolable> CreateItem();
    }
}