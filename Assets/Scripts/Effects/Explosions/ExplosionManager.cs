using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionManager : IExplosionManager
    {
        private readonly IPrefabFactory _prefabFactory;

        public ExplosionManager(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        // PERF  Cache explosions, instead of creating and destroying each explosion :P
        public void ShowExplosion(IExplosionStats explosionStats, Vector2 position)
        {
            CartoonExplosion explosion = _prefabFactory.CreateCartoonExplosion(explosionStats);
            explosion.Show(position);
        }
    }
}