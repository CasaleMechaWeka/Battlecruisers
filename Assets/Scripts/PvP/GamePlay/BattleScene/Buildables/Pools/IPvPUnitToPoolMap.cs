using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public interface IPvPUnitToPoolMap
    {
        Pool<PvPUnit, PvPBuildableActivationArgs> GetPool(IPvPUnit unit);
    }
}