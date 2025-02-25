using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }


        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            BulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BulletImpact);
            HighCalibreBulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HighCalibreBulletImpact);
            TinyBulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.TinyBulletImpact);
            NovaShellImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.NovaShellImpact);
            RocketShellImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.RocketShellImpact);
            BombExplosionPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BombExplosion);
            FlakExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.FlakExplosion);
            SmallExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion75);
            MediumExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion100);
            LargeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion150);
            HugeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion500);
            FirecrackerExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.ExplosionFirecracker);
            MFExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.ExplosionMF);
            FiveShellClusterExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.ExplosionFiveShellCluster);
        }

        private IPool<IPoolable<Vector3>, Vector3> CreateExplosionPool(IPrefabFactory prefabFactory, ExplosionKey explosionKey)
        {
            return
                new Pool<IPoolable<Vector3>, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        explosionKey));
        }

        public void SetInitialCapacity()
        {
            TinyBulletImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
            HighCalibreBulletImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
            BulletImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
            BombExplosionPool.AddCapacity(InitialCapacity.BOMB);
            FlakExplosionsPool.AddCapacity(InitialCapacity.FLAK);
            SmallExplosionsPool.AddCapacity(InitialCapacity.SMALL);
            MediumExplosionsPool.AddCapacity(InitialCapacity.MEDIUM);
            FirecrackerExplosionsPool.AddCapacity(InitialCapacity.MEDIUM);
            LargeExplosionsPool.AddCapacity(InitialCapacity.LARGE);
            NovaShellImpactPool.AddCapacity(InitialCapacity.LARGE);
            RocketShellImpactPool.AddCapacity(InitialCapacity.MEDIUM);
            HugeExplosionsPool.AddCapacity(InitialCapacity.HUGE);
            MFExplosionsPool.AddCapacity(InitialCapacity.MEDIUM);
            FiveShellClusterExplosionsPool.AddCapacity(InitialCapacity.MEDIUM);
        }
    }
}