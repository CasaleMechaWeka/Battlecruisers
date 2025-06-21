using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPPoolProviders
    {
        private PvPUnitPoolProvider _unitPoolProvider;
        public PvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public PvPUnitToPoolMap UnitToPoolMap { get; }

        public PvPPoolProviders()
        {
            _unitPoolProvider = new PvPUnitPoolProvider();

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _unitPoolProvider.SetInitialCapacity();
        }
    }
}