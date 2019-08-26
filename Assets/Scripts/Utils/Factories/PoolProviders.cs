using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders : IPoolProviders
    {
        public IExplosionPoolProvider ExplosionPoolProvider { get; }
        public IProjectilePoolProvider ProjectilePoolProvider { get; }
        public IUnitPoolProvider UnitPoolProvider { get; }

        public PoolProviders(IFactoryProvider factoryProvider, IUIManager uiManager)
        {
            Helper.AssertIsNotNull(factoryProvider, uiManager);

            ExplosionPoolProvider = new ExplosionPoolProvider(factoryProvider.PrefabFactory);
            ProjectilePoolProvider = new ProjectilePoolProvider(factoryProvider);
            UnitPoolProvider = new UnitPoolProvider(uiManager, factoryProvider);
        }
    }
}