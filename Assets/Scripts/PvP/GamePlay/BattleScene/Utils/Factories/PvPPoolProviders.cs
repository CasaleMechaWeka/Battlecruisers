using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPPoolProviders
    {
        private PvPShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private PvPProjectilePoolProvider _projectilePoolProvider;
        public IPvPProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private PvPUnitPoolProvider _unitPoolProvider;
        public PvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public PvPUnitToPoolMap UnitToPoolMap { get; }

        public PvPPoolProviders(IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(droneFactory);

            _shipDeathPoolProvider = new PvPShipDeathPoolProvider();
            _projectilePoolProvider = new PvPProjectilePoolProvider();
            _unitPoolProvider = new PvPUnitPoolProvider();

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }


        public PvPPoolProviders(
            IPvPUIManager uiManager,
            IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(uiManager, droneFactory);

            _shipDeathPoolProvider = new PvPShipDeathPoolProvider();
            _projectilePoolProvider = new PvPProjectilePoolProvider();
            _unitPoolProvider = new PvPUnitPoolProvider();

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _shipDeathPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
        }

        public void SetInitialCapacities_Rest()
        {
            _projectilePoolProvider.SetInitialCapacity_Rest();
        }
    }
}