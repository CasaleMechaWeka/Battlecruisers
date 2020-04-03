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
        public IPool<IExplosion, Vector3> BombExplosionPool { get; }
        public IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        public IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        public IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        public IPool<IExplosion, Vector3> HugeExplosionsPool { get; }

        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            BulletImpactPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BulletImpact);
            BombExplosionPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.BombExplosion);
            SmallExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion75);
            MediumExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion100);
            LargeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion150);
            HugeExplosionsPool = CreateExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.Explosion500);
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
            BulletImpactPool.AddCapacity(InitialCapacity.BULLET_IMPACT);
            BombExplosionPool.AddCapacity(InitialCapacity.BOMB);
            SmallExplosionsPool.AddCapacity(InitialCapacity.SMALL);
            MediumExplosionsPool.AddCapacity(InitialCapacity.MEDIUM);
            LargeExplosionsPool.AddCapacity(InitialCapacity.LARGE);
            HugeExplosionsPool.AddCapacity(InitialCapacity.HUGE);
        }
    }
}