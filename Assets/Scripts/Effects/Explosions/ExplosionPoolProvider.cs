using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<IExplosion, Vector3> BulletImpactPool { get; }
        public IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        public IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        public IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        public IPool<IExplosion, Vector3> HugeExplosionsPool { get; }

        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            BulletImpactPool
                = new Pool<IExplosion, Vector3>(
                    new BulletImpactExplosionFactory(
                        prefabFactory));

            SmallExplosionsPool = CreateAdvancedExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion75);
            MediumExplosionsPool = CreateAdvancedExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion100);
            LargeExplosionsPool = CreateAdvancedExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion150);
            HugeExplosionsPool = CreateAdvancedExplosionPool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion500);
        }

        private IPool<IExplosion, Vector3> CreateAdvancedExplosionPool(IPrefabFactory prefabFactory, ExplosionKey explosionKey)
        {
            return
                new Pool<IExplosion, Vector3>(
                    new AdvancedExplosionFactory(
                        prefabFactory,
                        explosionKey));
        }
    }
}