using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    public class ExplosionFactory : IExplosionFactory
    {
        private readonly IPrefabFactory _prefabFactory;

        public const float DEFAULT_EXPLOSION_DURATION_IN_S = 0.25f;

        public ExplosionFactory(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public IExplosion CreateDummyExplosion()
        {
            return new DummyExplosion();
        }

        public IExplosion CreateExplosion(float radiusInM, float durationInS = DEFAULT_EXPLOSION_DURATION_IN_S)
        {
            Explosion explosion = _prefabFactory.CreateExplosion();
            explosion.Initialise(radiusInM, durationInS);
            return explosion;
        }
    }
}
