using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPPoolProviders
    {
        private PvPUnitPoolProvider _unitPoolProvider;
        public PvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public PvPUnitToPoolMap UnitToPoolMap { get; }

        public PvPPoolProviders(IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(droneFactory);

            _unitPoolProvider = new PvPUnitPoolProvider();

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }


        public PvPPoolProviders(
            PvPUIManager uiManager,
            IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(uiManager, droneFactory);

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