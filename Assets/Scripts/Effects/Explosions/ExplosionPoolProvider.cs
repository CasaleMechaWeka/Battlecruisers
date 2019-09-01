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

            BulletImpactPool = CreatePool(prefabFactory, StaticPrefabKeys.Explosions.BulletImpact);
            SmallExplosionsPool = CreatePool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion75);
            MediumExplosionsPool = CreatePool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion100);
            LargeExplosionsPool = CreatePool(prefabFactory, StaticPrefabKeys.Explosions.HDExplosion150);
        }

        private IPool<IExplosion, Vector3> CreatePool(IPrefabFactory prefabFactory, ExplosionKey explosionKey)
        {
            return
                new Pool<IExplosion, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        explosionKey));
        }
    }
}