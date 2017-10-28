using BattleCruisers.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    public class ExplosionFactory : IExplosionFactory
    {
        private readonly IPrefabFactory _prefabFactory;

        public ExplosionFactory(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public void CreateExplosion(Transform parent, float radiusInM, float durationInS)
        {
            Explosion explosion = _prefabFactory.CreateExplosion(parent);
            explosion.Show(radiusInM, durationInS);
        }
    }
}
