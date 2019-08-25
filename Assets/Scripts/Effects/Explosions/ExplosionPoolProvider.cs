using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        public IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        public IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        public IPool<IExplosion, Vector3> HugeExplosionsPool { get; }

        public ExplosionPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            SmallExplosionsPool
                = new Pool<IExplosion, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion75));

            MediumExplosionsPool
                = new Pool<IExplosion, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion100));

            LargeExplosionsPool
                = new Pool<IExplosion, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion150));

            HugeExplosionsPool
                = new Pool<IExplosion, Vector3>(
                    new ExplosionFactory(
                        prefabFactory,
                        StaticPrefabKeys.Explosions.HDExplosion500));
        }
    }
}