using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.Pools;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders : IPoolProviders
    {
        public IExplosionPoolProvider ExplosionPoolProvider { get; }
        public IProjectilePoolProvider ProjectilePoolProvider { get; }

        public PoolProviders(IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            // FELIX  Prefab factory is all good :)
            ExplosionPoolProvider = new ExplosionPoolProvider(factoryProvider.PrefabFactory);

            // FELIX  IFactoryProvider not so much :P
            ProjectilePoolProvider = new ProjectilePoolProvider(factoryProvider);
        }
    }
}