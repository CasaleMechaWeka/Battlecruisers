using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders
    {
        private ShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private ProjectilePoolProvider _projectilePoolProvider;
        public IProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private UnitPoolProvider _unitPoolProvider;
        public UnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public UnitToPoolMap UnitToPoolMap { get; }

        public PoolProviders(IUIManager uiManager)
        {
            Helper.AssertIsNotNull(uiManager);

            _shipDeathPoolProvider = new ShipDeathPoolProvider();
            _projectilePoolProvider = new ProjectilePoolProvider();
            _unitPoolProvider = new UnitPoolProvider(uiManager);

            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }
    }
}