using BattleCruisers.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    public class ExplosionFactory : IExplosionFactory
    {
        private readonly IPrefabFactory _prefabFactory;

        public const float DEFAULT_EXPLOSION_DURATION_IN_S = 1;

        public ExplosionFactory(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IExplosion CreateDummyExplosion()
        {
            return new DummyExplosion();
        }

        public IExplosion CreateExplosion(Transform parent, float radiusInM, float durationInS = DEFAULT_EXPLOSION_DURATION_IN_S)
        {
            Explosion explosion = _prefabFactory.CreateExplosion(parent);
            explosion.Initialise(radiusInM, durationInS);
            return explosion;
        }
    }
}
