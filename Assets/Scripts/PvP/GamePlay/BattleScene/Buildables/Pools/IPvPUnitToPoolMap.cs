using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public interface IPvPUnitToPoolMap
    {
        IPvPPool<PvPUnit, PvPBuildableActivationArgs> GetPool(IPvPUnit unit);
    }
}