using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPoolableFactory<TPvPPoolable, TPvPArgs> where TPvPPoolable : IPvPPoolable<TPvPArgs>
    {
        Task<TPvPPoolable> CreateItem();
    }
}