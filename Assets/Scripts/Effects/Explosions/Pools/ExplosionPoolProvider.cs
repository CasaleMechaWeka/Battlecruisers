using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<Vector3> SmallExplosionsPool { get; }

        public IPool<Vector3> MediumExplosionsPool { get; }

        public IPool<Vector3> LargeExplosionsPool { get; }

        public IPool<Vector3> HugeExplosionsPool { get; }

        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            SmallExplosionsPool
                = new Pool<Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion75));

            MediumExplosionsPool
                = new Pool<Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion100));

            LargeExplosionsPool
                = new Pool<Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion150));

            HugeExplosionsPool
                = new Pool<Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion500));
        }
    }
}