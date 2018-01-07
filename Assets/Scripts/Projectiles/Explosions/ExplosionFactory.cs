using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    public class ExplosionFactory : IExplosionFactory
    {
        private readonly IPrefabFactory _prefabFactory;

        private const float DAMAGE_RADIUS_TO_EXPLOSION_RADIUS_MULTIPLIER = 2;
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

        public IExplosion CreateExplosion(float damageRadiusInM, float durationInS = DEFAULT_EXPLOSION_DURATION_IN_S)
        {
            Explosion explosion = _prefabFactory.CreateExplosion();

            float explosionRadiusInM = damageRadiusInM * DAMAGE_RADIUS_TO_EXPLOSION_RADIUS_MULTIPLIER;
            explosion.Initialise(explosionRadiusInM, durationInS);

            return explosion;
        }
    }
}
