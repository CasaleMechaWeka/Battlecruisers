using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public Pool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> RailSlugImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }


        public ExplosionPoolProvider()
        {
            BulletImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.BulletImpact);
            HighCalibreBulletImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.HighCalibreBulletImpact);
            TinyBulletImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.TinyBulletImpact);
            RailSlugImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.RailSlugImpact);
            NovaShellImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.NovaShellImpact);
            RocketShellImpactPool = CreateExplosionPool(StaticPrefabKeys.Explosions.RocketShellImpact);
            BombExplosionPool = CreateExplosionPool(StaticPrefabKeys.Explosions.BombExplosion);
            FlakExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.FlakExplosion);
            SmallExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.Explosion75);
            MediumExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.Explosion100);
            LargeExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.Explosion150);
            HugeExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.Explosion500);
            FirecrackerExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.ExplosionFirecracker);
            MFExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.ExplosionMF);
            FiveShellClusterExplosionsPool = CreateExplosionPool(StaticPrefabKeys.Explosions.ExplosionFiveShellCluster);
        }

        private Pool<IPoolable<Vector3>, Vector3> CreateExplosionPool(ExplosionKey explosionKey)
        {
            return
                new Pool<IPoolable<Vector3>, Vector3>(
                    new ExplosionFactory(explosionKey));
        }

        public void SetInitialCapacity()
        {
            TinyBulletImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
            RailSlugImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
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