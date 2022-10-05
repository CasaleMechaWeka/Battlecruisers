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
        public IPool<IExplosion, Vector3> BulletImpactPool { get; }
        public IPool<IExplosion, Vector3> HighCalibreBulletImpactPool { get; }
        public IPool<IExplosion, Vector3> TinyBulletImpactPool { get; }
        public IPool<IExplosion, Vector3> BombExplosionPool { get; }
        public IPool<IExplosion, Vector3> FlakExplosionsPool { get; }
        public IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        public IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        public IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        public IPool<IExplosion, Vector3> HugeExplosionsPool { get; }
        public IPool<IExplosion, Vector3> NovaShellImpactPool { get; }


        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            BulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BulletImpact);
            HighCalibreBulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HighCalibreBulletImpact);
            TinyBulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.TinyBulletImpact);
            BombExplosionPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BombExplosion);
            FlakExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.FlakExplosion);
            SmallExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion75);
            MediumExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion100);
            LargeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion150);
            HugeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion500);
            NovaShellImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.NovaShellImpact);
        }

        private IPool<IExplosion, Vector3> CreateExplosionPool(IPrefabFactory prefabFactory, ExplosionKey explosionKey)
        {
            return
                new Pool<IExplosion, Vector3>(
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
            LargeExplosionsPool.AddCapacity(InitialCapacity.LARGE);
            HugeExplosionsPool.AddCapacity(InitialCapacity.HUGE);
            NovaShellImpactPool.AddCapacity(InitialCapacity.LARGE);
        }
    }
}